namespace refactor_me.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class InitialCreate : DbMigration
	{
		public override void Up()
		{
			CreateTable(
				"dbo.ProductOptions",
				c => new
				{
					Id = c.Guid(nullable: false),
					ProductId = c.Guid(nullable: false),
					Name = c.String(nullable: false, maxLength: 1000),
					Description = c.String(),
				})
				.PrimaryKey(t => t.Id)
				.ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
				.Index(t => t.ProductId);

			CreateTable(
				"dbo.Products",
				c => new
				{
					Id = c.Guid(nullable: false),
					Name = c.String(nullable: false, maxLength: 1000),
					Description = c.String(),
					Price = c.Decimal(nullable: false, precision: 18, scale: 2),
					DeliveryPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
				})
				.PrimaryKey(t => t.Id);

		}

		public override void Down()
		{
			DropForeignKey("dbo.ProductOptions", "ProductId", "dbo.Products");
			DropIndex("dbo.ProductOptions", new[] { "ProductId" });
			DropTable("dbo.Products");
			DropTable("dbo.ProductOptions");
		}
	}
}
