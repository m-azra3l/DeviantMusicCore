using System;
using System.Collections.Generic;
using DeviantMusicCore.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DeviantMusicCore.Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;

namespace DeviantMusicCore.Data
{
    public class DeviantContext : IdentityDbContext<IdentityUser>
    {
        public  DeviantContext(DbContextOptions<DeviantContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityUser>(entity =>
            {
                entity.ToTable(name: "User");
            });

            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");

            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });
        }
        public DbSet<AdsB> AdsBs { get; set; }
        public DbSet<AdsPB> AdsPBs { get; set; }
        public DbSet<Audit> AuditLogs { get; set; }
        public DbSet<Beat> Beats { get; set; }
        public DbSet<BlogItem> BlogItems { get; set; }
        public DbSet<BlogCategory> BlogCategories { get; set; }
        public DbSet<Download> Downloads { get; set; }
        public DbSet<ExtrasLicense> ExtrasLicenses { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Product> Products { get; set; }        
        public DbSet<ProductLicense> ProductLicenses { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Software> Softwares { get; set; }
        public DbSet<SoftwareType> SoftwareTypes { get; set; }
        public DbSet<Social> Socials { get; set; }
        public DbSet<HomeCarousel> HomeCarousels { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }

        public virtual async Task<int> SaveChangesAsync(string userName = null)
        {
            OnBeforeSaveChanges(userName);
            var result = await base.SaveChangesAsync();
            return result;
        }

        private void OnBeforeSaveChanges(string userName)
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;
                var auditEntry = new AuditEntry(entry);
                auditEntry.TableName = entry.Entity.GetType().Name;
                auditEntry.UserName = userName;
                auditEntries.Add(auditEntry);
                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = AuditType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            auditEntry.AuditType = AuditType.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.ChangedColumns.Add(propertyName);
                                auditEntry.AuditType = AuditType.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }
            foreach (var auditEntry in auditEntries)
            {
                AuditLogs.Add(auditEntry.ToAudit());
            }
        }
    }
}