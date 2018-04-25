using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Data.Entity;

namespace VVC_Ng4_NetApi.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
        public string Name { get; set; }

        public virtual Card Card { get; set; }

        public virtual ICollection<Organization> Admins { get; set; }

        public virtual ICollection<CardRequest> FromUsers { get; set; }
        public virtual ICollection<CardRequest> ToUsers { get; set; }

        public virtual ICollection<Wallet> Owners { get; set; }
        public virtual ICollection<Wallet> Cards { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<CardRequest> CardRequests { get; set; }
        public DbSet<Wallet> Wallets { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //----Organization admin id with Applicationuser
            modelBuilder.Entity<Organization>()
               .HasOptional(x => x.Admin)
               .WithMany(x => x.Admins)
               .HasForeignKey(x => x.AdminId)
               .WillCascadeOnDelete(false);


            //------Card Request One to Many relationship with ApplicationUser

            modelBuilder.Entity<CardRequest>()
               .HasRequired(x => x.FromUser)
               .WithMany(x => x.FromUsers)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<CardRequest>()
               .HasRequired(x => x.ToUser)
               .WithMany(x => x.ToUsers)
               .WillCascadeOnDelete(false);

            //--------Wallet one to many realationship with ApplicationUser

            modelBuilder.Entity<Wallet>()
               .HasRequired(x => x.Owner)
               .WithMany(x => x.Owners)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Wallet>()
               .HasRequired(x => x.Card)
               .WithMany(x => x.Cards)
               .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);

        }

    }
}