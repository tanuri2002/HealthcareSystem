using System;
using System.Collections.Generic;

namespace HealthcareSystem
{
    public class DoctorBST
    {
        private Doctor root;
        private int idCounter = 1;

        public void AddDoctor(string name, string specialization)
        {
            root = Insert(root, name, specialization);
        }

        // New method to add doctor with specific ID from CSV
        public void AddDoctorFromCsv(int id, string name, string specialization)
        {
            root = InsertFromCsv(root, id, name, specialization);
            if (id >= idCounter) idCounter = id + 1; // Update idCounter if CSV ID is higher
        }

        private Doctor Insert(Doctor node, string name, string specialization)
        {
            if (node == null)
            {
                Console.WriteLine($"✅ Doctor added: Dr. {name} (ID: {idCounter}, Specialization: {specialization})");
                return new Doctor(idCounter++, name, specialization);
            }

            if (idCounter < node.Id)
                node.Left = Insert(node.Left, name, specialization);
            else
                node.Right = Insert(node.Right, name, specialization);

            return node;
        }

        private Doctor InsertFromCsv(Doctor node, int id, string name, string specialization)
        {
            if (node == null)
            {
                return new Doctor(id, name, specialization);
            }

            if (id < node.Id)
                node.Left = InsertFromCsv(node.Left, id, name, specialization);
            else
                node.Right = InsertFromCsv(node.Right, id, name, specialization);

            return node;
        }

        public void DisplayDoctors()
        {
            if (root == null)
            {
                Console.WriteLine("⚠️ No doctors available.");
                return;
            }
            Console.WriteLine("\n👨‍⚕️ Available Doctors:");
            InOrderTraversal(root);
        }

        private void InOrderTraversal(Doctor node)
        {
            if (node != null)
            {
                InOrderTraversal(node.Left);
                Console.WriteLine($"   🔹 Dr. {node.Name} (ID: {node.Id}, Specialization: {node.Specialization})");
                InOrderTraversal(node.Right);
            }
        }

        public Doctor FindDoctorById(int id)
        {
            return SearchDoctor(root, id);
        }

        private Doctor SearchDoctor(Doctor node, int id)
        {
            if (node == null || node.Id == id)
                return node;

            if (id < node.Id)
                return SearchDoctor(node.Left, id);
            else
                return SearchDoctor(node.Right, id);
        }

        private void SortAppointmentsByPatientId(List<AppointmentNode> appointments)
        {
            int n = appointments.Count;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (appointments[j].Id > appointments[j + 1].Id)
                    {
                        var temp = appointments[j];
                        appointments[j] = appointments[j + 1];
                        appointments[j + 1] = temp;
                    }
                }
            }
        }

        public void DisplayDoctorAppointments(int doctorId)
        {
            Doctor doctor = FindDoctorById(doctorId);
            if (doctor == null)
            {
                Console.WriteLine("⚠️ Doctor not found.");
                return;
            }

            if (doctor.Appointments.Count == 0)
            {
                Console.WriteLine($"👨‍⚕️ Dr. {doctor.Name} (ID: {doctor.Id}, Specialization: {doctor.Specialization}) has no appointments.");
                return;
            }

            SortAppointmentsByPatientId(doctor.Appointments);

            Console.WriteLine($"\n📋 Appointments for Dr. {doctor.Name} (ID: {doctor.Id}, Specialization: {doctor.Specialization}):");
            foreach (var appointment in doctor.Appointments)
            {
                Console.WriteLine($"   🔹 {appointment.PatientName} (ID: {appointment.Id}, Age: {appointment.PatientAge}, Condition: {appointment.PatientCondition})");
            }
        }
    }
}