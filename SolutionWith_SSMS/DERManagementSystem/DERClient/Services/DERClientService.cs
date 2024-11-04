using Common.Interfaces;
using System;
using System.ServiceModel;

namespace DERClient.Services
{
    public class DERClientService
    {
        private readonly IDERService _client;

        /// <summary>
        /// Konstruktor koji omogućava unos mock IDERService instance, koristi se za testiranje.
        /// </summary>
        public DERClientService(IDERService client)
        {
            _client = client;
        }

        /// <summary>
        /// Podrazumevani konstruktor koji postavlja WCF komunikaciju sa DERServer-om koristeći NetTcpBinding.
        /// </summary>
        public DERClientService()
        {
            var factory = new ChannelFactory<IDERService>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:8080/DERService"));
            _client = factory.CreateChannel();
        }

        /// <summary>
        /// Proverava da li resurs postoji na serveru pomoću resurs ID-a.
        /// </summary>
        /// <param name="resourceId">ID resursa</param>
        /// <returns>Vraća true ako resurs postoji, inače false.</returns>
        public bool ResourceExists(int resourceId)
        {
            var resourceInfo = _client.GetResourceStatus().Find(r => r.Id == resourceId);
            return resourceInfo != null;
        }

        /// <summary>
        /// Prikazuje informacije o resursu, kao i sveukupne statistike ako resurs postoji.
        /// </summary>
        /// <param name="resourceId">ID resursa</param>
        /// <returns>Vraća true ako resurs postoji i informacije su prikazane, inače false.</returns>
        public bool DisplayResourceSchedule(int resourceId)
        {
            var resourceInfo = _client.GetResourceStatus().Find(r => r.Id == resourceId);
            var statistics = _client.GetStatistics();

            if (resourceInfo != null)
            {
                Console.WriteLine("\n============================================================");
                Console.WriteLine("                  Resource Schedule Information             ");
                Console.WriteLine("============================================================");
                Console.WriteLine($"ID:                 {resourceInfo.Id}");
                Console.WriteLine($"Name:               {resourceInfo.Name}");
                Console.WriteLine($"Power:              {resourceInfo.Power} kW");
                Console.WriteLine($"Status:             {(resourceInfo.IsActive ? "Active" : "Inactive")}");
                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine("                   Schedule Details                         ");
                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine($"Start Time:         {resourceInfo.StartTime}");
                Console.WriteLine($"End Time:           {(resourceInfo.EndTime == DateTime.MinValue ? "Not yet ended" : resourceInfo.EndTime.ToString())}");
                Console.WriteLine($"Active Time:        {resourceInfo.ActiveTime} seconds");
                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine("                   Total Statistics                         ");
                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine($"Total Produced Energy: {statistics.TotalProducedEnergy} kWh");
                Console.WriteLine($"Total Active Power:    {statistics.TotalActivePower} kW");
                Console.WriteLine("============================================================\n");

                return true;
            }
            else
            {
                Console.WriteLine("\n============================================================");
                Console.WriteLine("                  Resource Schedule Information             ");
                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine("[ERROR] No information found for the specified resource.");
                Console.WriteLine("============================================================\n");
                return false;
            }
        }


        /// <summary>
        /// Registruje resurs pomoću ID-a na serveru.
        /// </summary>
        /// <param name="resourceId">ID resursa</param>
        /// <returns>Vraća rezultat registracije kao string poruku.</returns>
        public string RegisterResource(int resourceId)
        {
            return _client.RegisterResource(resourceId);
        }

        /// <summary>
        /// Deaktivira resurs na serveru pomoću ID-a, i prikazuje ažurirane informacije o resursu.
        /// </summary>
        /// <param name="resourceId">ID resursa</param>
        public void UnregisterResource(int resourceId)
        {
            _client.UnregisterResource(resourceId);

            // Dohvati ažurirane podatke nakon deaktivacije
            DisplayResourceSchedule(resourceId);
        }
    }
}
