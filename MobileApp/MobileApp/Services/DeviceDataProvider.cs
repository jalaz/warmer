using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MobileApp.Models;
using Newtonsoft.Json;

namespace MobileApp.Services
{
    public class DeviceDataProvider
    {
        private readonly string _deviceUrl;

        public DeviceDataProvider(string deviceUrl)
        {
            _deviceUrl = deviceUrl;
        }

        public async Task<IEnumerable<Equipment>> GetEquipments()
        {
            var client = new HttpClient();
            var deviceUrl = _deviceUrl + "/equipmentList";
            try
            {
                var response = await client.GetAsync(new Uri(deviceUrl));
                if (!response.IsSuccessStatusCode)
                    System.Diagnostics.Debug.WriteLine("Error. Failed to get equipment. The responce is not ok.");
                var message = await response.Content.ReadAsStringAsync();

                var equipmentList =  JsonConvert.DeserializeObject<IEnumerable<Equipment>>(message);
                if(equipmentList==null)
                    return new List<Equipment>();
                return equipmentList;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error. Failed to switch." + ex.ToString());
                return new List<Equipment>();
            }
        }

        public async Task<Equipment> SwitchEquipment(Guid id, bool isOn)
        {
            var client = new HttpClient();
            var deviceUrl = _deviceUrl + $"device?id={id}&turnOn=";
            deviceUrl += isOn ? "1" : "0";
            try
            {
                var response = await client.GetAsync(new Uri(deviceUrl));
                if (!response.IsSuccessStatusCode)
                    System.Diagnostics.Debug.WriteLine("Error. Failed to switch. The responce is not ok.");
                var message = await response.Content.ReadAsStringAsync();


                var equipment = JsonConvert.DeserializeObject<Equipment>(message);
                return equipment;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error. Failed to switch." + ex.ToString());
                return null;
            }
        }


    }
}