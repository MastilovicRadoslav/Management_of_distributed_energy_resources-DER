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
                Console.WriteLine("1 - Register new resource");
                Console.WriteLine("2 - Display resource status");
                Console.WriteLine("0 - Exit");
                Console.Write("Option: ");
                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        RegisterNewResource(userService);
                        break;
                    case "2":
                        userService.DisplayResourceStatus();
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
                    break; // Ako je unos validan broj, izlazi iz petlje
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
                    break; // Ako je unos validan broj, izlazi iz petlje
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid numeric power value.");
                }
            }

            var resource = new DERResource
            {
                Id = id, // ID koji unosi korisnik
                Name = name,
                Power = power,
                IsActive = false
            };

            userService.RegisterNewResource(resource);
        }

    }
}
