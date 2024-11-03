using Common.Interfaces;
using Common.Models;
using DERServer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace DERServer.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class DERService : IDERService
    {
        //private readonly Dictionary<int, DERResource> resources = new Dictionary<int, DERResource>(); // Skladišti sve registrovane resurse koristeći ID kao ključ.

        private readonly Statistics statistics = new Statistics(); // Čuva statistiku, uključujući ukupnu proizvedenu energiju.

        private readonly XmlDataAccess _xmlDataAccess;

        public DERService()
        {
            // Postavite putanju do XML datoteke
            _xmlDataAccess = new XmlDataAccess("C:\\Users\\Lenovo\\Documents\\GitHub\\Management_of_distributed_energy_resources-DER\\SolutionWith_XML\\Results\\resource.xml");
        }

        public string RegisterResource(int resourceId)
        {
            // Učitajte postojeće resurse iz XML datoteke
            var existingResources = _xmlDataAccess.LoadResources();

            // Pronađite resurs na osnovu ID-a
            var resource = existingResources.FirstOrDefault(r => r.Id == resourceId);

            if (resource != null)
            {
                // Ažurirajte status resursa
                resource.IsActive = true; // Postavite status resursa na aktivan
                resource.StartTime = DateTime.Now; // Postavite vreme početka

                // Ažurirajte ukupnu aktivnu snagu
                statistics.TotalActivePower += resource.Power;

                // Sačuvajte izmenjene resurse u XML
                _xmlDataAccess.SaveResources(existingResources, statistics); // Sačuvajte izmene nazad u XML

                // Ispis na konzolu
                Console.WriteLine($"Resource with ID {resourceId} is now active on server.");
                Console.WriteLine($"Start Time: {resource.StartTime}");
                Console.WriteLine($"Active Energy: {resource.Power} kW");

                return $"Resource with ID {resourceId} is now active.\nPower: {resource.Power} kW";
            }
            else
            {
                return "Resource not found.";
            }
        }




        public string UnregisterResource(int resourceId)
        {
            // Učitajte postojeće resurse iz XML datoteke
            var existingResources = _xmlDataAccess.LoadResources();

            // Pronađite resurs na osnovu ID-a
            var resource = existingResources.FirstOrDefault(r => r.Id == resourceId);

            if (resource != null && resource.IsActive)
            {
                // Deaktivirajte resurs
                resource.IsActive = false;

                // Postavite EndTime
                resource.EndTime = DateTime.Now;

                // Izračunajte ActiveTime
                if (resource.StartTime != DateTime.MinValue) // Proverite da StartTime nije null
                {
                    resource.ActiveTime = (resource.EndTime - resource.StartTime).TotalSeconds;
                }

                double producedEnergy = resource.Power * (resource.ActiveTime / 3600.0); // Izračunajte proizvedenu energiju
                statistics.TotalProducedEnergy += producedEnergy; // Ažurirajte ukupno proizvedenu energiju
                statistics.TotalActivePower -= resource.Power; // Ažurirajte ukupnu aktivnu snagu

                // Sačuvajte izmenjene resurse u XML
                _xmlDataAccess.SaveResources(existingResources, statistics); // Sačuvajte izmene nazad u XML

                // Ispis na serveru za deaktivaciju resursa
                Console.WriteLine($"Resource with ID {resourceId} has been deactivated on server.");
                Console.WriteLine($"End Time: {resource.EndTime}");
                Console.WriteLine($"Active Time: {resource.ActiveTime} seconds");
                Console.WriteLine($"Produced Energy: {producedEnergy} kWh");

                return $"Resource with ID {resourceId} has stopped.\nActive time: {resource.ActiveTime} seconds.\nProduced energy: {producedEnergy} kWh.";
            }
            else
            {
                return "Resource not active or does not exist.";
            }
        }



        public void RegisterNewResource(DERResource resource)
        {
            // Učitajte postojeće resurse iz XML datoteke
            var existingResources = _xmlDataAccess.LoadResources();

            // Proverite da li resurs već postoji u XML datoteci
            if (!existingResources.Any(r => r.Id == resource.Id))
            {

                // Dodajte resurs u XML datoteku
                existingResources.Add(resource);
                _xmlDataAccess.SaveResources(existingResources, statistics); // Sačuvajte izmene nazad u XML

                Console.WriteLine($"Resource added: ID = {resource.Id}, Name = {resource.Name}, Power = {resource.Power} kW");
            }
            else
            {
                Console.WriteLine($"Resource with ID {resource.Id} already exists in XML. Skipping this entry.");
            }
        }



        public List<ResourceInfo> GetResourceStatus()
        {
            // Učitajte sve resurse iz XML datoteke
            var existingResources = _xmlDataAccess.LoadResources();
            List<ResourceInfo> resourceInfoList = new List<ResourceInfo>();

            // Učitajte ukupne vrednosti iz XML-a
            var (totalActivePower, totalProducedEnergy) = _xmlDataAccess.LoadTotals();

            foreach (var resource in existingResources)
            {
                var resourceInfo = new ResourceInfo
                {
                    Id = resource.Id,
                    Name = resource.Name,
                    Power = resource.Power,
                    IsActive = resource.IsActive,
                    TotalActivePower = totalActivePower, // Dodelite učitanu vrednost
                    TotalProducedEnergy = totalProducedEnergy // Dodelite učitanu vrednost
                };

                // Dodelite ActiveTime i druge informacije ako su dostupne
                resourceInfo.StartTime = resource.StartTime;
                resourceInfo.EndTime = resource.EndTime;
                resourceInfo.ActiveTime = resource.ActiveTime;

                resourceInfoList.Add(resourceInfo);
            }
            return resourceInfoList;
        }



        public ResourceSchedule GetSchedule(int resourceId)
        {
            // Učitajte sve resurse iz XML datoteke
            var existingResources = _xmlDataAccess.LoadResources();

            // Pronađite resurs na osnovu ID-a
            var resource = existingResources.FirstOrDefault(r => r.Id == resourceId);

            // Prikupite raspored za resurs
            if (resource != null)
            {
                // Pretpostavljamo da je raspored povezan sa resursom u XML-u, 
                // ako koristite poseban XML element za rasporede, dodajte tu logiku.
                var schedule = new ResourceSchedule
                {
                    ResourceId = resource.Id,
                    StartTime = resource.StartTime, // Očitajte iz resursa
                    EndTime = resource.EndTime, // Očitajte iz resursa
                    ActiveTime = resource.ActiveTime // Očitajte iz resursa
                };

                return schedule; // Vraća raspored
            }

            return null; // Ako resurs nije pronađen
        }


        public void ClearAllResources()
        {
            _xmlDataAccess.ClearResources(); // Očisti resurse iz XML datoteke
        }


    }
}
