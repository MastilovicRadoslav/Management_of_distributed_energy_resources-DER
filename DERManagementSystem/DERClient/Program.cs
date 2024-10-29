using DERClient.Services;
using System;

namespace DERClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\nClient is running."); // Prikazuje poruku da je server pokrenut.

            var clientService = new DERClientService();
            bool validResource = false;

            int resourceId = 0; // Inicijalizacija pre petlje, da izbegnemo CS0165 grešku

            while (!validResource)
            {
                Console.Write("Enter Resource ID: ");

                if (int.TryParse(Console.ReadLine(), out resourceId))
                {
                    // Proveri da li resurs postoji i prikaži informacije
                    if (clientService.ResourceExists(resourceId))
                    {
                        if (clientService.DisplayResourceSchedule(resourceId))
                        {
                            validResource = true; // Resurs je validan, prekida unos ID-a
                        }
                        else
                        {
                            Console.WriteLine("Resource has no schedule on the server. Please try again.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Resource with the specified ID does not exist. Please try again.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid numeric ID.");
                }
            }

            while (true)
            {
                Console.WriteLine("\nChoose an option:");
                Console.WriteLine("1 - Activate resource");
                Console.WriteLine("2 - Deactivate resource");
                Console.WriteLine("0 - Exit");
                Console.Write("Option: ");
                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        Console.WriteLine(clientService.RegisterResource(resourceId)); // Aktivacija
                        break;
                    case "2":
                        clientService.UnregisterResource(resourceId); // Deaktivacija i prikaz informacija posle deaktivacije
                        break;
                    case "0":
                        Console.WriteLine("Press any key to exit.");
                        Console.ReadKey();
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
    }
}
