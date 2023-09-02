using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PandaSearch.Server.Models;
using PandaSearch.Shared;
using Radzen.Blazor.Rendering;

namespace PandaSearch.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Primary Keys
            modelBuilder.Entity<Product>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<Brand>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<Image>()
                .HasKey(p => p.Id);

            //Foreign Key

           // modelBuilder.Entity<Product>()
           //.HasOne(p => p.Brand)
           //.WithMany(c => c.Product)
           //.HasForeignKey(p => p.BrandId);

            //modelBuilder.Entity<Image>()
            //.HasOne(p => p.Product)
            //.WithMany(c => c.Images)
            //.HasForeignKey(p => p.ProductId);


        }
        public DbSet<Brand> Brands => Set<Brand>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Image> Images => Set<Image>();

    }
}