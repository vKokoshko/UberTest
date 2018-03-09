namespace TestProjectUber.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CalendarEntries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Day = c.Int(nullable: false),
                        HourId = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Hours", t => t.HourId, cascadeDelete: true)
                .Index(t => t.HourId)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Value = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Hours",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(nullable: false, maxLength: 5),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CalendarEntries", "HourId", "dbo.Hours");
            DropForeignKey("dbo.CalendarEntries", "CourseId", "dbo.Courses");
            DropIndex("dbo.CalendarEntries", new[] { "CourseId" });
            DropIndex("dbo.CalendarEntries", new[] { "HourId" });
            DropTable("dbo.Hours");
            DropTable("dbo.Courses");
            DropTable("dbo.CalendarEntries");
        }
    }
}
