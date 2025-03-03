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
        
        /// ////////////////////////////////////////////////////////////////////////////////////
        

        //Bubble Sort
        /*
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
        }*/
     /////////////////////////////////////////////////////////////////////////////////////////////////////////////       
    //Quick Sort
        
    private void SortAppointmentsByPatientId(List<AppointmentNode> appointments){
            if (appointments == null || appointments.Count <= 1)
                return; // Base case: already sorted or empty

            QuickSort(appointments, 0, appointments.Count - 1);
    }

    private void QuickSort(List<AppointmentNode> appointments, int low, int high){
            if (low < high){

            // Partition the list and get the pivot index    
                int pivotIndex = Partition(appointments, low, high);

            // Recursively sort the sublists
                QuickSort(appointments, low, pivotIndex - 1);  // Sort left sublist
                QuickSort(appointments, pivotIndex + 1, high); // Sort right sublist
            }
    }

    private int Partition(List<AppointmentNode> appointments, int low, int high){
            // Choose the last element as the pivot
                int pivot = appointments[high].Id;
                int i = low - 1; // Index of the smaller element

            for (int j = low; j < high; j++){
                // If the current element is smaller than or equal to the pivot
                    if (appointments[j].Id <= pivot){
                           i++;

                            // Swap appointments[i] and appointments[j]
                                    var temp = appointments[i];
                                    appointments[i] = appointments[j];
                                    appointments[j] = temp;
                    }
            }

            // Swap the pivot element with the element at i+1

            var tempPivot = appointments[i + 1];
            appointments[i + 1] = appointments[high];
            appointments[high] = tempPivot;

            return i + 1; // Return the pivot index
    }/*
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Merge Sort
    private void SortAppointmentsByPatientId(List<AppointmentNode> appointments){
        if (appointments == null || appointments.Count <= 1)
        return; // Base case: already sorted or empty

        MergeSort(appointments, 0, appointments.Count - 1);
    }

    private void MergeSort(List<AppointmentNode> appointments, int left, int right){
        if (left < right){

        // Find the middle point
        int mid = left + (right - left) / 2;

        // Recursively sort the two halves
        MergeSort(appointments, left, mid);      // Sort left half
        MergeSort(appointments, mid + 1, right); // Sort right half

        // Merge the sorted halves
        Merge(appointments, left, mid, right);
        }
    }

    private void Merge(List<AppointmentNode> appointments, int left, int mid, int right){
        // Create temporary arrays for the two halves
            int n1 = mid - left + 1; // Size of the left half
            int n2 = right - mid;    // Size of the right half

            var leftArray = new AppointmentNode[n1];
            var rightArray = new AppointmentNode[n2];

       // Copy data to temporary arrays
            for (int i = 0; i < n1; i++)
                leftArray[i] = appointments[left + i];

            for (int j = 0; j < n2; j++)
                rightArray[j] = appointments[mid + 1 + j];

      // Merge the two arrays back into the original list
            int iLeft = 0, iRight = 0; // Initial indexes of the two subarrays
            int k = left;              // Initial index of the merged array

        while (iLeft < n1 && iRight < n2){
            if (leftArray[iLeft].Id <= rightArray[iRight].Id){
                appointments[k] = leftArray[iLeft];
                iLeft++;
            }
            else{
                appointments[k] = rightArray[iRight];
                iRight++;
            }
            k++;
        }

        // Copy remaining elements of leftArray (if any)
        while (iLeft < n1){
            appointments[k] = leftArray[iLeft];
            iLeft++;
            k++;
        }

        // Copy remaining elements of rightArray (if any)
        while (iRight < n2){
            appointments[k] = rightArray[iRight];
            iRight++;
            k++;
        }
    }*/
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        

        public void DisplayDoctorAppointments(int doctorId)
        {
            Doctor doctor = FindDoctorById(doctorId);
            if (doctor == null)
            {
                Console.WriteLine("⚠ Doctor not found.");
                return;
            }
            var activeAppointments = doctor.Appointments.FindAll(a => a.Status != "Assigned"); // Only show non-assigned appointments
            if (activeAppointments.Count == 0)
            {
                Console.WriteLine($"👨‍⚕ Dr. {doctor.Name} (ID: {doctor.Id}, Specialization: {doctor.Specialization}) has no active appointments.");
                return;
            }

            SortAppointmentsByPatientId(activeAppointments);

            Console.WriteLine($"\n📋 Active Appointments for Dr. {doctor.Name} (ID: {doctor.Id}, Specialization: {doctor.Specialization}):");
            foreach (var appointment in activeAppointments)
            {
                Console.WriteLine($"   🔹 {appointment.PatientName} (ID: {appointment.Id}, Age: {appointment.PatientAge}, Condition: {appointment.PatientCondition}, Status: {appointment.Status})");
            }
        }
    }
}