namespace CSMS.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateCarsTable : DbMigration
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
                    Type = c.Int(nullable: false),
                    Plate = c.String(),
                })
                .PrimaryKey(t => t.CarId);

        }

        public override void Down()
        {
            DropTable("dbo.Cars");
        }
    }
}
