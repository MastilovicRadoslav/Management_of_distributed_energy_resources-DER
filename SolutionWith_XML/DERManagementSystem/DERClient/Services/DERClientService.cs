using Common.Interfaces;
using System;
using System.ServiceModel;

namespace DERClient.Services
{
    public class DERClientService
    {
        private readonly IDERService _client;

        public DERClientService(IDERService client) // Konstruktor sa argumentom
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
            // Provera da li resurs postoji u kolekciji resursa na serveru
            var resourceInfo = _client.GetResourceStatus().Find(r => r.Id == resourceId);
            return resourceInfo != null;
        }

        public bool DisplayResourceSchedule(int resourceId)
        {
            // Pribavi informacije o resursu
            var resourceInfo = _client.GetResourceStatus().Find(r => r.Id == resourceId);
            var schedule = _client.GetSchedule(resourceId);

            if (resourceInfo != null)
            {
                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine("                     Resource Information                   ");
                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine($"Resource ID           : {resourceInfo.Id}");
                Console.WriteLine($"Name                  : {resourceInfo.Name}");
                Console.WriteLine($"Power                 : {resourceInfo.Power} kW");
                Console.WriteLine($"Status                : {(resourceInfo.IsActive ? "Active" : "Inactive")}");
                Console.WriteLine("------------------------------------------------------------");

                // Prikaži raspored samo ako postoji
                if (schedule != null)
                {
                    Console.WriteLine("                     Schedule Information                   ");
                    Console.WriteLine("------------------------------------------------------------");
                    Console.WriteLine($"Start Time            : {(schedule.StartTime != DateTime.MinValue ? schedule.StartTime.ToString("dd-MM-yyyy HH:mm:ss") : "N/A")}");
                    Console.WriteLine($"End Time              : {(schedule.EndTime != DateTime.MinValue ? schedule.EndTime.ToString("dd-MM-yyyy HH:mm:ss") : "N/A")}");
                    Console.WriteLine($"Active Duration       : {schedule.ActiveTime} seconds");
                }
                else
                {
                    Console.WriteLine("No schedule information available for this resource.");
                }

                Console.WriteLine("------------------------------------------------------------\n");
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
            return _client.RegisterResource(resourceId); // Aktivira resurs
        }

        public void UnregisterResource(int resourceId)
        {
            _client.UnregisterResource(resourceId); // Deaktivira resurs
            DisplayResourceSchedule(resourceId); // Prikaz informacija nakon deaktivacije
        }
    }
}
