namespace Levi9Library.Infrastructure.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class OverdueCountIsBannedLastBannedDate : DbMigration
	{
		public override void Up()
		{
			AddColumn("dbo.AspNetUsers", "OverDueCount", c => c.Int(nullable: false));
			AddColumn("dbo.AspNetUsers", "IsBanned", c => c.Boolean(nullable: false));
			AddColumn("dbo.AspNetUsers", "LastBannedDate", c => c.DateTime());
		}

		public override void Down()
		{
			DropColumn("dbo.AspNetUsers", "LastBannedDate");
			DropColumn("dbo.AspNetUsers", "IsBanned");
			DropColumn("dbo.AspNetUsers", "OverDueCount");
		}
	}
}
