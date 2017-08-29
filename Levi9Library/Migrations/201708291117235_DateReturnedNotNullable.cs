namespace Levi9Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateReturnedNotNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserBooks", "DateReturned", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserBooks", "DateReturned", c => c.DateTime());
        }
    }
}
