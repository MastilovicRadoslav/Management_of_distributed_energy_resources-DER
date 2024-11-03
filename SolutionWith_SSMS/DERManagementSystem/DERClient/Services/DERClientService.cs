using Common.Interfaces;
using Common.Models;
using System;
using System.ServiceModel;

namespace DERClient.Services
{
    public class DERClientService
    {
        private readonly IDERService _client;

        public DERClientService(IDERService client)
        {
            _client = client;
        }

        public DERClientService()
        {
            var factory = new ChannelFactory<IDERService>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:8080/DERService"));
            _client = factory.CreateChannel();
        }

        public bool ResourceExists(int resourceId)
        {
            var resourceInfo = _client.GetResourceStatus().Find(r => r.Id == resourceId);
            return resourceInfo != null;
        }

        public bool DisplayResourceSchedule(int resourceId)
        {
            var resourceInfo = _client.GetResourceStatus().Find(r => r.Id == resourceId);
            var statistics = _client.GetStatistics();

            if (resourceInfo != null)
            {
                Console.WriteLine("\n--- Resource Information ---");
                Console.WriteLine($"ID: {resourceInfo.Id}");
                Console.WriteLine($"Name: {resourceInfo.Name}");
                Console.WriteLine($"Power: {resourceInfo.Power} kW");
                Console.WriteLine($"Status: {(resourceInfo.IsActive ? "Active" : "Inactive")}");
                Console.WriteLine("\n--- Schedule Information ---");
                Console.WriteLine($"Start Time: {resourceInfo.StartTime}");
                Console.WriteLine($"End Time: {(resourceInfo.EndTime == DateTime.MinValue ? "Not yet ended" : resourceInfo.EndTime.ToString())}");
                Console.WriteLine($"Active Time: {resourceInfo.ActiveTime} seconds");
                Console.WriteLine($"Total Produced Energy: {statistics.TotalProducedEnergy} kWh");
                Console.WriteLine($"Total Active Power: {statistics.TotalActivePower} kW");

                return true;
            }
            else
            {
                Console.WriteLine("No information found for the specified resource.");
                return false;
            }
        }

        public string RegisterResource(int resourceId)
        {
            return _client.RegisterResource(resourceId);
        }

        public void UnregisterResource(int resourceId)
        {
            _client.UnregisterResource(resourceId);

            // Dohvati ažurirane podatke nakon deaktivacije
            DisplayResourceSchedule(resourceId);

        }

    }
}
