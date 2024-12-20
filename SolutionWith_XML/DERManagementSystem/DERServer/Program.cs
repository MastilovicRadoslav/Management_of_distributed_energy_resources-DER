﻿using DERServer.Services;
using System;
using System.ServiceModel;

namespace DERServer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(new DERService()))
            {
                host.Open(); // Otvara ServiceHost i omogućava prihvatanje zahteva od klijenata.
                Console.WriteLine("\nServer is running."); // Prikazuje poruku da je server pokrenut.
                Console.WriteLine("Press any key to stop the server."); // Poruka za korisnika da zaustavi server pritiskom na bilo koji taster.
                Console.ReadKey(); // Čeka da korisnik pritisne bilo koji taster pre zatvaranja servisa.
            }
        }
    }
}
