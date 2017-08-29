namespace Levi9Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ColumnDateTime2 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.UserBooks");
            AlterColumn("dbo.UserBooks", "DateBorrowed", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.UserBooks", "DateReturned", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddPrimaryKey("dbo.UserBooks", new[] { "Id", "BookId", "DateBorrowed" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.UserBooks");
            AlterColumn("dbo.UserBooks", "DateReturned", c => c.DateTime(nullable: false));
            AlterColumn("dbo.UserBooks", "DateBorrowed", c => c.DateTime(nullable: false));
            AddPrimaryKey("dbo.UserBooks", new[] { "Id", "BookId", "DateBorrowed" });
        }
    }
}
