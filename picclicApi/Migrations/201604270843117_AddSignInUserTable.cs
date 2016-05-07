namespace picclicApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSignInUserTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SignInModels",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.SignUpUserModels", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SignInModels", "UserId", "dbo.SignUpUserModels");
            DropIndex("dbo.SignInModels", new[] { "UserId" });
            DropTable("dbo.SignInModels");
        }
    }
}
