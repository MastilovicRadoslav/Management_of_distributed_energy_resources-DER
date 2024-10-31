using Common.Interfaces;
using Common.Models;
using System;
using System.Linq;
using System.ServiceModel;

namespace UserClient.Services
{
    public class UserClientService
    {
        private readonly IDERService _client;
        private readonly FileResourceLoader _fileLoader;


        public UserClientService()
        {
            var factory = new ChannelFactory<IDERService>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:8080/DERService"));
            _client = factory.CreateChannel();
            _fileLoader = new FileResourceLoader(); // Inicijalizacija fileLoader

        }


        public void RegisterNewResource(DERResource resource)
        {
            _client.RegisterNewResource(resource);
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
            foreach (var resource in resources)
            {
                Console.WriteLine($"ID: {resource.Id}, Name: {resource.Name}, Power: {resource.Power} kW, Status: {(resource.IsActive ? "Active" : "Inactive")}");
                Console.WriteLine($"Start Time: {resource.StartTime}, End Time: {resource.EndTime}, Active Time: {resource.ActiveTime} seconds");
            }
        }

        public void DisplayActiveResources()
        {
            var resources = _client.GetResourceStatus().Where(r => r.IsActive);
            foreach (var resource in resources)
            {
                Console.WriteLine($"ID: {resource.Id}, Name: {resource.Name}, Power: {resource.Power} kW");
                Console.WriteLine($"Start Time: {resource.StartTime}, Active Time: {resource.ActiveTime} seconds");
            }
        }

        public void DisplayInactiveResources()
        {
            var resources = _client.GetResourceStatus().Where(r => !r.IsActive);
            foreach (var resource in resources)
            {
                Console.WriteLine($"ID: {resource.Id}, Name: {resource.Name}, Power: {resource.Power} kW");
                Console.WriteLine($"End Time: {resource.EndTime}, Total Active Time: {resource.ActiveTime} seconds");
            }
        }

        public void DisplayTotalPowerAndEnergy()
        {
            var resources = _client.GetResourceStatus();
            var totalPower = resources.Sum(r => r.IsActive ? r.Power : 0);
            var totalEnergy = resources.Sum(r => r.TotalProducedEnergy);

            Console.WriteLine($"Total Active Power: {totalPower} kW");
            Console.WriteLine($"Total Produced Energy: {totalEnergy} kWh");
        }

        public void DisplayResourceByName(string name)
        {
            var resource = _client.GetResourceStatus().FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (resource != null)
            {
                Console.WriteLine($"ID: {resource.Id}, Name: {resource.Name}, Power: {resource.Power} kW, Status: {(resource.IsActive ? "Active" : "Inactive")}");
                Console.WriteLine($"Start Time: {resource.StartTime}, End Time: {resource.EndTime}, Active Time: {resource.ActiveTime} seconds");
                Console.WriteLine($"Total Produced Energy: {resource.TotalProducedEnergy} kWh");
            }
            else
            {
                Console.WriteLine("Resource with the specified name not found.");
            }
        }
    }
}
