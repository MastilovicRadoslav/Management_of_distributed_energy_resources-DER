using DERClient.Services;
using System;

namespace DERClient
{
    class Program
    {
        static void Main(string[] args)
        {

            var clientService = new DERClientService();

            // Aktiviraj i deaktiviraj nasumičan resurs
            clientService.ActivateAndDeactivateRandomResource();

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
