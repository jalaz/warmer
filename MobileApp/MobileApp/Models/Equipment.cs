using System;

namespace MobileApp.Models
{
    public class Equipment
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsOn { get; set; }  
    }
}