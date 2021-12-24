namespace Appdev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createTableForcoureCategory2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Assigns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourseId = c.Int(nullable: false),
                        TrainerId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.TrainerId, cascadeDelete: true)
                .Index(t => t.CourseId)
                .Index(t => t.TrainerId);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Enrolls",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourseId = c.Int(nullable: false),
                        TraineeId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.TraineeId, cascadeDelete: true)
                .Index(t => t.CourseId)
                .Index(t => t.TraineeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Enrolls", "TraineeId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Enrolls", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Assigns", "TrainerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Assigns", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Courses", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Enrolls", new[] { "TraineeId" });
            DropIndex("dbo.Enrolls", new[] { "CourseId" });
            DropIndex("dbo.Courses", new[] { "CategoryId" });
            DropIndex("dbo.Assigns", new[] { "TrainerId" });
            DropIndex("dbo.Assigns", new[] { "CourseId" });
            DropTable("dbo.Enrolls");
            DropTable("dbo.Categories");
            DropTable("dbo.Courses");
            DropTable("dbo.Assigns");
        }
    }
}
