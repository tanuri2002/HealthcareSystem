using System;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO;
using HealthcareSystem;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        Console.WriteLine(" _____________________________________________________ ");
        Console.WriteLine("|                                                     |");
        Console.WriteLine("|     ##      ##      ##                              |");
        Console.WriteLine("|     ##      ##      ##    MEDICARE                  |");
        Console.WriteLine("|   ######  ######  ######  HEALTHCARE APPOINTMENT    |");
        Console.WriteLine("|     ##      ##      ##    MANAGEMENT SYSTEM         |");
        Console.WriteLine("|     ##      ##      ##                              |");
        Console.WriteLine("|_____________________________________________________|");

        AppointmentList appointmentList = new AppointmentList();
        DoctorBST doctors = new DoctorBST();

        string directoryPath = @"C:\Users\User\Desktop\Third_Sem\EC 3202-Data Structures & Algorithms\Csv Files";
        string doctorsFilePath = Path.Combine(directoryPath, "doctors.csv");
        string appointmentsFilePath = Path.Combine(directoryPath, "appointments.csv");

        Directory.CreateDirectory(directoryPath);

        LoadDoctorsFromCsv(doctors, doctorsFilePath);
        LoadAppointmentsFromCsv(appointmentList, doctors, appointmentsFilePath);

        while (true)
        {
            Console.WriteLine("\n1. Add New Appointment");
            Console.WriteLine("2. Assign a Patient to a Doctor");
            Console.WriteLine("3. Cancel Appointment");
            Console.WriteLine("4. Display All Appointments");
            Console.WriteLine("5. Add New Doctor");
            Console.WriteLine("6. Display All Doctors");
            Console.WriteLine("7. Display Sorted Appointments for a Doctor");
            Console.WriteLine("8. Exit");
            Console.Write("\nEnter your choice: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    appointmentList.AddAppointment(doctors);
                    break;
                case "2":
                    Console.Write("Enter Appointment ID to assign: ");
                    if (!int.TryParse(Console.ReadLine(), out int appointmentId))
                    {
                        Console.WriteLine("⚠ Invalid Appointment ID format.");
                        break;
                    }
                    appointmentList.AssignPatientToDoctor(appointmentId, doctors);
                    break;
                case "3":
                    Console.Write("Enter Appointment ID to Cancel: ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                        appointmentList.CancelAppointment(id);
                    else
                        Console.WriteLine("⚠️ Invalid ID format.");
                    break;
                case "4":
                    appointmentList.DisplayAppointments();
                    break;                
                case "5":
                    Console.Write("Enter Doctor's Name: ");
                    string doctorName = Console.ReadLine();
                    Console.Write("Enter Doctor's Specialization: ");
                    string specialization = Console.ReadLine();
                    doctors.AddDoctor(doctorName, specialization);
                    break;
                case "6":
                    doctors.DisplayDoctors();
                    break;
                case "7":
                    Console.Write("Enter Doctor ID: ");
                    int doctorId;
                    if (int.TryParse(Console.ReadLine(), out doctorId))
                    {
                        doctors.DisplayDoctorAppointments(doctorId);
                    }
                    else
                    {
                        Console.WriteLine("⚠️ Invalid Doctor ID format.");
                    }
                    break;
                case "8":
                    Console.WriteLine("🔚 Exiting...");
                    return;
                default:
                    Console.WriteLine("⚠️ Invalid choice. Try again.");
                    break;
            }
        }
        List<AppointmentNode> testList = new List<AppointmentNode>();
        for (int i = 0; i < 100; i++)
        {
            testList.Add(new AppointmentNode(i, $"Patient{i}", 30, "Condition", 1));
        }

        // Test each sorting method
        TestSorting(testList, BubbleSort, "Bubble Sort");
        TestSorting(testList, MergeSortWrapper, "Merge Sort");
        TestSorting(testList, QuickSortWrapper, "Quick Sort");
        //TestSorting(testList, ListSort, "List.Sort");

        // Helper methods
        static void BubbleSort(List<AppointmentNode> list) { /* Your Bubble Sort code */ }
        static void MergeSortWrapper(List<AppointmentNode> list) { /* Your Merge Sort code */ }
        static void QuickSortWrapper(List<AppointmentNode> list) { /* Your Quick Sort code */ }
        //static void ListSort(List<AppointmentNode> list) { list.Sort((a, b) => a.Id.CompareTo(b.Id)); }
    }

    static void LoadDoctorsFromCsv(DoctorBST doctors, string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    BadDataFound = context => // Older versions use Configuration.BadDataFound
                    {
                        Console.WriteLine($"⚠️ Bad data found in doctors.csv: {context.RawRecord}");
                    }
                };

                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, config))
                {
                    var records = new List<Doctor>();
                    csv.Read(); // Read the header
                    csv.ReadHeader();
                    while (csv.Read())
                    {
                        try
                        {
                            var doctor = new Doctor(
                                csv.GetField<int>("Id"),
                                csv.GetField<string>("Name"),
                                csv.GetField<string>("Specialization")
                            );
                            records.Add(doctor);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"⚠️ Error parsing doctor row: {ex.Message}");
                            Console.WriteLine($"Raw data: {csv.GetRecord<string>()}");
                        }
                    }
                    foreach (var doctor in records)
                    {
                        doctors.AddDoctorFromCsv(doctor.Id, doctor.Name, doctor.Specialization);
                    }
                    //Console.WriteLine($"✅ Loaded {records.Count} doctors from {filePath}");
                }
            }
            else
            {
                Console.WriteLine($"⚠️ Doctors CSV not found at {filePath}. Starting with empty doctor list.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ Error loading doctors from CSV: {ex.Message}");
        }
    }

    static void LoadAppointmentsFromCsv(AppointmentList appointmentList, DoctorBST doctors, string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    BadDataFound = context => // Older versions use Configuration.BadDataFound
                    {
                        Console.WriteLine($"⚠️ Bad data found in appointments.csv: {context.RawRecord}");
                    }
                };

                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, config))
                {
                    var records = new List<AppointmentNode>();
                    csv.Read(); // Read the header
                    csv.ReadHeader();
                    while (csv.Read())
                    {
                        try
                        {
                            var appointment = new AppointmentNode(
                                csv.GetField<int>("Id"),
                                csv.GetField<string>("PatientName"),
                                csv.GetField<int>("PatientAge"),
                                csv.GetField<string>("PatientCondition"),
                                csv.GetField<int>("DoctorId")
                            );
                            records.Add(appointment);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"⚠️ Error parsing appointment row: {ex.Message}");
                            Console.WriteLine($"Raw data: {csv.GetRecord<string>()}");
                        }
                    }
                    foreach (var appointment in records)
                    {
                        appointmentList.AddAppointmentFromCsv(appointment, doctors);
                    }
                    //Console.WriteLine($"✅ Loaded {records.Count} appointments from {filePath}");
                }
            }
            else
            {
                Console.WriteLine($"⚠️ Appointments CSV not found at {filePath}. Starting with empty appointment list.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ Error loading appointments from CSV: {ex.Message}");
        }
    }
    public static void TestSorting(List<AppointmentNode> appointments, Action<List<AppointmentNode>> sortMethod, string methodName)
    {
        List<AppointmentNode> copy = new List<AppointmentNode>(appointments);
        Stopwatch stopwatch = Stopwatch.StartNew();
        sortMethod(copy);
        stopwatch.Stop();
        Console.WriteLine($"{methodName} for {appointments.Count} items: {stopwatch.Elapsed.TotalMilliseconds} ms");
    }
}