using System;
using System.ServiceModel;
using Common.Interfaces;
using Common.Models;

namespace DERClient.Services
{
    public class DERClientService
    {
        private readonly IDERService _client; // WCF klijent za komunikaciju sa DER serverom.

        public DERClientService()
        {
            // Konfiguracija WCF klijenta sa NetTcpBinding i adresom endpoint-a.
            var factory = new ChannelFactory<IDERService>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:8080/DERService"));
            _client = factory.CreateChannel(); // Kreira kanal za komunikaciju sa serverom.
        }

        public void RegisterResource(DERResource resource)
        {
            _client.RegisterResource(resource); // Poziva server metodu za registraciju resursa.
            Console.WriteLine($"Resource {resource.Name} registered on the server."); // Prikazuje potvrdu o registraciji resursa.
        }

        public ResourceSchedule GetSchedule(int resourceId)
        {
            return _client.GetSchedule(resourceId); // Dohvata raspored za dati ID resursa od servera.
        }

        public void LogProduction(int resourceId, double producedEnergy)
        {
            _client.LogProduction(resourceId, producedEnergy); // Beleži proizvedenu energiju za dati ID resursa na serveru.
            Console.WriteLine($"Logged {producedEnergy} kWh for Resource ID {resourceId}."); // Prikazuje potvrdu o beleženju energije.
        }
    }
}
