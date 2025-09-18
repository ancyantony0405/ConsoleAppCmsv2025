using ConsoleAppCmsv2025.Model;
using ConsoleAppCmsv2025.Repository;
using ConsoleAppCmsv2025.Service;
using ConsoleAppCmsv2025.Utility;
using ConsoleAppCmsv2025.ViewModel;
using System;
using System.Numerics;

namespace ConsoleAppCmsv2025
{
    internal class CmsvApp2025
    {
        static async Task Main(string[] args)
        {
            while (true) // Always return to login screen
            {
                Console.Clear();
            lblUserName:
                Console.WriteLine("---------------------");
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("    L  O   G  I  N   ");
                Console.ResetColor();
                Console.WriteLine("---------------------");
                Console.WriteLine();

                // Username
                Console.Write("Enter Username: ");
                string username = Console.ReadLine();

                if (!CustomValidation.IsValidUsername(username))
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Username, Try again");
                    Console.ResetColor();
                    goto lblUserName;
                }

            lblPassword:
                // Password
                Console.Write("Enter Password: ");
                string password = CustomValidation.ReadPassword();

                if (!CustomValidation.IsValidPassword(password))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Password, Try again");
                    Console.ResetColor();
                    goto lblPassword;
                }

                // Authenticate using Service/Repository
                IUserService userService = new UserServiceImpl(new UserRepositoryImpl());
                int resultRoleId = await userService.AuthenticateUserByRoleIdAsyn(username, password);

                if (resultRoleId >= 1)
                {
                    switch (resultRoleId)
                    {
                        // Receptionist
                        case 1: 
                            await ShowReceptionistMenuAsync();
                            break;

                        case 2: // Doctor
                            ShowDoctorMenu();
                            break;

                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid RoleId : ACCESS DENIED");
                            Console.ResetColor();
                            break;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Credentials! Please try again.");
                    Console.ResetColor();
                }

                Console.WriteLine("\nPress any key to go back to Login...");
                Console.ReadKey();
            }
        }

        private static async Task ShowReceptionistMenuAsync()
        {
            var patientRepo = new PatientRepositoryImpl();

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("------------------------------");
            Console.WriteLine("    Receptionist Dashboard    ");
            Console.WriteLine("------------------------------");
            Console.ResetColor();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("1. Search Patient by MMR");
            Console.WriteLine("2. Search Patient by PhoneNumber");
            Console.WriteLine("3. Patient Registration");
            Console.WriteLine("4. Logout");
            Console.ResetColor();
            Console.WriteLine();

            Console.Write("\nEnter choice: ");
            string choice = Console.ReadLine();

            PatientViewModel patient = null;

            switch (choice)
            {
                case "1":
                    Console.Write("Enter Unique Number in MMR : ");
                    string uniquePart = Console.ReadLine();

                    // Hardcode prefix "MMR" and last two digits of year
                    string prefix = "MMR" + DateTime.Now.Year.ToString().Substring(2, 2);

                    // Combine to get full MMR number
                    string mmr = prefix + uniquePart;

                    Console.WriteLine($"Patient MMR Number: {mmr}");

                    // Search patient
                    patient = await patientRepo.SearchPatientByMMRAsync(mmr);
                    break;

                case "2":
                    Console.Write("Enter Phone Number: ");
                    string phone = Console.ReadLine();
                    patient = await patientRepo.SearchPatientByPhoneAsync(phone);
                    break;

                case "3":
                    patient = await RegisterNewPatientAsync(patientRepo);
                    break;

                case "4":
                    Console.WriteLine("Logging out...");
                    return;

                default:
                    Console.WriteLine("Invalid choice!");
                    return;
            }

            // If patient not found, add new patient
            if (patient == null)
            {
                Console.WriteLine("\nPatient not found. Adding new patient...");

                Console.Write("Enter Name: ");
                string name = Console.ReadLine();

                Console.Write("Enter DOB (yyyy-MM-dd): ");
                DateTime dob = DateTime.Parse(Console.ReadLine());

                Console.Write("Enter Gender (M/F/O): ");
                string gender = Console.ReadLine();

                Console.Write("Enter Phone: ");
                string phoneNumber = Console.ReadLine();

                Console.Write("Enter Address: ");
                string address = Console.ReadLine();

                var patientInput = new PatientInputModel
                {
                    Name = name,
                    DateOfBirth = dob,
                    Gender = gender,
                    Phone = phoneNumber,
                    Address = address
                };

                // Add patient to DB
                (int patientId, string mmrNumber) = await patientRepo.AddPatientAsync(patientInput);

                patient = new PatientViewModel
                {
                    PatientId = patientId,
                    Name = name,
                    DateOfBirth = dob,
                    Gender = gender,
                    Phone = phoneNumber,
                    Address = address,
                    MMRNumber = mmrNumber
                };

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Patient added successfully. MMR Number: {mmrNumber}");
                Console.ResetColor();

                var memberships = await patientRepo.GetMembershipsAsync();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n--- Available Memberships ---");
                Console.WriteLine();
                foreach (var m in memberships)
                {
                    Console.WriteLine($"{m.MembershipId}. {m.MembershipName} - {m.DiscountPercent}% Discount, {m.DurationMonths} months, Fee: {m.Fee}");
                }
                Console.ResetColor();
                Console.Write("Enter Membership Id to assign (or 0 to skip): ");
                int memId = int.Parse(Console.ReadLine());
                if (memId > 0)
                {
                    await patientRepo.AssignMembershipAsync(patient.PatientId, memId);
                    Console.WriteLine("Membership assigned successfully!");
                }
            }
            else
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("--- Patient Details ---");
                Console.WriteLine();
                Console.WriteLine("{0,-5} | {1,-12} | {2,-10} | {3,-6} | {4,-10} | {5,-12} | {6,-6} | {7,-6} | {8,-12}",
                    "ID", "Name", "DOB", "Gen", "MMR", "Phone", "Total", "Paid", "Membership");

                Console.WriteLine(new string('-', 90));

                Console.WriteLine("{0,-5} | {1,-12} | {2,-10:yyyy-MM-dd} | {3,-6} | {4,-10} | {5,-12} | {6,-6} | {7,-6} | {8,-12}",
                    patient.PatientId, patient.Name, patient.DateOfBirth, patient.Gender,
                    patient.MMRNumber, patient.Phone, patient.TotalAmount, patient.AmountPaid,
                    string.IsNullOrEmpty(patient.MembershipName) ? "None" : patient.MembershipName);
                Console.ResetColor();
            }

            // Prompt Doctor Id
            int doctorId;
            while (true)
            {
                Console.WriteLine();
                Console.Write("\nEnter Doctor Id to book appointment (1. Cardiologist / 2. Neurologist / 3. Orthopedist / 4.General Physician): ");
                if (int.TryParse(Console.ReadLine(), out doctorId) && doctorId > 0)
                    break;
                Console.WriteLine("Invalid Doctor Id. Try again.");
            }

            // Prompt Appointment Date
            DateTime appointmentDate;
            while (true)
            {
                Console.Write("Enter Appointment DateTime (yyyy-MM-dd HH:mm): ");
                string dateInput = Console.ReadLine();
                if (DateTime.TryParse(dateInput, out appointmentDate))
                {
                    if (appointmentDate >= new DateTime(1753, 1, 1))
                        break;
                    Console.WriteLine("Date must be after 1/1/1753. Try again.");
                }
                else
                {
                    Console.WriteLine("Invalid date format. Try again.");
                }
            }

            // Book appointment
            var result = await patientRepo.BookAppointmentAsync(patient.PatientId, doctorId, appointmentDate);
            int appointmentId = result.AppointmentId;
            int tokenNo = result.TokenNo;

            // Handle booking result
            if (appointmentId == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Appointment failed! Doctor is busy at this time. Please choose a different time slot.");
                Console.ResetColor();
            }
            else if (appointmentId == -1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Appointment failed! You already have an overlapping appointment at this time.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Appointment booked successfully. Appointment Id: {appointmentId}, Token No: {tokenNo}");
                Console.ResetColor();
            }
        }

        #region Patient Registration
        private static async Task<PatientViewModel> RegisterNewPatientAsync(PatientRepositoryImpl patientRepo)
        {
            Console.WriteLine("\n--- New Patient Registration ---");
            Console.WriteLine();

            Console.Write("Enter Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter DOB (yyyy-MM-dd): ");
            DateTime dob = DateTime.Parse(Console.ReadLine());

            Console.Write("Enter Gender (M/F/O): ");
            string gender = Console.ReadLine();

            Console.Write("Enter Phone: ");
            string phone = Console.ReadLine();

            Console.Write("Enter Address: ");
            string address = Console.ReadLine();

            var patientInput = new PatientInputModel
            {
                Name = name,
                DateOfBirth = dob,
                Gender = gender,
                Phone = phone,
                Address = address
            };

            (int patientId, string mmrNumber) = await patientRepo.AddPatientAsync(patientInput);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Patient registered successfully. Assigned MMR: {mmrNumber}");
            Console.ResetColor();

            // Offer membership immediately after registration
            var memberships = await patientRepo.GetMembershipsAsync();
            Console.WriteLine("\n--- Available Memberships ---");
            Console.WriteLine();
            foreach (var m in memberships)
            {
                Console.WriteLine($"{m.MembershipId}. {m.MembershipName} - {m.DiscountPercent}% Discount, {m.DurationMonths} months, Fee: {m.Fee}");
            }

            Console.Write("Enter Membership Id to assign (or 0 to skip): ");
            int memId = int.Parse(Console.ReadLine());
            if (memId > 0)
            {
                await patientRepo.AssignMembershipAsync(patientId, memId);
                Console.WriteLine("Membership assigned successfully!");
            }

            return new PatientViewModel
            {
                PatientId = patientId,
                Name = name,
                DateOfBirth = dob,
                Gender = gender,
                Phone = phone,
                Address = address,
                MMRNumber = mmrNumber
            };
        }
        #endregion
        private static void ShowDoctorMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("------------------------------");
            Console.WriteLine("    Doctor Dashboard    ");
            Console.WriteLine("------------------------------");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("1. View Appointments");
            Console.WriteLine("2. Logout");
            Console.ResetColor();
            Console.WriteLine();

            Console.Write("\nEnter choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("Today's appointments...");
                    break;
                case "2":
                    Console.WriteLine("Logging out...");
                    break;
                default:
                    Console.WriteLine("Invalid choice!");
                    break;
            }
        }
    }
}