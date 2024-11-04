using Common.Interfaces;
using System;
using System.ServiceModel;

namespace UserClient.Services
{
    public class UserClientService
    {
        private readonly IDERService _client;
        private readonly FileResourceLoader _fileLoader;

        // Konstruktor koji prima IDERService interfejs kao parametar, koristi se za testiranje ili kada je IDERService već dostupan
        public UserClientService(IDERService client)
        {
            _client = client;
            _fileLoader = new FileResourceLoader();
        }

        // Konstruktor koji kreira kanal za komunikaciju sa serverom i inicijalizuje fileLoader
        public UserClientService()
        {
            var factory = new ChannelFactory<IDERService>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:8080/DERService"));
            _client = factory.CreateChannel();
            _fileLoader = new FileResourceLoader();
        }

        /// <summary>
        /// Registruje novi resurs na serveru. Ako resurs već postoji, prijavljuje duplikat.
        /// </summary>
        /// <param name="resource">Resurs koji se registruje</param>
        /// <returns>True ako je resurs uspešno dodat, False ako već postoji</returns>
        public bool RegisterNewResource(DERResource resource)
        {
            var addedResource = _client.RegisterNewResource(resource);

            if (addedResource != null)
            {
                Console.WriteLine("\n------------------------------------------------------------");
                Console.WriteLine("                      Resource Registration                  ");
                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine($"[SUCCESS] Resource '{addedResource.Name}' has been added.");
                Console.WriteLine($"          ID: {addedResource.Id}");
                Console.WriteLine($"          Power: {addedResource.Power} kW");
                Console.WriteLine("------------------------------------------------------------\n");
                return true;
            }
            else
            {
                Console.WriteLine("\n------------------------------------------------------------");
                Console.WriteLine("                      Resource Registration                  ");
                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine("[WARNING] Resource with similar characteristics already exists.");
                Console.WriteLine("          Skipping this entry.");
                Console.WriteLine("------------------------------------------------------------\n");
                return false;
            }
        }


        /// <summary>
        /// Učitava resurse iz fajla i pokušava da ih registruje na serveru.
        /// Prikazuje poruku o statusu za svaki resurs iz fajla.
        /// </summary>
        /// <param name="filePath">Putanja do fajla iz kojeg se resursi učitavaju</param>
        public void LoadAndRegisterResourcesFromFile(string filePath)
        {
            var resources = _fileLoader.LoadResourcesFromFile(filePath);

            Console.WriteLine("\n------------------------------------------------------------");
            Console.WriteLine("                Resource Registration from File              ");
            Console.WriteLine("------------------------------------------------------------");

            foreach (var resource in resources)
            {
                var addedResource = _client.RegisterNewResource(resource);

                if (addedResource != null)
                {
                    Console.WriteLine($"[ADDED]   Resource: {addedResource.Name,-20} | ID: {addedResource.Id,-5} | Power: {addedResource.Power} kW");
                }
                else
                {
                    Console.WriteLine($"[SKIPPED] Resource: {resource.Name,-20} | ID: (existing) | Reason: Duplicate");
                }
            }

            Console.WriteLine("------------------------------------------------------------\n");
        }


        /// <summary>
        /// Prikazuje status svih resursa sa servera, uključujući informacije o svakom resursu, ukupnoj aktivnoj snazi i proizvedenoj energiji.
        /// </summary>
        public void DisplayResourceStatus()
        {
            var resources = _client.GetResourceStatus();
            var statistics = _client.GetStatistics();

            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("                      Resource Summary                      ");
            Console.WriteLine("------------------------------------------------------------");

            foreach (var resource in resources)
            {
                Console.WriteLine($"Resource ID      : {resource.Id}");
                Console.WriteLine($"Name             : {resource.Name}");
                Console.WriteLine($"Power            : {resource.Power} kW");
                Console.WriteLine($"Status           : {(resource.IsActive ? "Active" : "Inactive")}");
                Console.WriteLine($"Start Time       : {(resource.StartTime.HasValue ? resource.StartTime.Value.ToString("dd-MM-yyyy HH:mm:ss") : "N/A")}");
                Console.WriteLine($"End Time         : {(resource.EndTime.HasValue ? resource.EndTime.Value.ToString("dd-MM-yyyy HH:mm:ss") : "N/A")}");
                Console.WriteLine($"Active Duration  : {resource.ActiveTime} seconds");
                Console.WriteLine("------------------------------------------------------------");
            }

            Console.WriteLine("Overall Statistics:");
            Console.WriteLine($"Total Active Power      : {statistics.TotalActivePower} kW");
            Console.WriteLine($"Total Produced Energy   : {statistics.TotalProducedEnergy} kWh");
            Console.WriteLine("------------------------------------------------------------");

        }

        /// <summary>
        /// Briše sve resurse sa servera i prikazuje poruku o uspešnom brisanju.
        /// </summary>
        public void ClearAllResources()
        {
            _client.ClearAllResources();
            Console.WriteLine("\nResource deletion successful!");
        }
    }
}
