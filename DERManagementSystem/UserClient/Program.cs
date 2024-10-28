using System;
using UserClient.Services;
using Common.Models;

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
                Console.WriteLine("2 - Set schedule for a resource");
                Console.WriteLine("3 - Display resource status");
                Console.WriteLine("0 - Exit");
                Console.Write("Option: ");
                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        RegisterNewResource(userService);
                        break;
                    case "2":
                        SetResourceSchedule(userService);
                        break;
                    case "3":
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
            Console.Write("Enter Resource Name: ");
            var name = Console.ReadLine();
            Console.Write("Enter Resource Power (kW): ");
            var power = double.Parse(Console.ReadLine());

            var resource = new DERResource
            {
                Id = new Random().Next(1, 1000),
                Name = name,
                Power = power,
                IsActive = false
            };

            userService.RegisterResource(resource);
        }

        private static void SetResourceSchedule(UserClientService userService)
        {
            Console.Write("Enter Resource ID: ");
            var resourceId = int.Parse(Console.ReadLine());
            Console.Write("Enter Start Time (yyyy-MM-dd HH:mm): ");
            var startTime = DateTime.Parse(Console.ReadLine());
            Console.Write("Enter End Time (yyyy-MM-dd HH:mm): ");
            var endTime = DateTime.Parse(Console.ReadLine());

            var schedule = new ResourceSchedule
            {
                ResourceId = resourceId,
                StartTime = startTime,
                EndTime = endTime
            };

            userService.SetSchedule(schedule);
        }
    }
}
