namespace picclicApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pswpublic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SignUpUserModels", "Password", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SignUpUserModels", "Password");
        }
    }
}
