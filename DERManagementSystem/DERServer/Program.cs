using DERServer.Services;
using System;
using System.ServiceModel;

namespace DERServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Kreira novu instancu DERService
            DERService service = new DERService();

            // Kreira novi ServiceHost za DERService tip, koji omogućava hostovanje WCF servisa.
            using (ServiceHost host = new ServiceHost(service))
            {
                host.Open(); // Otvara ServiceHost i omogućava prihvatanje zahteva od klijenata.
                Console.WriteLine("\nServer is running."); // Prikazuje poruku da je server pokrenut.
                Console.WriteLine("Press any key to stop the server."); // Poruka za korisnika da zaustavi server pritiskom na bilo koji taster.
                Console.ReadKey(); // Čeka da korisnik pritisne bilo koji taster pre zatvaranja servisa.

                // Sačuvaj podatke u fajl pre nego što zatvoriš server
                service.SaveDataToFile("C:\\Users\\Lenovo\\Desktop\\Praksa\\rezultati.txt"); // Postavite putanju gde želite sačuvati fajl
            }
        }
    }
}
