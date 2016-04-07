namespace picclicApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropUserName : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.SignUpUserModels", "Username");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SignUpUserModels", "Username", c => c.String());
        }
    }
}
