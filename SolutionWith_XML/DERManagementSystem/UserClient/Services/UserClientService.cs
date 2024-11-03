using Common.Interfaces;
using System;
using System.Linq;
using System.ServiceModel;

namespace UserClient.Services
{
    public class UserClientService
    {
        private readonly IDERService _client;
        private readonly FileResourceLoader _fileLoader;


        public UserClientService(IDERService client)
        {
            _client = client;
        }
        public UserClientService()
        {
            var factory = new ChannelFactory<IDERService>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:8080/DERService"));
            _client = factory.CreateChannel();


            _fileLoader = new FileResourceLoader(); // Inicijalizacija fileLoader

        }

        public void RegisterNewResource(DERResource resource)
        {
            _client.RegisterNewResource(resource);


            // Proverava da li resurs već postoji u bazi
            var existingResource = _client.GetResourceStatus().FirstOrDefault(r => r.Id == resource.Id);

            if (existingResource == null) // Ako resurs ne postoji
            {
                RegisterNewResource(resource);
                Console.WriteLine($"Resource with ID {resource.Id} already exists. Skipping this entry.");

            }
            else
            {
                Console.WriteLine($"Resource {resource.Name} with ID {resource.Id} has been added from console.");

            }


        }

        public void LoadAndRegisterResourcesFromFile(string filePath)
        {
            var resources = _fileLoader.LoadResourcesFromFile(filePath);

            foreach (var resource in resources)
            {
                // Proverava da li resurs već postoji u bazi
                var existingResource = _client.GetResourceStatus().FirstOrDefault(r => r.Id == resource.Id);

                if (existingResource == null) // Ako resurs ne postoji
                {
                    RegisterNewResource(resource);
                    Console.WriteLine($"Resource {resource.Name} with ID {resource.Id} has been added from file.");
                }
                else
                {
                    Console.WriteLine($"Resource with ID {resource.Id} already exists. Skipping this entry.");
                }
            }
        }

        public void DisplayResourceStatus()
        {
            var resources = _client.GetResourceStatus();
            var totalPower = resources.Sum(r => r.IsActive ? r.Power : 0);
            var totalEnergy = resources.Sum(r => r.TotalProducedEnergy);
            foreach (var resource in resources)
            {
                Console.WriteLine($"ID: {resource.Id}, Name: {resource.Name}, Power: {resource.Power} kW, Status: {(resource.IsActive ? "Active" : "Inactive")}");
                Console.WriteLine($"Start Time: {resource.StartTime}, End Time: {resource.EndTime}, Active Time: {resource.ActiveTime} seconds");
            }
            Console.WriteLine($"Total Active Power: {totalPower} kW");
            Console.WriteLine($"Total Produced Energy: {totalEnergy} kWh");
        }

        public void ClearAllResources()
        {
            _client.ClearAllResources(); // Poziv na server za brisanje svih resursa
            Console.WriteLine("Resource deletion successful!");

        }

    }
}
