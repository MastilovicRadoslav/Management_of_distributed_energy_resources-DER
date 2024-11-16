using Common.Interfaces;
using System;
using System.Linq;
using System.ServiceModel;
using System.Threading;

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

        public void ActivateAndDeactivateRandomResource()
        {
            var resources = _client.GetResourceStatus();
            if (resources == null || !resources.Any())
            {
                Console.WriteLine("There are no resources in the station to activate it.");
                return;
            }

            // Nasumično biramo jedan ID resursa iz dostupnih resursa
            Random random = new Random();
            var randomResource = resources[random.Next(resources.Count)];
            int resourceId = randomResource.Id;

            // Prikaz informacija o resursu pre aktivacije
            DisplayResourceInfo(resourceId);

            Console.WriteLine("\nThe resource is being activated instantly...\n");
            Thread.Sleep(2000); // Pauza pre aktivacije (2 sekunde)

            // Aktiviramo resurs
            Console.WriteLine(_client.RegisterResource(resourceId));

            // Zadržavanje resursa aktivnim između 9 i 15 sekundi
            int delay = random.Next(9, 15);
            Thread.Sleep(delay * 1000);

            // Deaktiviramo resurs
            _client.UnregisterResource(resourceId);

            // Prikaz informacija o resursu nakon deaktivacije
            DisplayResourceInfo(resourceId);
        }

        private void DisplayResourceInfo(int resourceId)
        {
            var resource = _client.GetResourceStatus().FirstOrDefault(r => r.Id == resourceId);
            if (resource != null)
            {
                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine("                     Resource Information                   ");
                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine($"Resource ID which is activated.      : {resource.Id}");
                Console.WriteLine($"Name                                 : {resource.Name}");
                Console.WriteLine($"Power                                : {resource.Power} kW");
                Console.WriteLine($"Status                               : {(resource.IsActive ? "Active" : "Inactive")}");
                Console.WriteLine($"Start Time                           : {(resource.StartTime.HasValue ? resource.StartTime.Value.ToString("dd-MM-yyyy HH:mm:ss") : "N/A")}");
                Console.WriteLine($"End Time                             : {(resource.EndTime.HasValue ? resource.EndTime.Value.ToString("dd-MM-yyyy HH:mm:ss") : "N/A")}");
                Console.WriteLine($"Active Duration                      : {resource.ActiveTime} seconds");
                Console.WriteLine("------------------------------------------------------------\n");
            }
            else
            {
                Console.WriteLine("Resource information could not be retrieved.");
            }
        }
    }
}
