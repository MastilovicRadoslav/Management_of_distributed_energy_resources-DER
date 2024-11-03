using Common.Interfaces;
using Common.Models;
using DERServer.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.ServiceModel;

namespace DERServer.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class DERService : IDERService
    {

        private readonly Statistics statistics = new Statistics(); // Čuva statistiku, uključujući ukupnu proizvedenu energiju.


        public string RegisterResource(int resourceId)
        {
            using (var context = new DERManagementContext())
            {
                var resource = context.DERResources.FirstOrDefault(r => r.Id == resourceId);

                if (resource != null)
                {
                    resource.IsActive = true;
                    resource.StartTime = DateTime.Now;

                    // Pronađi ili kreiraj zapis u tabeli Statistics
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

                    Console.WriteLine($"Resource with ID {resourceId} is now active.");
                    Console.WriteLine($"Name: {resource.Name}");
                    Console.WriteLine($"Power: {resource.Power} kW");
                    Console.WriteLine($"Start Time: {resource.StartTime}");
                    Console.WriteLine($"Total Active Power: {statistics.TotalActivePower} kW");

                    return $"Resource with ID {resourceId} is now active.";
                }
                else
                {
                    return "Resource not found.";
                }
            }
        }


        public string UnregisterResource(int resourceId)
        {
            using (var context = new DERManagementContext())
            {
                var resource = context.DERResources.FirstOrDefault(r => r.Id == resourceId);

                if (resource != null && resource.IsActive)
                {
                    // Deaktivirajte resurs
                    resource.IsActive = false;
                    resource.EndTime = DateTime.Now;

                    if (resource.StartTime.HasValue)
                    {
                        resource.ActiveTime = (resource.EndTime.Value - resource.StartTime.Value).TotalSeconds;
                        double producedEnergy = resource.Power * (resource.ActiveTime / 3600.0); // kWh

                        // Pronađite ili kreirajte zapis u tabeli Statistics
                        var statistics = context.Statistics.FirstOrDefault();
                        if (statistics == null)
                        {
                            statistics = new Statistics
                            {
                                TotalActivePower = 0, // Resetujemo jer je resurs deaktiviran
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

                        // Ispis informacija o resursu
                        Console.WriteLine($"Resource with ID {resourceId} has been deactivated.");
                        Console.WriteLine($"Name: {resource.Name}");
                        Console.WriteLine($"Power: {resource.Power} kW");
                        Console.WriteLine($"Start Time: {resource.StartTime}");
                        Console.WriteLine($"End Time: {resource.EndTime}");
                        Console.WriteLine($"Active Time: {resource.ActiveTime} seconds");
                        Console.WriteLine($"Produced Energy: {statistics.TotalProducedEnergy} kWh");
                        Console.WriteLine($"Active Energy: {statistics.TotalActivePower} kWh");


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

                    // Preuzmi ažurirani resurs sa generisanim ID-jem
                    return context.DERResources.FirstOrDefault(r => r.Id == resource.Id);
                }
                else
                {
                    Console.WriteLine("Resource with similar characteristics already exists. Skipping this entry.");
                    return null; // Vrati `null` ako resurs već postoji
                }
            }
        }



        public List<ResourceInfo> GetResourceStatus()
        {
            using (var context = new DERManagementContext()) // Inicijalizujte DbContext
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

        public void ClearAllResources()
        {
            using (var context = new DERManagementContext()) // Inicijalizujte DbContext
            {
                // Brisanje svih resursa iz tabela
                context.DERResources.RemoveRange(context.DERResources);
                context.Statistics.RemoveRange(context.Statistics);
                context.SaveChanges(); // Sačuvajte promene u bazi

                // Resetovanje IDENTITY vrednosti za DERResources tabelu
                context.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('DERResources', RESEED, 0)");
            }

            //Console.WriteLine("All resources cleared from the database and ID reset.");
        }

        public Statistics GetStatistics()
        {
            using (var context = new DERManagementContext())
            {
                return context.Statistics.FirstOrDefault() ?? new Statistics();
            }
        }

    }
}
