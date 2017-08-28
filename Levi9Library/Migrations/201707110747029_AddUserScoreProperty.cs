namespace Levi9Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserScoreProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "UserScore", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "UserScore");
        }
    }
}
