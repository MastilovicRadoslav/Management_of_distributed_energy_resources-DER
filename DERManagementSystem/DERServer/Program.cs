using System;
using System.ServiceModel;
using DERServer.Services;

namespace DERServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Kreira novi ServiceHost za DERService tip, koji omogućava hostovanje WCF servisa.
            using (ServiceHost host = new ServiceHost(typeof(DERService)))
            {
                host.Open(); // Otvara ServiceHost i omogućava prihvatanje zahteva od klijenata.
                Console.WriteLine("DER Server is running..."); // Prikazuje poruku da je server pokrenut.
                Console.WriteLine("Press any key to stop the server."); // Poruka za korisnika da zaustavi server pritiskom na bilo koji taster.
                Console.ReadKey(); // Čeka da korisnik pritisne bilo koji taster pre zatvaranja servisa.
            } // Zatvara ServiceHost kada se izađe iz 'using' bloka, oslobađajući resurse.
        }
    }
}
