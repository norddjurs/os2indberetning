namespace Infrastructure.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BankAccounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.String(nullable: false, unicode: false),
                        Description = c.String(nullable: false, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            //AddColumn("dbo.Reports", "Timestamp", c => c.String(unicode: false));
            //AddColumn("dbo.Reports", "AccountNumber", c => c.String(unicode: false));
            //AddColumn("dbo.Reports", "KilometerAllowance", c => c.Int());
            AddColumn("dbo.MobileTokens", "Description", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MobileTokens", "Description");
            //DropColumn("dbo.Reports", "KilometerAllowance");
            //DropColumn("dbo.Reports", "AccountNumber");
            //DropColumn("dbo.Reports", "Timestamp");
            DropTable("dbo.BankAccounts");
        }
    }
}