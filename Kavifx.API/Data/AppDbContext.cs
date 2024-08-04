using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kavifx.API.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole,int>
    {
        private readonly IConfiguration _config;
        private readonly string _conString;
        public AppDbContext(IConfiguration configuration)
        {
            _config = configuration;
            var host = _config["mysql:DBHOST"] ?? "localhost";
            var port = _config["mysql:PORT"] ?? "3306";
            var password = _config["mysql:PASSWORD"] ?? "password";
            var database = _config["mysql:DATABASE"] ?? "AppDB";

            _conString = $"server={host};userid=root;pwd={password};port:{port};database={database}";
        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }                
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(_conString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
