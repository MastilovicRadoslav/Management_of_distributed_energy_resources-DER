using Common.Models; // Uključite putanju do vaših modela
using System.Data.Entity;

namespace DERServer.Data
{
    public class DERManagementContext : DbContext
    {
        public DERManagementContext() : base("DERManagement") // Konekcioni string iz App.config
        {
        }

        // Definišite DbSet-ove za entitete
        public DbSet<DERResource> DERResources { get; set; }
        public DbSet<Statistics> Statistics { get; set; }


        // Metoda za podešavanje modela
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
