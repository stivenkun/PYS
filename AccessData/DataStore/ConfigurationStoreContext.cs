using AccessData.Models;
using AccessData.Models.ClientStoreEnitity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccessData.DataStore
{
    public class ConfigurationStoreContext : DbContext
    {
        public ConfigurationStoreContext(DbContextOptions<ConfigurationStoreContext> options) : base(options)
        { }

        public DbSet<ClientEntity> Clients { get; set; }
        public DbSet<ApiResourceEntity> ApiResources { get; set; }
        public DbSet<IdentityResourceEntity> IdentityResources { get; set; }
        public DbSet<Aplication> Applications { set; get; }
        public DbSet<AppClaims> AppClaims { set; get; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ClientEntity>().HasKey(m => m.ClientId);
            builder.Entity<ApiResourceEntity>().HasKey(m => m.Name);
            builder.Entity<IdentityResourceEntity>().HasKey(m => m.Name);
            builder.Entity<Aplication>().HasKey(m => m.Id);
            builder.Entity<AppClaims>().HasKey(m => m.Id);
            base.OnModelCreating(builder);
        }
    }
}
