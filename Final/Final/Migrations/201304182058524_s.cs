namespace Final.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class s : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectModels",
                c => new
                    {
                        ProjectID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProjectID)
                .ForeignKey("dbo.UserModels", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.UserModels",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(nullable: false),
                        Phone = c.String(),
                        Password = c.String(),
                        RoleID = c.Int(),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.RoleModels", t => t.RoleID)
                .Index(t => t.RoleID);
            
            CreateTable(
                "dbo.RoleModels",
                c => new
                    {
                        RoleID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.RoleID);
            
            CreateTable(
                "dbo.UATModels",
                c => new
                    {
                        UATID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Version = c.String(),
                        Intro = c.String(),
                        Started = c.DateTime(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                        ProjectID = c.Int(nullable: false),
                        StatusD = c.Int(),
                    })
                .PrimaryKey(t => t.UATID)
                .ForeignKey("dbo.ProjectModels", t => t.ProjectID, cascadeDelete: true)
                .ForeignKey("dbo.StatusModels", t => t.StatusD)
                .Index(t => t.ProjectID)
                .Index(t => t.StatusD);
            
            CreateTable(
                "dbo.StatusModels",
                c => new
                    {
                        StatusD = c.Int(nullable: false, identity: true),
                        StatusType = c.String(),
                    })
                .PrimaryKey(t => t.StatusD);
            
            CreateTable(
                "dbo.TestCaseModels",
                c => new
                    {
                        TestCaseID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Steps = c.String(),
                        Comments = c.String(),
                        StatusD = c.Int(),
                        UATID = c.Int(),
                    })
                .PrimaryKey(t => t.TestCaseID)
                .ForeignKey("dbo.UATModels", t => t.UATID)
                .ForeignKey("dbo.StatusModels", t => t.StatusD)
                .Index(t => t.UATID)
                .Index(t => t.StatusD);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.TestCaseModels", new[] { "StatusD" });
            DropIndex("dbo.TestCaseModels", new[] { "UATID" });
            DropIndex("dbo.UATModels", new[] { "StatusD" });
            DropIndex("dbo.UATModels", new[] { "ProjectID" });
            DropIndex("dbo.UserModels", new[] { "RoleID" });
            DropIndex("dbo.ProjectModels", new[] { "UserID" });
            DropForeignKey("dbo.TestCaseModels", "StatusD", "dbo.StatusModels");
            DropForeignKey("dbo.TestCaseModels", "UATID", "dbo.UATModels");
            DropForeignKey("dbo.UATModels", "StatusD", "dbo.StatusModels");
            DropForeignKey("dbo.UATModels", "ProjectID", "dbo.ProjectModels");
            DropForeignKey("dbo.UserModels", "RoleID", "dbo.RoleModels");
            DropForeignKey("dbo.ProjectModels", "UserID", "dbo.UserModels");
            DropTable("dbo.TestCaseModels");
            DropTable("dbo.StatusModels");
            DropTable("dbo.UATModels");
            DropTable("dbo.RoleModels");
            DropTable("dbo.UserModels");
            DropTable("dbo.ProjectModels");
        }
    }
}
