using System;
using System.Collections.Generic;
using System.IO;

namespace UserClient.Services
{
    public class FileResourceLoader
    {
        public List<DERResource> LoadResourcesFromFile(string filePath)
        {
            var resources = new List<DERResource>();

            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found.");
                return resources;
            }

            string[] lines = File.ReadAllLines(filePath);
            DERResource resource = null;

            foreach (var line in lines)
            {
                if (line.StartsWith("Name:"))
                {
                    resource = new DERResource(); // Kreiraj novi resurs na početku svakog zapisa
                    resource.Name = line.Split(':')[1].Trim();
                }
                else if (line.StartsWith("Power:") && resource != null)
                {
                    resource.Power = double.Parse(line.Split(':')[1].Trim());
                }
                else if (line.StartsWith("Status:") && resource != null)
                {
                    resource.IsActive = line.Split(':')[1].Trim().Equals("Active", StringComparison.OrdinalIgnoreCase);
                }
                else if (line.StartsWith("---") && resource != null)
                {
                    resources.Add(resource);
                    resource = null; // Postavi na null nakon dodavanja resursa
                }
            }

            return resources;
        }
    }
}
