using System;

namespace HealthcareSystem
{
    public class AppointmentList
    {
        private AppointmentNode head;
        private int idCounter = 1;

        public void AddAppointment(DoctorBST doctors)
        {
            Console.Write("Enter Doctor ID for the appointment: ");
            if (!int.TryParse(Console.ReadLine(), out int doctorId))
            {
                Console.WriteLine("⚠️ Invalid Doctor ID format.");
                return;
            }

            Doctor selectedDoctor = doctors.FindDoctorById(doctorId);
            if (selectedDoctor == null)
            {
                Console.WriteLine("⚠️ Doctor not found!");
                return;
            }

            Console.Write("Enter Patient Name: ");
            string patientName = Console.ReadLine();
            Console.Write("Enter Patient Age: ");
            if (!int.TryParse(Console.ReadLine(), out int patientAge))
            {
                Console.WriteLine("⚠️ Invalid age format.");
                return;
            }
            Console.Write("Enter Patient Condition: ");
            string patientCondition = Console.ReadLine();

            AppointmentNode newAppointment = new AppointmentNode(idCounter++, patientName, patientAge, patientCondition, doctorId);

            if (head == null)
            {
                head = newAppointment;
            }
            else
            {
                AppointmentNode temp = head;
                while (temp.Next != null)
                {
                    temp = temp.Next;
                }
                temp.Next = newAppointment;
            }

            selectedDoctor.Appointments.Add(newAppointment);

            Console.WriteLine($"✅ Appointment added: {newAppointment.PatientName} (ID: {newAppointment.Id}) with Dr. {selectedDoctor.Name}");
        }

        // New method to add appointment from CSV
        public void AddAppointmentFromCsv(AppointmentNode appointment, DoctorBST doctors)
        {
            Doctor selectedDoctor = doctors.FindDoctorById(appointment.DoctorId);
            if (selectedDoctor == null)
            {
                Console.WriteLine($"⚠️ Doctor ID {appointment.DoctorId} not found for appointment ID {appointment.Id}. Skipping.");
                return;
            }

            if (head == null)
            {
                head = appointment;
            }
            else
            {
                AppointmentNode temp = head;
                while (temp.Next != null)
                {
                    temp = temp.Next;
                }
                temp.Next = appointment;
            }

            selectedDoctor.Appointments.Add(appointment);

            if (appointment.Id >= idCounter) idCounter = appointment.Id + 1; // Update idCounter if CSV ID is higher
        }

        public void CancelAppointment(int id)
        {
            if (head == null)
            {
                Console.WriteLine("⚠️ No appointments found.");
                return;
            }

            if (head.Id == id)
            {
                Console.WriteLine($"❌ Appointment canceled: {head.PatientName} (ID: {head.Id})");
                head = head.Next;
                return;
            }

            AppointmentNode temp = head, prev = null;
            while (temp != null && temp.Id != id)
            {
                prev = temp;
                temp = temp.Next;
            }

            if (temp == null)
            {
                Console.WriteLine($"⚠️ Appointment ID {id} not found.");
                return;
            }

            prev.Next = temp.Next;
            Console.WriteLine($"❌ Appointment canceled: {temp.PatientName} (ID: {temp.Id})");
        }

        public void DisplayAppointments()
        {
            if (head == null)
            {
                Console.WriteLine("⚠️ No upcoming appointments.");
                return;
            }

            Console.WriteLine("\n📋 Upcoming Appointments:");
            AppointmentNode temp = head;
            while (temp != null)
            {
                Console.WriteLine($"🔹 {temp.PatientName} (ID: {temp.Id}, Age: {temp.PatientAge}, Condition: {temp.PatientCondition}, Doctor ID: {temp.DoctorId})");
                temp = temp.Next;
            }
        }
    }
}