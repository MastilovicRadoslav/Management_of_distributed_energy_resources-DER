using Common.Models;
using System;
using UserClient.Services;

namespace UserClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var userService = new UserClientService();

            while (true)
            {
                Console.WriteLine("\nChoose an option:");
                Console.WriteLine("1 - Register new resources through the console");
                Console.WriteLine("2 - Register new resources through report from file");
                Console.WriteLine("3 - Show information about all active resources");
                Console.WriteLine("4 - Show information about all inactive resources");
                Console.WriteLine("5 - Show TotalActivePower and TotalProducedEnergy");
                Console.WriteLine("6 - Show information for a specific resource by name");
                Console.WriteLine("7 - Show all information about all resources");
                Console.WriteLine("0 - Exit");
                Console.Write("Option: ");
                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        RegisterNewResource(userService);
                        break;
                    case "2":
                        Console.Write("Enter type 'DA': ");
                        var filePath = Console.ReadLine();
                        // Postavi putanju do `resources.txt` fajla u `Resources` folderu
                        filePath = "C:\\Users\\Lenovo\\Desktop\\Praksa\\DERManagementSystem\\UserClient\\Resources\\resources.txt";
                        userService.LoadAndRegisterResourcesFromFile(filePath);
                        break;
                    case "3":
                        userService.DisplayActiveResources(); // Prikaz aktivnih resursa
                        break;
                    case "4":
                        userService.DisplayInactiveResources(); // Prikaz neaktivnih resursa
                        break;
                    case "5":
                        userService.DisplayTotalPowerAndEnergy(); // Prikaz TotalActivePower i TotalProducedEnergy
                        break;
                    case "6":
                        DisplayResourceByName(userService); // Prikaz informacija o resursu po imenu
                        break;
                    case "7":
                        userService.DisplayResourceStatus(); // Prikaz svih resursa
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private static void RegisterNewResource(UserClientService userService)
        {
            int id;
            while (true)
            {
                Console.Write("Enter Resource ID: ");
                var idInput = Console.ReadLine();

                if (int.TryParse(idInput, out id))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid numeric ID.");
                }
            }

            Console.Write("Enter Resource Name: ");
            var name = Console.ReadLine();

            double power;
            while (true)
            {
                Console.Write("Enter Resource Power (kW): ");
                var powerInput = Console.ReadLine();

                if (double.TryParse(powerInput, out power))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid numeric power value.");
                }
            }

            var resource = new DERResource
            {
                Id = id,
                Name = name,
                Power = power,
                IsActive = false
            };

            userService.RegisterNewResource(resource);
            Console.WriteLine("Resource has been added successfully.");
        }

        private static void DisplayResourceByName(UserClientService userService)
        {
            Console.Write("Enter the name of the resource: ");
            var name = Console.ReadLine();
            userService.DisplayResourceByName(name);
        }
    }
}
