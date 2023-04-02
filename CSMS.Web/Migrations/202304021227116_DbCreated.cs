namespace CSMS.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbCreated : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cars",
                c => new
                    {
                        CarId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Model = c.String(),
                        Year = c.Int(nullable: false),
                        Type = c.String(),
                        Plate = c.String(),
                    })
                .PrimaryKey(t => t.CarId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Surname = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                        Plate = c.String(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.CustomerId);
            
            CreateTable(
                "dbo.FaultRecords",
                c => new
                    {
                        RecordId = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        CarKm = c.Int(nullable: false),
                        ArrivalDate = c.DateTime(nullable: false),
                        Record = c.String(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RecordId)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FaultRecords", "CustomerId", "dbo.Customers");
            DropIndex("dbo.FaultRecords", new[] { "CustomerId" });
            DropTable("dbo.FaultRecords");
            DropTable("dbo.Customers");
            DropTable("dbo.Cars");
        }
    }
}
