namespace EShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesToProductSchema : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Products", "CategoryId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "CategoryId", c => c.Int(nullable: false));
        }
    }
}
