using System;
using System.ServiceModel;
using Common.Interfaces;
using Common.Models;

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

        public void RegisterResource(DERResource resource)
        {
            _client.RegisterResource(resource);
            Console.WriteLine($"Resource {resource.Name} successfully registered on the server.");
        }

        public void SetSchedule(ResourceSchedule schedule)
        {
            // Kod za postavljanje rasporeda
            // U slučaju da je `GetSchedule` metoda modifikovana da prima raspored, dodaj poziv serveru za to.
            Console.WriteLine($"Schedule set for Resource ID {schedule.ResourceId}: Start - {schedule.StartTime}, End - {schedule.EndTime}");
        }

        public void DisplayResourceStatus()
        {
            // Prikaz statusa resursa, kao što su ukupna snaga i proizvedena energija
            // Ovu metodu ćemo proširiti kada dodamo API za status resursa na serveru
            Console.WriteLine("Displaying current resource status...");
        }
    }
}
