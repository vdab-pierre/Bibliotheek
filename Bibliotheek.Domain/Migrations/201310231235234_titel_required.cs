namespace Bibliotheek.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class titel_required : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Boek", "Titel", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Boek", "Titel", c => c.String());
        }
    }
}
