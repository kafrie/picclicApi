namespace picclicApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class makeUserNameUniqueAddUserIDasID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SignInModels", "UserName", c => c.String(maxLength: 450));
            AddColumn("dbo.SignUpUserModels", "UserName", c => c.String(maxLength: 450));
            CreateIndex("dbo.SignInModels", "UserName", unique: true);
            CreateIndex("dbo.SignUpUserModels", "UserName", unique: true);
            DropColumn("dbo.SignUpUserModels", "OldUserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SignUpUserModels", "OldUserId", c => c.String());
            DropIndex("dbo.SignUpUserModels", new[] { "UserName" });
            DropIndex("dbo.SignInModels", new[] { "UserName" });
            DropColumn("dbo.SignUpUserModels", "UserName");
            DropColumn("dbo.SignInModels", "UserName");
        }
    }
}
