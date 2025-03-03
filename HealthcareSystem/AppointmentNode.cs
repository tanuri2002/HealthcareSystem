using System;

namespace HealthcareSystem
{
    public class AppointmentNode
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public int PatientAge { get; set; }
        public string PatientCondition { get; set; }
        public int DoctorId { get; set; }

        public string Status { get; set; } // New property: e.g., "Pending", "Assigned", "Canceled"
        public AppointmentNode Next { get; set; }

        public AppointmentNode(int id, string name, int age, string condition, int doctorId)
        {
            Id = id;
            PatientName = name;
            PatientAge = age;
            PatientCondition = condition;
            DoctorId = doctorId;
            Next = null;
            Status = "Pending"; // Default status
        }
    }
}