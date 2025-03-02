using System;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO;
using HealthcareSystem;

class Program
{
    static void Main()
    {
        Console.WriteLine(" __________________________________________________________ ");
        Console.WriteLine("|                                                          |");
        Console.WriteLine("|       #####    #####   #####                             |");
        Console.WriteLine("|       #   #    #   #   #   #   MEDICARE                  |");
        Console.WriteLine("|       #   #    #   #   #   #   HEALTHCARE APPOINTMENT    |");
        Console.WriteLine("|       #####    #####   #####   MANAGEMENT SYSTEM         |");
        Console.WriteLine("|                                                          |");
        Console.WriteLine("|__________________________________________________________|");

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
            Console.WriteLine("5. View Doctor Availability");
            Console.WriteLine("6. Add New Doctor");
            Console.WriteLine("7. Display All Doctors");
            Console.WriteLine("8. Display Sorted Appointments for a Doctor");
            Console.WriteLine("9. Exit");
            Console.Write("Enter your choice: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    appointmentList.AddAppointment(doctors);
                    break;
                case "2":
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
                    //doctors.DisplayDoctors();
                    break;
                case "6":
                    Console.Write("Enter Doctor's Name: ");
                    string doctorName = Console.ReadLine();
                    Console.Write("Enter Doctor's Specialization: ");
                    string specialization = Console.ReadLine();
                    doctors.AddDoctor(doctorName, specialization);
                    break;
                case "7":
                    doctors.DisplayDoctors();
                    break;
                case "8":
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
                case "9":
                    Console.WriteLine("🔚 Exiting...");
                    return;
                default:
                    Console.WriteLine("⚠️ Invalid choice. Try again.");
                    break;
            }
        }
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
                    Console.WriteLine($"✅ Loaded {records.Count} doctors from {filePath}");
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
                    Console.WriteLine($"✅ Loaded {records.Count} appointments from {filePath}");
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
}