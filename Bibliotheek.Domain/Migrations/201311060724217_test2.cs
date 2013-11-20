namespace Bibliotheek.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Etiket", "Testje");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Etiket", "Testje", c => c.Int(nullable: false));
        }
    }
}
