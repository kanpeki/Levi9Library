namespace Levi9Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeDateReturnedNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserBooks", "DateReturned", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserBooks", "DateReturned", c => c.DateTime(nullable: false));
        }
    }
}
