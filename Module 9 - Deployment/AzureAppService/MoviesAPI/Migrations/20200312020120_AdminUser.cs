using Microsoft.EntityFrameworkCore.Migrations;

namespace MoviesAPI.Migrations
{
    public partial class AdminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'f123cfab-fad6-4591-ab52-11ed65b4e2d1', N'felipe@hotmail.com', N'FELIPE@HOTMAIL.COM', N'felipe@hotmail.com', N'FELIPE@HOTMAIL.COM', 0, N'AQAAAAEAACcQAAAAEMUr61c1UskCPQ8vcqAfLDY01QgIlAuTnfXOtBt9PnC1HEkgFY/C3ILYghpen9UVhQ==', N'RVOBVJXVI4WSRHPDK2SBNKIGY33K6DMA', N'3c8dfca3-6a68-4ad5-b92e-2a18b3668d2b', NULL, 0, 0, NULL, 1, 0)
GO
SET IDENTITY_INSERT [dbo].[AspNetUserClaims] ON 
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (1, N'f123cfab-fad6-4591-ab52-11ed65b4e2d1', N'http://schemas.microsoft.com/ws/2008/06/identity/claims/role', N'Admin')
GO
SET IDENTITY_INSERT [dbo].[AspNetUserClaims] OFF
GO

");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
    delete [dbo].[AspNetUsers] where [Id] = 'f123cfab-fad6-4591-ab52-11ed65b4e2d1'
    delete [dbo].[AspNetUserClaims] where [Id] = 1
");
        }
    }
}
