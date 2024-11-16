namespace DERServer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DERResources",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                    Power = c.Double(nullable: false),
                    IsActive = c.Boolean(nullable: false),
                    StartTime = c.DateTime(),
                    EndTime = c.DateTime(),
                    ActiveTime = c.Double(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Statistics",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TotalActivePower = c.Double(nullable: false),
                    TotalProducedEnergy = c.Double(nullable: false),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.Statistics");
            DropTable("dbo.DERResources");
        }
    }
}
