using DERClient.Services;
using System;

namespace DERClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var clientService = new DERClientService();

            // Aktivira i deaktivira nasumični resurs, zatim završava program
            clientService.ActivateAndDeactivateRandomResource();

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
