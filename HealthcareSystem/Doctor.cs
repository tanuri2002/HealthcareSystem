using System;
using System.Collections.Generic;

namespace HealthcareSystem
{
    public class Doctor
    {
        public int Id { get; set; } // Changed to settable for CSV reading
        public string Name { get; set; } // Changed to settable for CSV reading
        public string Specialization { get; set; } // Changed to settable for CSV reading
        public List<AppointmentNode> Appointments { get; }

        public Doctor Left { get; set; }
        public Doctor Right { get; set; }

        public Doctor(int id, string name, string specialization)
        {
            Id = id;
            Name = name;
            Specialization = specialization;
            Appointments = new List<AppointmentNode>();
            Left = null;
            Right = null;
        }
    }
}