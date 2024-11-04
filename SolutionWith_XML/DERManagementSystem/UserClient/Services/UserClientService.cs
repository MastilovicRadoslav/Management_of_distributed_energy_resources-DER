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
            // Proverava da li resurs već postoji u bazi
            var existingResource = _client.GetResourceStatus().FirstOrDefault(r => r.Id == resource.Id);

            if (existingResource == null) // Ako resurs ne postoji
            {
                _client.RegisterNewResource(resource);
                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine("                Resource Registration Success               ");
                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine($"Resource Name         : {resource.Name}");
                Console.WriteLine($"Assigned ID           : {resource.Id}");
                Console.WriteLine($"Power                 : {resource.Power} kW");
                Console.WriteLine("Resource successfully added from console.");
                Console.WriteLine("------------------------------------------------------------\n");
            }
            else
            {
                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine("              Resource Registration Skipped                 ");
                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine($"Resource with ID {resource.Id} already exists. Skipping this entry.");
                Console.WriteLine("------------------------------------------------------------\n");
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
                    _client.RegisterNewResource(resource);
                    Console.WriteLine("------------------------------------------------------------");
                    Console.WriteLine("                Resource Registration Success               ");
                    Console.WriteLine("------------------------------------------------------------");
                    Console.WriteLine($"Resource Name         : {resource.Name}");
                    Console.WriteLine($"Assigned ID           : {resource.Id}");
                    Console.WriteLine($"Power                 : {resource.Power} kW");
                    Console.WriteLine("Resource successfully added from file.");
                    Console.WriteLine("------------------------------------------------------------\n");
                }
                else
                {
                    Console.WriteLine("------------------------------------------------------------");
                    Console.WriteLine("              Resource Registration Skipped                 ");
                    Console.WriteLine("------------------------------------------------------------");
                    Console.WriteLine($"Resource with ID {resource.Id} already exists. Skipping this entry.");
                    Console.WriteLine("------------------------------------------------------------\n");
                }
            }
        }


        public void DisplayResourceStatus()
        {
            var resources = _client.GetResourceStatus();
            var totalPower = resources.Sum(r => r.IsActive ? r.Power : 0);
            var totalEnergy = resources.Sum(r => r.TotalProducedEnergy);

            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("                       Resource Status                      ");
            Console.WriteLine("------------------------------------------------------------");

            foreach (var resource in resources)
            {
                Console.WriteLine($"Resource ID           : {resource.Id}");
                Console.WriteLine($"Name                  : {resource.Name}");
                Console.WriteLine($"Power                 : {resource.Power} kW");
                Console.WriteLine($"Status                : {(resource.IsActive ? "Active" : "Inactive")}");
                Console.WriteLine($"Start Time            : {(resource.StartTime != null ? resource.StartTime.ToString() : "N/A")}");
                Console.WriteLine($"End Time              : {(resource.EndTime != null ? resource.EndTime.ToString() : "N/A")}");
                Console.WriteLine($"Active Duration       : {resource.ActiveTime} seconds");
                Console.WriteLine("------------------------------------------------------------");
            }

            Console.WriteLine("                       Summary Statistics                   ");
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine($"Total Active Power    : {totalPower} kW");
            Console.WriteLine($"Total Produced Energy : {totalEnergy} kWh");
            Console.WriteLine("------------------------------------------------------------\n");
        }


        public void ClearAllResources()
        {
            _client.ClearAllResources(); // Poziv na server za brisanje svih resursa
            Console.WriteLine("\nResource deletion successful!");

        }

    }
}
