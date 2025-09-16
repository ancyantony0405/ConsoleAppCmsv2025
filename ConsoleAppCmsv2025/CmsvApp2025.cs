using ConsoleAppCmsv2025.Repository;
using ConsoleAppCmsv2025.Service;
using ConsoleAppCmsv2025.Utility;

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
                Console.WriteLine("-----------------");
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("   L O G I N   ");
                Console.ResetColor();
                Console.WriteLine("-----------------");

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
                        case 1: // Receptionist
                            ShowReceptionistMenu();
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

        private static void ShowReceptionistMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== Receptionist Dashboard ===");
            Console.ResetColor();

            Console.WriteLine("1. Register Patient");
            Console.WriteLine("2. Book Appointment");
            Console.WriteLine("0. Logout");

            Console.Write("\nEnter choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("Registering new patient...");
                    break;
                case "2":
                    Console.WriteLine("Booking appointment...");
                    break;
                case "0":
                    Console.WriteLine("Logging out...");
                    break;
                default:
                    Console.WriteLine("Invalid choice!");
                    break;
            }
        }

        private static void ShowDoctorMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("=== Doctor Dashboard ===");
            Console.ResetColor();

            Console.WriteLine("1. View Appointments");
            Console.WriteLine("2. Add Consultation");
            Console.WriteLine("3. Write Prescription");
            Console.WriteLine("0. Logout");

            Console.Write("\nEnter choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("Showing today's appointments...");
                    break;
                case "2":
                    Console.WriteLine("Adding consultation notes...");
                    break;
                case "3":
                    Console.WriteLine("Writing prescription...");
                    break;
                case "0":
                    Console.WriteLine("Logging out...");
                    break;
                default:
                    Console.WriteLine("Invalid choice!");
                    break;
            }
        }
    }
}