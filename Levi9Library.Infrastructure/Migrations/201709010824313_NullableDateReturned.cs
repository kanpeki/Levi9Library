namespace Levi9Library.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableDateReturned : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserBooks", "DateReturned", c => c.DateTime(precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserBooks", "DateReturned", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
    }
}
