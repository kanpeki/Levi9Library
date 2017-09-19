namespace Levi9Library.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReplacedAvailableCopiesWithBorrowedCount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "BorrowedCount", c => c.Int(nullable: false));
            DropColumn("dbo.Books", "AvailableCopies");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Books", "AvailableCopies", c => c.Int(nullable: false));
            DropColumn("dbo.Books", "BorrowedCount");
        }
    }
}
