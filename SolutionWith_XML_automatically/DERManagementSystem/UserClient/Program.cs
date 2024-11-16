﻿using System;
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
                Console.WriteLine("1 - Register new resources through the console.");
                Console.WriteLine("2 - Register new resources through report from file.");
                Console.WriteLine("3 - Show all information about all resources.");
                Console.WriteLine("4 - Delete all resources in resources station.");
                Console.WriteLine("0 - Exit");
                Console.Write("Option: ");
                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        RegisterNewResource(userService);
                        break;
					case "2":
						string input;
						do
						{
							Console.Write("Enter type 'Y' to proceed or 'N' to cancel: ");
							input = Console.ReadLine().ToUpper();

							if (input != "Y" && input != "N")
							{
								Console.WriteLine("Invalid input. Please enter 'Y' or 'N'.");
							}
						} while (input != "Y" && input != "N");

						if (input == "Y")
						{
							var filePath = "C:\Users\Lenovo\Documents\GitHub\Management_of_distributed_energy_resources-DER\SolutionWith_XML_automatically\Resources\resources.txt";
							userService.LoadAndRegisterResourcesFromFile(filePath);
						}
						else
						{
							Console.WriteLine("Operation canceled.");
						}
						break;
                    case "3":
                        userService.DisplayResourceStatus(); // Prikaz svih resursa
                        break;
                    case "4":
                        userService.ClearAllResources(); // Prikaz svih resursa
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
        }
    }
}