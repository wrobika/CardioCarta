namespace CardioCarta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddcolumnsforAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "CityOrVillage", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.AspNetUsers", "District", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.AspNetUsers", "Street", c => c.String(maxLength: 50));
            AddColumn("dbo.AspNetUsers", "House", c => c.String(nullable: false, maxLength: 10));
            AddColumn("dbo.AspNetUsers", "Flat", c => c.String(maxLength: 10));
            AddColumn("dbo.AspNetUsers", "PostalCode", c => c.String(nullable: false, maxLength: 6));
            AlterColumn("dbo.AspNetUsers", "Surname", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.AspNetUsers", "FirstName", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "FirstName", c => c.String());
            AlterColumn("dbo.AspNetUsers", "Surname", c => c.String());
            DropColumn("dbo.AspNetUsers", "PostalCode");
            DropColumn("dbo.AspNetUsers", "Flat");
            DropColumn("dbo.AspNetUsers", "House");
            DropColumn("dbo.AspNetUsers", "Street");
            DropColumn("dbo.AspNetUsers", "District");
            DropColumn("dbo.AspNetUsers", "CityOrVillage");
        }
    }
}
