﻿// <auto-generated />
using System;
using BillingApplication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BillingApplication.Migrations
{
    [DbContext(typeof(BillingAppDbContext))]
    [Migration("20250111102820_value_generate_on_add_ALL")]
    partial class value_generate_on_add_ALL
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BillingApplication.DataLayer.Entities.BundleEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<TimeSpan>("CallTIme")
                        .HasColumnType("interval");

                    b.Property<long>("Internet")
                        .HasColumnType("bigint");

                    b.Property<int>("Messages")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Bundles");
                });

            modelBuilder.Entity("BillingApplication.DataLayer.Entities.ExtrasEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Package")
                        .HasColumnType("integer");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Package");

                    b.ToTable("Extras");
                });

            modelBuilder.Entity("BillingApplication.Entities.CallsEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Duration")
                        .HasColumnType("integer");

                    b.Property<int>("FromSubscriberId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<string>("ToPhoneNumber")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("FromSubscriberId");

                    b.ToTable("Calls");
                });

            modelBuilder.Entity("BillingApplication.Entities.MessagesEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("FromPhoneId")
                        .HasColumnType("integer");

                    b.Property<string>("MessageText")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<string>("ToPhoneNumber")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("FromPhoneId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("BillingApplication.Entities.OperatorEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("boolean");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Operators");
                });

            modelBuilder.Entity("BillingApplication.Entities.OwnerChangeEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("LastUserId")
                        .HasColumnType("integer");

                    b.Property<int>("NewUserId")
                        .HasColumnType("integer");

                    b.Property<int>("PhoneId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("LastUserId");

                    b.HasIndex("NewUserId");

                    b.HasIndex("PhoneId");

                    b.ToTable("OwnerChanges");
                });

            modelBuilder.Entity("BillingApplication.Entities.PassportInfoEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<DateTime?>("ExpiryDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FullName")
                        .HasColumnType("text");

                    b.Property<DateTime?>("IssueDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("IssuedBy")
                        .HasColumnType("text");

                    b.Property<string>("PassportNumber")
                        .HasColumnType("text");

                    b.Property<string>("Registration")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("PassportInfos");
                });

            modelBuilder.Entity("BillingApplication.Entities.PaymentEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PhoneId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PhoneId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("BillingApplication.Entities.SubscriberEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<decimal>("Balance")
                        .HasColumnType("numeric")
                        .HasColumnName("balance");

                    b.Property<TimeSpan>("CallTime")
                        .HasColumnType("interval")
                        .HasColumnName("call_time");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("creation_date");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<long>("InternetAmount")
                        .HasColumnType("bigint")
                        .HasColumnName("internet");

                    b.Property<int>("MessagesCount")
                        .HasColumnType("integer")
                        .HasColumnName("messages");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("phone_number");

                    b.Property<int?>("PassportId")
                        .HasColumnType("integer")
                        .HasColumnName("passport_id");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("payment_date");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("salt");

                    b.Property<int?>("TariffId")
                        .HasColumnType("integer")
                        .HasColumnName("tariff_id");

                    b.HasKey("Id");

                    b.HasIndex("PassportId");

                    b.HasIndex("TariffId");

                    b.ToTable("Subscribers");
                });

            modelBuilder.Entity("BillingApplication.Entities.TariffChangeEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("LastTariffId")
                        .HasColumnType("integer");

                    b.Property<int?>("NewTariffId")
                        .HasColumnType("integer");

                    b.Property<int>("PhoneId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("LastTariffId");

                    b.HasIndex("NewTariffId");

                    b.HasIndex("PhoneId");

                    b.ToTable("TariffChanges");
                });

            modelBuilder.Entity("BillingApplication.Entities.TariffEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("TariffPlan")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("TariffPlan");

                    b.ToTable("Tariffs");
                });

            modelBuilder.Entity("BillingApplication.Entities.TopUpsEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("PhoneId")
                        .HasColumnType("integer");

                    b.Property<string>("SenderInfo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PhoneId");

                    b.ToTable("TopUps");
                });

            modelBuilder.Entity("BillingApplication.Server.DataLayer.Entities.InternetEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("PhoneId")
                        .HasColumnType("integer");

                    b.Property<int>("Price")
                        .HasColumnType("integer");

                    b.Property<long>("SpentInternet")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("PhoneId");

                    b.ToTable("Internet");
                });

            modelBuilder.Entity("BillingApplication.DataLayer.Entities.ExtrasEntity", b =>
                {
                    b.HasOne("BillingApplication.DataLayer.Entities.BundleEntity", "Bundle")
                        .WithMany("Extras")
                        .HasForeignKey("Package")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bundle");
                });

            modelBuilder.Entity("BillingApplication.Entities.CallsEntity", b =>
                {
                    b.HasOne("BillingApplication.Entities.SubscriberEntity", "Subscriber")
                        .WithMany("Calls")
                        .HasForeignKey("FromSubscriberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subscriber");
                });

            modelBuilder.Entity("BillingApplication.Entities.MessagesEntity", b =>
                {
                    b.HasOne("BillingApplication.Entities.SubscriberEntity", "Subscriber")
                        .WithMany("Messages")
                        .HasForeignKey("FromPhoneId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subscriber");
                });

            modelBuilder.Entity("BillingApplication.Entities.OwnerChangeEntity", b =>
                {
                    b.HasOne("BillingApplication.Entities.PassportInfoEntity", "LastUser")
                        .WithMany()
                        .HasForeignKey("LastUserId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("BillingApplication.Entities.PassportInfoEntity", "NewUser")
                        .WithMany()
                        .HasForeignKey("NewUserId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("BillingApplication.Entities.SubscriberEntity", "Subscriber")
                        .WithMany("OwnerChanges")
                        .HasForeignKey("PhoneId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LastUser");

                    b.Navigation("NewUser");

                    b.Navigation("Subscriber");
                });

            modelBuilder.Entity("BillingApplication.Entities.PaymentEntity", b =>
                {
                    b.HasOne("BillingApplication.Entities.SubscriberEntity", "Subscriber")
                        .WithMany("Payments")
                        .HasForeignKey("PhoneId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Subscriber");
                });

            modelBuilder.Entity("BillingApplication.Entities.SubscriberEntity", b =>
                {
                    b.HasOne("BillingApplication.Entities.PassportInfoEntity", "PassportInfo")
                        .WithMany("Subscribers")
                        .HasForeignKey("PassportId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("BillingApplication.Entities.TariffEntity", "Tariff")
                        .WithMany("Subscribers")
                        .HasForeignKey("TariffId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("PassportInfo");

                    b.Navigation("Tariff");
                });

            modelBuilder.Entity("BillingApplication.Entities.TariffChangeEntity", b =>
                {
                    b.HasOne("BillingApplication.Entities.TariffEntity", "LastTariff")
                        .WithMany()
                        .HasForeignKey("LastTariffId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("BillingApplication.Entities.TariffEntity", "NewTariff")
                        .WithMany()
                        .HasForeignKey("NewTariffId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("BillingApplication.Entities.SubscriberEntity", "Subscriber")
                        .WithMany("TariffChanges")
                        .HasForeignKey("PhoneId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("LastTariff");

                    b.Navigation("NewTariff");

                    b.Navigation("Subscriber");
                });

            modelBuilder.Entity("BillingApplication.Entities.TariffEntity", b =>
                {
                    b.HasOne("BillingApplication.DataLayer.Entities.BundleEntity", "Bundle")
                        .WithMany("Tariffs")
                        .HasForeignKey("TariffPlan")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bundle");
                });

            modelBuilder.Entity("BillingApplication.Entities.TopUpsEntity", b =>
                {
                    b.HasOne("BillingApplication.Entities.SubscriberEntity", "Subscriber")
                        .WithMany("TopUps")
                        .HasForeignKey("PhoneId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Subscriber");
                });

            modelBuilder.Entity("BillingApplication.Server.DataLayer.Entities.InternetEntity", b =>
                {
                    b.HasOne("BillingApplication.Entities.SubscriberEntity", "Subscriber")
                        .WithMany("Internet")
                        .HasForeignKey("PhoneId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subscriber");
                });

            modelBuilder.Entity("BillingApplication.DataLayer.Entities.BundleEntity", b =>
                {
                    b.Navigation("Extras");

                    b.Navigation("Tariffs");
                });

            modelBuilder.Entity("BillingApplication.Entities.PassportInfoEntity", b =>
                {
                    b.Navigation("Subscribers");
                });

            modelBuilder.Entity("BillingApplication.Entities.SubscriberEntity", b =>
                {
                    b.Navigation("Calls");

                    b.Navigation("Internet");

                    b.Navigation("Messages");

                    b.Navigation("OwnerChanges");

                    b.Navigation("Payments");

                    b.Navigation("TariffChanges");

                    b.Navigation("TopUps");
                });

            modelBuilder.Entity("BillingApplication.Entities.TariffEntity", b =>
                {
                    b.Navigation("Subscribers");
                });
#pragma warning restore 612, 618
        }
    }
}
