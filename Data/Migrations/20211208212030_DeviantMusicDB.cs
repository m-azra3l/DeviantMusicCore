using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeviantMusicCore.Data.Migrations
{
    public partial class DeviantMusicDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdsBs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Url = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    AlternateText = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    NavigateUrl = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ImageUrl = table.Column<byte[]>(type: "BLOB", nullable: true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Hits = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdsBs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdsPBs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Url = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    AlternateText = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    NavigateUrl = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ImageUrl = table.Column<byte[]>(type: "BLOB", nullable: true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Hits = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdsPBs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    TableName = table.Column<string>(type: "TEXT", nullable: true),
                    DateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OldValues = table.Column<string>(type: "TEXT", nullable: true),
                    NewValues = table.Column<string>(type: "TEXT", nullable: true),
                    AffectedColumns = table.Column<string>(type: "TEXT", nullable: true),
                    PrimaryKey = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlogCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Category = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExtrasLicenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    License = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtrasLicenses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HomeCarousels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CarouselImage = table.Column<byte[]>(type: "BLOB", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeCarousels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductLicenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    License = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductLicenses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Tag = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Socials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SMName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Url = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    SMImage = table.Column<byte[]>(type: "BLOB", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Socials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SoftwareTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftwareTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subscribers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscribers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    StageName = table.Column<string>(type: "TEXT", nullable: true),
                    Bio = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Designation = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Url = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    UserImage = table.Column<byte[]>(type: "BLOB", nullable: true),
                    IsArtiste = table.Column<bool>(type: "INTEGER", nullable: true),
                    IsTeam = table.Column<bool>(type: "INTEGER", nullable: true),
                    SocialUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Beats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    StreamPlatformUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    ReleaseDate = table.Column<DateTime>(type: "TEXT", maxLength: 50, nullable: false),
                    UploadDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Image = table.Column<byte[]>(type: "BLOB", nullable: true),
                    Url = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    DownloadCount = table.Column<int>(type: "INTEGER", nullable: false),
                    AudioPath = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    ContentType = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Data = table.Column<byte[]>(type: "BLOB", nullable: true),
                    Price = table.Column<double>(type: "REAL", nullable: true),
                    ExternalProducer = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    PurchaseURL = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GenreId = table.Column<int>(type: "INTEGER", nullable: false),
                    ExtrasLicenseId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProducerId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Beats_ExtrasLicenses_ExtrasLicenseId",
                        column: x => x.ExtrasLicenseId,
                        principalTable: "ExtrasLicenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Beats_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Beats_User_ProducerId",
                        column: x => x.ProducerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BlogItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Article = table.Column<string>(type: "TEXT", maxLength: 10000, nullable: false),
                    PublishDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PostImage = table.Column<byte[]>(type: "BLOB", nullable: true),
                    BlogItemUrl = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Views = table.Column<int>(type: "INTEGER", nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    AuthorId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogItems_BlogCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "BlogCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogItems_User_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ArtisteName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "TEXT", maxLength: 50, nullable: false),
                    UploadDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ProductImage = table.Column<byte[]>(type: "BLOB", nullable: true),
                    StreamPlatformUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Fanlink = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Url = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    DownloadCount = table.Column<int>(type: "INTEGER", nullable: false),
                    AudioPath = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    ContentType = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Data = table.Column<byte[]>(type: "BLOB", nullable: true),
                    Producer = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Featuring = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Writer = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    TrackNumber = table.Column<int>(type: "INTEGER", nullable: true),
                    AlbumTrack = table.Column<bool>(type: "INTEGER", nullable: false),
                    EPTrack = table.Column<bool>(type: "INTEGER", nullable: false),
                    Uploader = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    GenreId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProductTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductLicenseId = table.Column<int>(type: "INTEGER", nullable: false),
                    AlbumorEPId = table.Column<int>(type: "INTEGER", nullable: true),
                    ArtisteId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_ProductLicenses_ProductLicenseId",
                        column: x => x.ProductLicenseId,
                        principalTable: "ProductLicenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Products_AlbumorEPId",
                        column: x => x.AlbumorEPId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_ProductTypes_ProductTypeId",
                        column: x => x.ProductTypeId,
                        principalTable: "ProductTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_User_ArtisteId",
                        column: x => x.ArtisteId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Softwares",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "TEXT", maxLength: 50, nullable: false),
                    UploadDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Image = table.Column<byte[]>(type: "BLOB", nullable: true),
                    Url = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    DownloadCount = table.Column<int>(type: "INTEGER", nullable: false),
                    FilePath = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    ContentType = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Data = table.Column<byte[]>(type: "BLOB", nullable: true),
                    Price = table.Column<double>(type: "REAL", nullable: true),
                    ExternalDeveloper = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    PurchaseURL = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    DownloadURL = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Uploader = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    ExtrasLicenseId = table.Column<int>(type: "INTEGER", nullable: false),
                    SoftwareTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeveloperId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Softwares", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Softwares_ExtrasLicenses_ExtrasLicenseId",
                        column: x => x.ExtrasLicenseId,
                        principalTable: "ExtrasLicenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Softwares_SoftwareTypes_SoftwareTypeId",
                        column: x => x.SoftwareTypeId,
                        principalTable: "SoftwareTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Softwares_User_DeveloperId",
                        column: x => x.DeveloperId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Downloads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DownloadTitle = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Url = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    DownloadToken = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Hits = table.Column<int>(type: "INTEGER", nullable: true),
                    ExpireAfterDownload = table.Column<bool>(type: "INTEGER", nullable: false),
                    Downloaded = table.Column<bool>(type: "INTEGER", nullable: false),
                    BeatId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Downloads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Downloads_Beats_BeatId",
                        column: x => x.BeatId,
                        principalTable: "Beats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Beats_ExtrasLicenseId",
                table: "Beats",
                column: "ExtrasLicenseId");

            migrationBuilder.CreateIndex(
                name: "IX_Beats_GenreId",
                table: "Beats",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Beats_ProducerId",
                table: "Beats",
                column: "ProducerId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogItems_AuthorId",
                table: "BlogItems",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogItems_CategoryId",
                table: "BlogItems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Downloads_BeatId",
                table: "Downloads",
                column: "BeatId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_AlbumorEPId",
                table: "Products",
                column: "AlbumorEPId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ArtisteId",
                table: "Products",
                column: "ArtisteId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_GenreId",
                table: "Products",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductLicenseId",
                table: "Products",
                column: "ProductLicenseId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductTypeId",
                table: "Products",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Role",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Softwares_DeveloperId",
                table: "Softwares",
                column: "DeveloperId");

            migrationBuilder.CreateIndex(
                name: "IX_Softwares_ExtrasLicenseId",
                table: "Softwares",
                column: "ExtrasLicenseId");

            migrationBuilder.CreateIndex(
                name: "IX_Softwares_SoftwareTypeId",
                table: "Softwares",
                column: "SoftwareTypeId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "User",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "User",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdsBs");

            migrationBuilder.DropTable(
                name: "AdsPBs");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "BlogItems");

            migrationBuilder.DropTable(
                name: "Downloads");

            migrationBuilder.DropTable(
                name: "HomeCarousels");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "Socials");

            migrationBuilder.DropTable(
                name: "Softwares");

            migrationBuilder.DropTable(
                name: "Subscribers");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "BlogCategories");

            migrationBuilder.DropTable(
                name: "Beats");

            migrationBuilder.DropTable(
                name: "ProductLicenses");

            migrationBuilder.DropTable(
                name: "ProductTypes");

            migrationBuilder.DropTable(
                name: "SoftwareTypes");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "ExtrasLicenses");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
