namespace Levi9Library.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BookIsDisabled : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "IsDisabled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "IsDisabled");
        }
    }
}
