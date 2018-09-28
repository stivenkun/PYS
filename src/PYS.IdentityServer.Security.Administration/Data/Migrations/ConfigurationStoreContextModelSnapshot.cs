﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PYS.IdentityServer.Security.Administration.ConfigurationStore;

namespace PYS.IdentityServer.Security.Administration.Data.Migrations
{
    [DbContext(typeof(ConfigurationStoreContext))]
    partial class ConfigurationStoreContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AccessData.Models.Aplication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("IconSrc");

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Url")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("AccessData.Models.AppClaims", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("StudentId");

                    b.Property<string>("Value")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("StudentId");

                    b.ToTable("AppClaims");
                });

            modelBuilder.Entity("PYS.IdentityServer.Security.Administration.ConfigurationStore.ApiResourceEntity", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("DisplayName");

                    b.Property<bool>("Enabled");

                    b.HasKey("Name");

                    b.ToTable("ApiResources");
                });

            modelBuilder.Entity("PYS.IdentityServer.Security.Administration.ConfigurationStore.ClientEntity", b =>
                {
                    b.Property<string>("ClientId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AbsoluteRefreshTokenLifetime");

                    b.Property<int>("AccessTokenLifetime");

                    b.Property<int>("AccessTokenType");

                    b.Property<bool>("AllowAccessTokensViaBrowser");

                    b.Property<bool>("AllowOfflineAccess");

                    b.Property<bool>("AllowPlainTextPkce");

                    b.Property<bool>("AllowRememberConsent");

                    b.Property<bool>("AlwaysIncludeUserClaimsInIdToken");

                    b.Property<bool>("AlwaysSendClientClaims");

                    b.Property<int>("AuthorizationCodeLifetime");

                    b.Property<bool>("BackChannelLogoutSessionRequired");

                    b.Property<string>("BackChannelLogoutUri");

                    b.Property<string>("ClientClaimsPrefix");

                    b.Property<string>("ClientName");

                    b.Property<string>("ClientUri");

                    b.Property<int?>("ConsentLifetime");

                    b.Property<bool>("EnableLocalLogin");

                    b.Property<bool>("Enabled");

                    b.Property<bool>("FrontChannelLogoutSessionRequired");

                    b.Property<string>("FrontChannelLogoutUri");

                    b.Property<int>("IdentityTokenLifetime");

                    b.Property<bool>("IncludeJwtId");

                    b.Property<string>("LogoUri");

                    b.Property<string>("PairWiseSubjectSalt");

                    b.Property<string>("ProtocolType");

                    b.Property<int>("RefreshTokenExpiration");

                    b.Property<int>("RefreshTokenUsage");

                    b.Property<bool>("RequireClientSecret");

                    b.Property<bool>("RequireConsent");

                    b.Property<bool>("RequirePkce");

                    b.Property<int>("SlidingRefreshTokenLifetime");

                    b.Property<bool>("UpdateAccessTokenClaimsOnRefresh");

                    b.HasKey("ClientId");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("PYS.IdentityServer.Security.Administration.ConfigurationStore.IdentityResourceEntity", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("DisplayName");

                    b.Property<bool>("Emphasize");

                    b.Property<bool>("Enabled");

                    b.Property<bool>("Required");

                    b.Property<bool>("ShowInDiscoveryDocument");

                    b.HasKey("Name");

                    b.ToTable("IdentityResources");
                });

            modelBuilder.Entity("AccessData.Models.AppClaims", b =>
                {
                    b.HasOne("AccessData.Models.Aplication", "Aplication")
                        .WithMany("AppClaims")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}