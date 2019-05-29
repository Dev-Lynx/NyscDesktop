﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NyscIdentify.Common.Infrastructure.Data;

namespace NyscIdentify.Common.Infrastructure.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity("NyscIdentify.Common.Infrastructure.Models.Entities.ResourceBase", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateAdded");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("LocalPath");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.ToTable("Resources");

                    b.HasDiscriminator<string>("Discriminator").HasValue("ResourceBase");
                });

            modelBuilder.Entity("NyscIdentify.Common.Infrastructure.Models.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccessToken");

                    b.Property<int>("ApprovalStatus");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("Department");

                    b.Property<string>("DisplayName");

                    b.Property<int>("FileNo");

                    b.Property<string>("Gender");

                    b.Property<string>("LastName");

                    b.Property<string>("Location");

                    b.Property<string>("OtherNames");

                    b.Property<string>("Password");

                    b.Property<string>("PhoneNumber");

                    b.Property<string>("Qualification");

                    b.Property<string>("Rank");

                    b.Property<string>("Role")
                        .IsRequired();

                    b.Property<string>("StateOfOrigin");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("NyscIdentify.Common.Infrastructure.Models.Entities.Photo", b =>
                {
                    b.HasBaseType("NyscIdentify.Common.Infrastructure.Models.Entities.ResourceBase");

                    b.Property<int>("Type");

                    b.Property<string>("UserId");

                    b.HasIndex("UserId");

                    b.HasDiscriminator().HasValue("Photo");
                });

            modelBuilder.Entity("NyscIdentify.Common.Infrastructure.Models.Entities.Photo", b =>
                {
                    b.HasOne("NyscIdentify.Common.Infrastructure.Models.Entities.User", "User")
                        .WithMany("Photos")
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
