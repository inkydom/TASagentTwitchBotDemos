﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TASagentTwitchBot.NoOverlaysDemo.Database;

#nullable disable

namespace TASagentTwitchBot.NoOverlaysDemo.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.1");

            modelBuilder.Entity("TASagentTwitchBot.Core.Database.CustomTextCommand", b =>
                {
                    b.Property<int>("CustomTextCommandId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Command")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Enabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("CustomTextCommandId");

                    b.ToTable("CustomTextCommands");
                });

            modelBuilder.Entity("TASagentTwitchBot.Core.Database.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AuthorizationLevel")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Color")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("FirstFollowed")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("FirstSeen")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LastSuccessfulTTS")
                        .HasColumnType("TEXT");

                    b.Property<string>("TTSEffectsChain")
                        .HasColumnType("TEXT");

                    b.Property<int>("TTSPitchPreference")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TTSSpeedPreference")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TTSVoicePreference")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TwitchUserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TwitchUserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TASagentTwitchBot.Plugin.Quotes.Quote", b =>
                {
                    b.Property<int>("QuoteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("CreatorId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FakeNewsExplanation")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsFakeNews")
                        .HasColumnType("INTEGER");

                    b.Property<string>("QuoteText")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Speaker")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("QuoteId");

                    b.HasIndex("CreatorId");

                    b.ToTable("Quotes");
                });

            modelBuilder.Entity("TASagentTwitchBot.Plugin.Quotes.Quote", b =>
                {
                    b.HasOne("TASagentTwitchBot.Core.Database.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");
                });
#pragma warning restore 612, 618
        }
    }
}
