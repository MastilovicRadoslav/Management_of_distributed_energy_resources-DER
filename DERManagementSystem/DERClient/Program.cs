using System;
using DERClient.Services;
using Common.Models;

namespace DERClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var clientService = new DERClientService();

            // Podesi ID resursa za koji želiš da preuzmeš raspored (mora da odgovara ID-ju koji je registrovan kroz UserClient)
            int resourceId = 1; // Primer ID-a resursa koji je registrovan

            // Preuzimanje rasporeda za resurs
            var schedule = clientService.GetSchedule(resourceId);
            if (schedule != null)
            {
                Console.WriteLine($"Schedule received: Start - {schedule.StartTime}, End - {schedule.EndTime}");

                // Simulacija rada resursa i logovanje proizvodnje
                double producedEnergy = 15.0; // Primer proizvedene energije u kWh
                clientService.LogProduction(resourceId, producedEnergy);
            }
            else
            {
                Console.WriteLine("No schedule found for this resource.");
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
