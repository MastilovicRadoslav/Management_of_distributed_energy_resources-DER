using Common.Interfaces;
using System;
using System.ServiceModel;

namespace UserClient.Services
{
    public class UserClientService
    {
        private readonly IDERService _client;
        private readonly FileResourceLoader _fileLoader;

        // Konstruktor koji prima IDERService interfejs kao parametar
        public UserClientService(IDERService client)
        {
            _client = client;
            _fileLoader = new FileResourceLoader();
        }
        public UserClientService()
        {
            var factory = new ChannelFactory<IDERService>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:8080/DERService"));
            _client = factory.CreateChannel();
            _fileLoader = new FileResourceLoader(); // Inicijalizacija fileLoader

        }
        public bool RegisterNewResource(DERResource resource)
        {
            var addedResource = _client.RegisterNewResource(resource); // Sada vraća `DERResource` objekat ili `null` ako već postoji

            if (addedResource != null)
            {
                Console.WriteLine($"Resource {addedResource.Name} with ID {addedResource.Id} has been added.");
                return true;
            }
            else
            {
                Console.WriteLine($"Resource with similar characteristics already exists. Skipping this entry.");
                return false;
            }
        }



        public void LoadAndRegisterResourcesFromFile(string filePath)
        {
            var resources = _fileLoader.LoadResourcesFromFile(filePath);

            foreach (var resource in resources)
            {
                // Pokušajte da registrujete novi resurs
                var success = _client.RegisterNewResource(resource); // Čuvanje rezultata

                if (resource != null)
                {
                    Console.WriteLine($"Resource {success.Name} with ID {success.Id} has been added from file.");
                }
                else
                {
                    Console.WriteLine($"Resource with ID {success.Id} already exists. Skipping this entry.");
                }
            }
        }


        public void DisplayResourceStatus()
        {
            var resources = _client.GetResourceStatus();
            var statistics = _client.GetStatistics(); // Preuzmi ukupne vrednosti iz baze

            foreach (var resource in resources)
            {
                Console.WriteLine($"ID: {resource.Id}, Name: {resource.Name}, Power: {resource.Power} kW, Status: {(resource.IsActive ? "Active" : "Inactive")}");
                Console.WriteLine($"Start Time: {resource.StartTime}, End Time: {resource.EndTime}, Active Time: {resource.ActiveTime} seconds");
            }
            Console.WriteLine($"Total Active Power: {statistics.TotalActivePower} kW");
            Console.WriteLine($"Total Produced Energy: {statistics.TotalProducedEnergy} kWh");
        }


        public void ClearAllResources()
        {
            _client.ClearAllResources(); // Poziv na server za brisanje svih resursa
            Console.WriteLine("Resource deletion successful!");

        }

    }
}
