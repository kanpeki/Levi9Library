namespace Levi9Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserBookTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserBooks",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        BookId = c.Int(nullable: false),
                        DateBorrowed = c.DateTime(nullable: false),
                        DateReturned = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.BookId, t.DateBorrowed })
                .ForeignKey("dbo.AspNetUsers", t => t.Id, cascadeDelete: true)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.BookId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserBooks", "BookId", "dbo.Books");
            DropForeignKey("dbo.UserBooks", "Id", "dbo.AspNetUsers");
            DropIndex("dbo.UserBooks", new[] { "BookId" });
            DropIndex("dbo.UserBooks", new[] { "Id" });
            DropTable("dbo.UserBooks");
        }
    }
}
