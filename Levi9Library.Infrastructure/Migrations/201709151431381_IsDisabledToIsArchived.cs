namespace Levi9Library.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsDisabledToIsArchived : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "IsArchived", c => c.Boolean(nullable: false));
            DropColumn("dbo.Books", "IsDisabled");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Books", "IsDisabled", c => c.Boolean(nullable: false));
            DropColumn("dbo.Books", "IsArchived");
        }
    }
}
