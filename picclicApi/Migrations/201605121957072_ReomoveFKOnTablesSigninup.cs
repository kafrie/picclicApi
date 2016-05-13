namespace picclicApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReomoveFKOnTablesSigninup : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SignInModels", "UserId", "dbo.SignUpUserModels");
            DropIndex("dbo.SignInModels", new[] { "UserId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.SignInModels", "UserId");
            AddForeignKey("dbo.SignInModels", "UserId", "dbo.SignUpUserModels", "UserId");
        }
    }
}
