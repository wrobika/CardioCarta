namespace CardioCarta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Nullable_Personal_Fileds : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Surname", c => c.String(maxLength: 50));
            AlterColumn("dbo.AspNetUsers", "FirstName", c => c.String(maxLength: 50));
            AlterColumn("dbo.AspNetUsers", "CityOrVillage", c => c.String(maxLength: 50));
            AlterColumn("dbo.AspNetUsers", "District", c => c.String(maxLength: 50));
            AlterColumn("dbo.AspNetUsers", "House", c => c.String(maxLength: 10));
            AlterColumn("dbo.AspNetUsers", "Flat", c => c.String());
            AlterColumn("dbo.AspNetUsers", "PostalCode", c => c.String(maxLength: 6));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "PostalCode", c => c.String(nullable: false, maxLength: 6));
            AlterColumn("dbo.AspNetUsers", "Flat", c => c.String(maxLength: 10));
            AlterColumn("dbo.AspNetUsers", "House", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.AspNetUsers", "District", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.AspNetUsers", "CityOrVillage", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.AspNetUsers", "FirstName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.AspNetUsers", "Surname", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
