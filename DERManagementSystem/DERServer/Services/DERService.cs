using Common.Interfaces;
using Common.Models;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace DERServer.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class DERService : IDERService
    {
        private readonly Dictionary<int, DERResource> resources = new Dictionary<int, DERResource>(); // Skladišti sve registrovane resurse koristeći ID kao ključ.

        private readonly Dictionary<int, ResourceSchedule> schedules = new Dictionary<int, ResourceSchedule>(); // Skladišti rasporede za resurse prema njihovim ID-jevima.

        private readonly Statistics statistics = new Statistics(); // Čuva statistiku, uključujući ukupnu proizvedenu energiju.

        public string RegisterResource(int resourceId)
        {
            if (resources.ContainsKey(resourceId))
            {
                var schedule = new ResourceSchedule
                {
                    ResourceId = resourceId,
                    StartTime = DateTime.Now,
                    EndTime = DateTime.MinValue,
                    ActiveTime = 0.00
                };
                schedules[resourceId] = schedule;
                resources[resourceId].IsActive = true;
                statistics.TotalActivePower += resources[resourceId].Power;

                // Ispis na serveru za aktivaciju resursa
                Console.WriteLine($"Resource with ID {resourceId} is now active on server.");
                Console.WriteLine($"Start Time: {schedule.StartTime}");
                Console.WriteLine($"Power: {resources[resourceId].Power} kW");

                return $"Resource with ID {resourceId} is now active.\nPower: {resources[resourceId].Power} kW";
            }
            else
            {
                return "Resource not found.";
            }
        }

        public string UnregisterResource(int resourceId)
        {
            if (resources.ContainsKey(resourceId) && resources[resourceId].IsActive)
            {
                resources[resourceId].IsActive = false;

                if (schedules.TryGetValue(resourceId, out var schedule))
                {
                    schedule.EndTime = DateTime.Now;
                    schedule.ActiveTime = (schedule.EndTime - schedule.StartTime).TotalSeconds;
                    double producedEnergy = resources[resourceId].Power * (schedule.ActiveTime / 3600.0);
                    statistics.TotalProducedEnergy += producedEnergy;
                    statistics.TotalActivePower -= resources[resourceId].Power;

                    // Ispis na serveru za deaktivaciju resursa
                    Console.WriteLine($"Resource with ID {resourceId} has been deactivated on server.");
                    Console.WriteLine($"End Time: {schedule.EndTime}");
                    Console.WriteLine($"Active Time: {schedule.ActiveTime} seconds");
                    Console.WriteLine($"Produced Energy: {producedEnergy} kWh");

                    return $"Resource with ID {resourceId} has stopped.\nActive time: {schedule.ActiveTime} seconds.\nProduced energy: {producedEnergy} kWh.";
                }
                else
                {
                    return "No schedule found for this resource.";
                }
            }
            else
            {
                return "Resource not active or does not exist.";
            }
        }




        // Implementacija nove metode za postavljanje resursa sa rasporedom
        public void RegisterNewResource(DERResource resource)
        {
            if (!resources.ContainsKey(resource.Id))
            {
                resources[resource.Id] = resource;
                Console.WriteLine($"Resource added: ID = {resource.Id}, Name = {resource.Name}, Power = {resource.Power} kW");
            }
            else
            {
                Console.WriteLine($"Resource with ID {resource.Id} already exists.");
            }
        }

        public List<ResourceInfo> GetResourceStatus()
        {
            List<ResourceInfo> resourceInfoList = new List<ResourceInfo>();

            foreach (var resource in resources.Values)
            {
                var resourceInfo = new ResourceInfo
                {
                    Id = resource.Id,
                    Name = resource.Name,
                    Power = resource.Power,
                    IsActive = resource.IsActive,
                    TotalActivePower = statistics.TotalActivePower,
                    TotalProducedEnergy = statistics.TotalProducedEnergy
                };

                // Ako postoji raspored za resurs, dodaj informacije o rasporedu
                if (schedules.TryGetValue(resource.Id, out var schedule))
                {
                    resourceInfo.StartTime = schedule.StartTime;
                    resourceInfo.EndTime = schedule.EndTime;
                    resourceInfo.ActiveTime = schedule.ActiveTime;
                }

                resourceInfoList.Add(resourceInfo);
            }

            Console.WriteLine("Fetched resource status for all resources.");
            return resourceInfoList;
        }

        public ResourceSchedule GetSchedule(int resourceId)
        {
            // Proverava da li postoji raspored za dati ID resursa
            schedules.TryGetValue(resourceId, out ResourceSchedule schedule);
            return schedule; // Vraća raspored ako postoji ili `null` ako ne postoji
        }
    }
}
