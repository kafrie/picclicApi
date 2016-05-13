namespace picclicApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateSignUpUserTableWithOldUserId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SignUpUserModels", "OldUserId", c => c.String());
            DropColumn("dbo.SignUpUserModels", "Password");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SignUpUserModels", "Password", c => c.String());
            DropColumn("dbo.SignUpUserModels", "OldUserId");
        }
    }
}
