using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using MobileApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using System.Net.Http;
using System.Net;

namespace MobileApp
{
    public class HomePage : ContentPage
    {
        public HomePage()
        {
            Label header = new Label
            {
                Text = "Equipment control",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };

            var equipments = new List<Equipment>
            {
                new Equipment {Id = Guid.NewGuid(), Name = "Car heater"},
                new Equipment {Id = Guid.NewGuid(), Name = "Ventilation", IsOn = true}
            };

            var tableView = DrawEquipment(equipments);

            // Accomodate iPhone status bar.
            this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);

            // Build the page.
            this.Content = new StackLayout
            {
                Children =
                {
                    header,
                    tableView
                }
            };
        }

        private TableView DrawEquipment(IEnumerable<Equipment> equipments)
        {
            var tableView = new TableView
            {
                Intent = TableIntent.Form,
                Root = new TableRoot {}

            };
            foreach (var equipment in equipments)
            {
                var cell = new SwitchCell
                {
                    Text = equipment.Name,
                    On = equipment.IsOn,
                    StyleId = equipment.Id.ToString()

                };
                cell.OnChanged += OnCellChanged;
                tableView.Root.Add(new TableSection {cell});
            }
            return tableView;
        }

        private void OnCellChanged(object sender, ToggledEventArgs e)
        {
            var cell = sender as SwitchCell;
            if (cell == null)
            {
                System.Diagnostics.Debug.WriteLine("Attemp to switch undefined object");
                return;
            }

            System.Diagnostics.Debug.WriteLine("Attemp to switch " + (cell.On ? "On " : "Off ") + cell.Text);
            SwitchEquipment(cell);
        }

        private async void SwitchEquipment(SwitchCell cell)
        {
            var id = Guid.Parse(cell.StyleId);
            var isOn = cell.On;
            bool currentState = isOn;
            var client = new HttpClient();
            var deviceUrl = "http://192.168.4.1/device?turnOn=";
            deviceUrl += isOn ? "1" : "0";
            try
            {
                var response = await client.GetAsync(new Uri(deviceUrl));
                if (!response.IsSuccessStatusCode)
                    System.Diagnostics.Debug.WriteLine("Error. Failed to switch. The responce is not ok.");
                var message = await response.Content.ReadAsStringAsync();


                if (message.Contains("OFF"))
                    currentState = false;
                else if (message.Contains("ON"))
                {
                    currentState = true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error. Failed to switch." + ex.ToString());
                return;
            }
            if (isOn != currentState)
                    UpdateSwitchCell(cell, currentState);
            
        }

        private void UpdateSwitchCell(SwitchCell cell, bool currentState)
        {
            cell.On = currentState;
        }
    }
}
