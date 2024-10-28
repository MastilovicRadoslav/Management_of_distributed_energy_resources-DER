using Common.Interfaces;
using Common.Models;
using System;
using System.Collections.Generic;

namespace DERServer.Services
{
    public class DERService : IDERService
    {
        private readonly Dictionary<int, DERResource> resources = new Dictionary<int, DERResource>(); // Skladišti sve registrovane resurse koristeći ID kao ključ.

        private readonly Dictionary<int, ResourceSchedule> schedules = new Dictionary<int, ResourceSchedule>(); // Skladišti rasporede za resurse prema njihovim ID-jevima.

        private readonly Statistics statistics = new Statistics(); // Čuva statistiku, uključujući ukupnu proizvedenu energiju.

        public void RegisterResource(DERResource resource)
        {
            if (!resources.ContainsKey(resource.Id)) // Proverava da li resurs već postoji prema ID-ju.
            {
                resources[resource.Id] = resource; // Dodaje novi resurs u kolekciju.
                Console.WriteLine($"Resource {resource.Name} registered with ID {resource.Id}."); // Ispisuje potvrdu registracije resursa.

            }
        }

        public ResourceSchedule GetSchedule(int resourceId)
        {
            if (schedules.TryGetValue(resourceId, out ResourceSchedule schedule)) // Pokušava da pronađe raspored za dati ID resursa.
            {
                Console.WriteLine($"Schedule sent for Resource ID {resourceId}."); // Ispisuje potvrdu o slanju rasporeda.
                return schedule; // Vraća raspored ako postoji.
            }
            Console.WriteLine($"No schedule found for Resource ID {resourceId}."); // Obaveštava da raspored nije pronađen.
            return null; // Vraća `null` ako raspored ne postoji.
        }

        public void LogProduction(int resourceId, double producedEnergy)
        {
            if (resources.ContainsKey(resourceId)) // Proverava da li resurs postoji u kolekciji.
            {
                statistics.TotalProducedEnergy += producedEnergy; // Dodaje proizvedenu energiju ukupnoj proizvedenoj energiji.
                Console.WriteLine($"Energy logged for Resource ID {resourceId}: {producedEnergy} kWh."); // Ispisuje potvrdu o logovanju energije.
            }
        }

        // Implementacija nove metode za postavljanje rasporeda
        public void SetSchedule(ResourceSchedule schedule)
        {
            schedules[schedule.ResourceId] = schedule;
            Console.WriteLine($"Schedule set for Resource ID {schedule.ResourceId}: Start - {schedule.StartTime}, End - {schedule.EndTime}");
        }
    }
}
