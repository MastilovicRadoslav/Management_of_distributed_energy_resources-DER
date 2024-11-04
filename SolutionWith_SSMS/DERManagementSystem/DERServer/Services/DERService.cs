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
        private readonly Statistics statistics = new Statistics(); // Čuva statistiku, uključujući ukupnu proizvedenu energiju.

        /// <summary>
        /// Aktivira resurs na osnovu prosleđenog ID-a. Ažurira početno vreme aktivacije resursa i povećava ukupnu aktivnu snagu.
        /// </summary>
        /// <param name="resourceId">ID resursa za aktivaciju</param>
        /// <returns>Poruka o statusu aktivacije resursa</returns>
        public string RegisterResource(int resourceId)
        {
            using (var context = new DERManagementContext())
            {
                var resource = context.DERResources.FirstOrDefault(r => r.Id == resourceId);

                if (resource != null && !resource.IsActive)
                {
                    resource.IsActive = true;
                    resource.StartTime = DateTime.Now;

                    var statistics = context.Statistics.FirstOrDefault();
                    if (statistics == null)
                    {
                        statistics = new Statistics { TotalActivePower = resource.Power, TotalProducedEnergy = 0 };
                        context.Statistics.Add(statistics);
                    }
                    else
                    {
                        statistics.TotalActivePower += resource.Power;
                    }

                    context.SaveChanges();

                    Console.WriteLine("\n------------------------------------------------------------");
                    Console.WriteLine("                   Resource Activation Summary              ");
                    Console.WriteLine("------------------------------------------------------------");
                    Console.WriteLine($"Resource ID           : {resourceId}");
                    Console.WriteLine($"Name                  : {resource.Name}");
                    Console.WriteLine($"Power                 : {resource.Power} kW");
                    Console.WriteLine($"Activation Start Time : {resource.StartTime:dd-MM-yyyy HH:mm:ss}");
                    Console.WriteLine("------------------------------------------------------------");
                    Console.WriteLine("Current Statistics:");
                    Console.WriteLine($"Total Active Power    : {statistics.TotalActivePower} kW");
                    Console.WriteLine("------------------------------------------------------------\n");

                    return $"\nResource with ID {resourceId} is now active.";
                }
                else
                {
                    return resource == null ? "Resource not found." : "Resource is already active.";
                }
            }
        }

        /// <summary>
        /// Deaktivira resurs na osnovu ID-a, postavlja vreme završetka i izračunava proizvedenu energiju.
        /// Ažurira statistiku ukupne proizvedene energije i aktivne snage.
        /// </summary>
        /// <param name="resourceId">ID resursa za deaktivaciju</param>
        /// <returns>Poruka o statusu deaktivacije resursa</returns>
        public string UnregisterResource(int resourceId)
        {
            using (var context = new DERManagementContext())
            {
                var resource = context.DERResources.FirstOrDefault(r => r.Id == resourceId);

                if (resource != null && resource.IsActive)
                {
                    resource.IsActive = false;
                    resource.EndTime = DateTime.Now;

                    if (resource.StartTime.HasValue)
                    {
                        resource.ActiveTime = (resource.EndTime.Value - resource.StartTime.Value).TotalSeconds;
                        double producedEnergy = resource.Power * (resource.ActiveTime / 3600.0); // kWh

                        var statistics = context.Statistics.FirstOrDefault();
                        if (statistics == null)
                        {
                            statistics = new Statistics
                            {
                                TotalActivePower = 0,
                                TotalProducedEnergy = producedEnergy
                            };
                            context.Statistics.Add(statistics);
                        }
                        else
                        {
                            statistics.TotalActivePower -= resource.Power;
                            statistics.TotalProducedEnergy += producedEnergy;
                        }

                        context.SaveChanges();

                        Console.WriteLine("------------------------------------------------------------");
                        Console.WriteLine("                   Resource Deactivation Summary            ");
                        Console.WriteLine("------------------------------------------------------------");
                        Console.WriteLine($"Resource ID           : {resourceId}");
                        Console.WriteLine($"Name                  : {resource.Name}");
                        Console.WriteLine($"Power                 : {resource.Power} kW");
                        Console.WriteLine($"Start Time            : {(resource.StartTime.HasValue ? resource.StartTime.Value.ToString("dd-MM-yyyy HH:mm:ss") : "N/A")}");
                        Console.WriteLine($"End Time              : {(resource.EndTime.HasValue ? resource.EndTime.Value.ToString("dd-MM-yyyy HH:mm:ss") : "N/A")}");
                        Console.WriteLine($"Active Duration       : {resource.ActiveTime} seconds");
                        Console.WriteLine("------------------------------------------------------------");
                        Console.WriteLine("Statistics Update:");
                        Console.WriteLine($"Total Active Power    : {statistics.TotalActivePower} kW");
                        Console.WriteLine($"Total Produced Energy : {statistics.TotalProducedEnergy} kWh");
                        Console.WriteLine("------------------------------------------------------------\n");

                        return $"Resource with ID {resourceId} has stopped.";
                    }
                    else
                    {
                        return "Resource was not active.";
                    }
                }
                else
                {
                    return "Resource not active or does not exist.";
                }
            }
        }

        /// <summary>
        /// Registruje novi resurs u bazi, proverava da li već postoji resurs sa istim karakteristikama.
        /// </summary>
        /// <param name="resource">Resurs koji se registruje</param>
        /// <returns>Vraća resurs sa dodeljenim ID-em ako je uspešno dodato, inače null</returns>
        public DERResource RegisterNewResource(DERResource resource)
        {
            using (var context = new DERManagementContext())
            {
                var existingResource = context.DERResources
                                             .FirstOrDefault(r => r.Name == resource.Name && r.Power == resource.Power);

                if (existingResource == null)
                {
                    context.DERResources.Add(resource);
                    context.SaveChanges(); // Sada je `resource.Id` dodeljen u bazi

                    return context.DERResources.FirstOrDefault(r => r.Id == resource.Id);
                }
                else
                {
                    Console.WriteLine("Resource with similar characteristics already exists. Skipping this entry.");
                    return null;
                }
            }
        }

        /// <summary>
        /// Preuzima status svih resursa u bazi, uključujući informacije o aktivnim i neaktivnim resursima.
        /// </summary>
        /// <returns>Lista informacija o resursima</returns>
        public List<ResourceInfo> GetResourceStatus()
        {
            using (var context = new DERManagementContext())
            {
                var resources = context.DERResources.ToList();
                List<ResourceInfo> resourceInfoList = new List<ResourceInfo>();

                foreach (var resource in resources)
                {
                    var resourceInfo = new ResourceInfo
                    {
                        Id = resource.Id,
                        Name = resource.Name,
                        Power = resource.Power,
                        IsActive = resource.IsActive,
                        StartTime = resource.StartTime,
                        EndTime = resource.EndTime,
                        ActiveTime = resource.ActiveTime
                    };

                    resourceInfoList.Add(resourceInfo);
                }

                return resourceInfoList;
            }
        }

        /// <summary>
        /// Briše sve resurse i statistiku iz baze podataka i resetuje ID brojač.
        /// </summary>
        public void ClearAllResources()
        {
            using (var context = new DERManagementContext())
            {
                context.DERResources.RemoveRange(context.DERResources);
                context.Statistics.RemoveRange(context.Statistics);
                context.SaveChanges();

                context.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('DERResources', RESEED, 0)");
				context.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('Statistics', RESEED, 0)");

            }
        }

        /// <summary>
        /// Dohvata statistiku koja sadrži ukupnu aktivnu snagu i ukupnu proizvedenu energiju.
        /// </summary>
        /// <returns>Statistički podaci iz baze</returns>
        public Statistics GetStatistics()
        {
            using (var context = new DERManagementContext())
            {
                return context.Statistics.FirstOrDefault() ?? new Statistics();
            }
        }
    }
}
