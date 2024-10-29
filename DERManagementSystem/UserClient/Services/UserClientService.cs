using Common.Interfaces;
using Common.Models;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace UserClient.Services
{
    public class UserClientService
    {
        private readonly IDERService _client;

        public UserClientService()
        {
            // Konfiguracija WCF klijenta
            var factory = new ChannelFactory<IDERService>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:8080/DERService"));
            _client = factory.CreateChannel();
        }

        public void RegisterNewResource(DERResource resource)
        {
            _client.RegisterNewResource(resource);
            Console.WriteLine($"Resource added with ID {resource.Id}, Name: {resource.Name}, Power: {resource.Power} kW");
        }

        public void DisplayResourceStatus()
        {
            List<ResourceInfo> resources = _client.GetResourceStatus();

            Console.WriteLine("\n--- Resource Status ---\n");
            foreach (var resource in resources)
            {
                Console.WriteLine($"ID: {resource.Id}");
                Console.WriteLine($"Name: {resource.Name}");
                Console.WriteLine($"Power: {resource.Power} kW");
                Console.WriteLine($"Status: {(resource.IsActive ? "Active" : "Inactive")}");
                Console.WriteLine($"Start Time: {resource.StartTime}");
                Console.WriteLine($"End Time: {resource.EndTime}");
                Console.WriteLine($"Active Time: {resource.ActiveTime} seconds");
                Console.WriteLine();
            }

            // Pretpostavljamo da su TotalActivePower i TotalProducedEnergy već prikupljeni sa servera
            if (resources.Count > 0)
            {
                Console.WriteLine("--- Summary ---");
                Console.WriteLine($"Total Active Power: {resources[0].TotalActivePower} kW");
                Console.WriteLine($"Total Produced Energy: {resources[0].TotalProducedEnergy} kWh");
            }
        }
    }
}
