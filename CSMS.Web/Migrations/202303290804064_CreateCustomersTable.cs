namespace CSMS.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateCustomersTable : DbMigration
    {
        public override void Up()
        {
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

        }

        public override void Down()
        {
            DropTable("dbo.Customers");
        }
    }
}
