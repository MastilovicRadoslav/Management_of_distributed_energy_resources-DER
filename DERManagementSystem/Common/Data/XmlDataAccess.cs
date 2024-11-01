using Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DERServer.Data
{
    public class XmlDataAccess
    {
        private readonly string _filePath;

        public XmlDataAccess(string filePath)
        {
            _filePath = filePath;

            // Proverite da li datoteka postoji, ako ne, kreirajte je
            if (!File.Exists(_filePath))
            {
                CreateEmptyXmlFile();
            }
        }

        private void CreateEmptyXmlFile()
        {
            // Kreirajte prazan XML sa korenskim elementom
            var emptyDoc = new XDocument(new XElement("Resources"));
            emptyDoc.Save(_filePath); // Sačuvajte novi XML na datoteci
            Console.WriteLine($"XML file created at: {_filePath}");
        }

        public List<DERResource> LoadResources()
        {
            // Proveravamo da li datoteka postoji
            if (!System.IO.File.Exists(_filePath))
            {
                Console.WriteLine("XML file does not exist. Returning an empty resource list.");
                return new List<DERResource>(); // Vraća praznu listu
            }

            XDocument doc;

            try
            {
                // Učitajte XML dokument
                doc = XDocument.Load(_filePath);
            }
            catch (System.Xml.XmlException)
            {
                // Kreirajte prazan XML sa korenskim elementom
                doc = new XDocument(new XElement("Resources"));
                doc.Save(_filePath); // Sačuvajte novi XML
            }

            // Proverite da li dokument ima korenski element
            if (doc.Root == null || !doc.Root.HasElements)
            {
                return new List<DERResource>(); // Vraća praznu listu
            }

            // Učitajte resurse iz XML-a
            var resources = doc.Descendants("Resource").Select(element => new DERResource
            {
                Id = (int)element.Element("ResourceID"),
                Name = (string)element.Element("Name"),
                Power = (double)element.Element("Power"),
                IsActive = (string)element.Element("Status") == "Active",

                // Proverite da li element postoji pre pristupa
                StartTime = (DateTime?)element.Element("StartTime") ?? DateTime.MinValue, // Postavite na MinValue ako ne postoji
                EndTime = (DateTime?)element.Element("EndTime") ?? DateTime.MinValue,     // Postavite na MinValue ako ne postoji
                ActiveTime = (double?)element.Element("ActiveTime") ?? 0.0                // Postavite na 0 ako ne postoji
            }).ToList();

            return resources;
        }



        public void SaveResources(List<DERResource> resources, Statistics statistics)
        {
            XElement root = new XElement("Resources",
                from r in resources
                select new XElement("Resource",
                    new XElement("ResourceID", r.Id),
                    new XElement("Name", r.Name),
                    new XElement("Power", r.Power),
                    new XElement("Status", r.IsActive ? "Active" : "Inactive"),
                    new XElement("StartTime", r.StartTime),
                    new XElement("EndTime", r.EndTime),
                    new XElement("ActiveTime", r.ActiveTime)
                ));

            // Dodajte nova polja na kraju XML-a
            root.Add(new XElement("TotalActivePower", statistics.TotalActivePower));
            root.Add(new XElement("TotalProducedEnergy", statistics.TotalProducedEnergy));

            root.Save(_filePath);
        }

        public void ClearResources()
        {
            // Kreirajte prazan XML sa korenskim elementom
            var emptyDoc = new XDocument(new XElement("Resources"));
            emptyDoc.Save(_filePath); // Sačuvajte novi prazan XML
        }

        public (double TotalActivePower, double TotalProducedEnergy) LoadTotals()
        {
            // Učitajte XML datoteku
            XDocument doc = XDocument.Load(_filePath);

            // Inicijalizujte promenljive
            double totalActivePower = 0;
            double totalProducedEnergy = 0;

            // Proverite da li postoji korenski element
            if (doc.Root != null)
            {
                // Proverite da li elementi postoje i dodajte njihove vrednosti
                var totalActivePowerElement = doc.Root.Element("TotalActivePower");
                var totalProducedEnergyElement = doc.Root.Element("TotalProducedEnergy");

                if (totalActivePowerElement != null)
                {
                    totalActivePower = (double)totalActivePowerElement;
                }

                if (totalProducedEnergyElement != null)
                {
                    totalProducedEnergy = (double)totalProducedEnergyElement;
                }
            }
            else
            {
                Console.WriteLine("XML file is empty or has no root element.");
            }

            return (totalActivePower, totalProducedEnergy);
        }


    }
}
