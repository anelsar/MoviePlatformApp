namespace APP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Movies",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        MovieName = c.String(),
                        MovieDescription = c.String(),
                        MovieDuration = c.Int(nullable: false),
                        MovieRating = c.Double(nullable: false),
                        MovieStreamingLink = c.String(),
                        MovieImagePath = c.String(),
                        MovieActors = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MyRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.MyUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.MyRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.MyUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.MyUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.MyUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MyUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserMovies",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        MovieId = c.String(maxLength: 128),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Movies", t => t.MovieId)
                .ForeignKey("dbo.MyUsers", t => t.UserId)
                .Index(t => t.MovieId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.MyUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.MyUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MyUserRoles", "UserId", "dbo.MyUsers");
            DropForeignKey("dbo.MyUserLogins", "UserId", "dbo.MyUsers");
            DropForeignKey("dbo.UserMovies", "UserId", "dbo.MyUsers");
            DropForeignKey("dbo.UserMovies", "MovieId", "dbo.Movies");
            DropForeignKey("dbo.MyUserClaims", "UserId", "dbo.MyUsers");
            DropForeignKey("dbo.MyUserRoles", "RoleId", "dbo.MyRoles");
            DropIndex("dbo.MyUserLogins", new[] { "UserId" });
            DropIndex("dbo.UserMovies", new[] { "UserId" });
            DropIndex("dbo.UserMovies", new[] { "MovieId" });
            DropIndex("dbo.MyUserClaims", new[] { "UserId" });
            DropIndex("dbo.MyUsers", "UserNameIndex");
            DropIndex("dbo.MyUserRoles", new[] { "RoleId" });
            DropIndex("dbo.MyUserRoles", new[] { "UserId" });
            DropIndex("dbo.MyRoles", "RoleNameIndex");
            DropTable("dbo.MyUserLogins");
            DropTable("dbo.UserMovies");
            DropTable("dbo.MyUserClaims");
            DropTable("dbo.MyUsers");
            DropTable("dbo.MyUserRoles");
            DropTable("dbo.MyRoles");
            DropTable("dbo.Movies");
        }
    }
}
