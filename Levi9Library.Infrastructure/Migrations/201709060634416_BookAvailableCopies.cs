namespace Levi9Library.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BookAvailableCopies : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "AvailableCopies", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "AvailableCopies");
        }
    }
}
