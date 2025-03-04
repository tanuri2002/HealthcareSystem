using System;
using System.Collections.Generic;
using System.Diagnostics;

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
        private void SortAppointmentsByPatientId(List<AppointmentNode> appointments, string methodName)
        {
            if (appointments.Count <= 1)
            {
                Console.WriteLine($"{methodName} for {appointments.Count} items: 0 ms (list too small)");
                return;
            }

            Stopwatch stopwatch = Stopwatch.StartNew();

            // Choose one sorting algorithm (e.g., Merge Sort)
            BubbleSort(appointments, 0, appointments.Count - 1);

            stopwatch.Stop();
            Console.WriteLine($"{methodName} for {appointments.Count} items: {stopwatch.Elapsed.TotalMilliseconds} ms");
        }
       
       
        /////////////////////////////////////////////////////////////////////////////////////////     
        //Merge Sort
        /*private void MergeSort(List<AppointmentNode> appointments, int left, int right)
        {
            if (left < right)
            {
                int mid = (left + right) / 2;
                MergeSort(appointments, left, mid);
                MergeSort(appointments, mid + 1, right);
                Merge(appointments, left, mid, right);
            }
        }

        private void Merge(List<AppointmentNode> appointments, int left, int mid, int right)
        {
            List<AppointmentNode> temp = new List<AppointmentNode>(right - left + 1);
            int i = left, j = mid + 1;

            while (i <= mid && j <= right)
            {
                if (appointments[i].Id <= appointments[j].Id)
                {
                    temp.Add(appointments[i++]);
                }
                else
                {
                    temp.Add(appointments[j++]);
                }
            }

            while (i <= mid) temp.Add(appointments[i++]);
            while (j <= right) temp.Add(appointments[j++]);

            for (int k = 0; k < temp.Count; k++)
                appointments[left + k] = temp[k];
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
        ///Quick Sort
   /*
   private void QuickSort(List<AppointmentNode> appointments, int low, int high)
        {
            if (low < high)
            {
                int pivotIndex = Partition(appointments, low, high);
                QuickSort(appointments, low, pivotIndex - 1);
                QuickSort(appointments, pivotIndex + 1, high);
            }
        }

        private int Partition(List<AppointmentNode> appointments, int low, int high)
        {
            int pivot = appointments[high].Id;
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                if (appointments[j].Id <= pivot)
                {
                    i++;
                    var temp = appointments[i];
                    appointments[i] = appointments[j];
                    appointments[j] = temp;
                }
            }

            var tempPivot = appointments[i + 1];
            appointments[i + 1] = appointments[high];
            appointments[high] = tempPivot;

            return i + 1;
        }*/

   //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
   //Bubble Sort
       private void BubbleSort(List<AppointmentNode> appointments,int low, int high)
        {
            //int n = appointments.Count;
            int n = high - low + 1; // Number of elements in the range
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
                Console.WriteLine("⚠ Doctor not found.");
                return;
            }

            if (doctor.Appointments.Count == 0)
            {
                Console.WriteLine($"👨‍⚕ Dr. {doctor.Name} (ID: {doctor.Id}, Specialization: {doctor.Specialization}) has no appointments.");
                return;
            }

            // Test different sorting algorithms by uncommenting one
            //SortAppointmentsByPatientId(doctor.Appointments, "Merge Sort");
            //SortAppointmentsByPatientId(doctor.Appointments, "Quick Sort");
            SortAppointmentsByPatientId(doctor.Appointments, "Bubble Sort"); // Default for now

            Console.WriteLine($"\n📋 Appointments for Dr. {doctor.Name} (ID: {doctor.Id}, Specialization: {doctor.Specialization}):");
            foreach (var appointment in doctor.Appointments)
            {
                Console.WriteLine($"   🔹 {appointment.PatientName} (ID: {appointment.Id}, Age: {appointment.PatientAge}, Condition: {appointment.PatientCondition})");
            }
        }



    }

}
