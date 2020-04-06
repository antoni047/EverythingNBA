﻿// <auto-generated />
using System;
using EverythingNBA.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EverythingNBA.Data.Migrations
{
    [DbContext(typeof(EverythingNBADbContext))]
    partial class EverythingNBADbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EverythingNBA.Models.AllStarTeam", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("SeasonId")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SeasonId");

                    b.ToTable("AllStarTeams");
                });

            modelBuilder.Entity("EverythingNBA.Models.Award", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Name")
                        .HasColumnType("int");

                    b.Property<int?>("SeasonId")
                        .HasColumnType("int");

                    b.Property<int?>("WinnerId")
                        .HasColumnType("int");

                    b.Property<string>("WinnerTeamName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SeasonId");

                    b.HasIndex("WinnerId");

                    b.ToTable("Awards");
                });

            modelBuilder.Entity("EverythingNBA.Models.CloudinaryImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ImagePublicId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageThumbnailURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Length")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("CloudinaryImages");
                });

            modelBuilder.Entity("EverythingNBA.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsFinished")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPlayoffGame")
                        .HasColumnType("bit");

                    b.Property<int?>("SeasonId")
                        .HasColumnType("int");

                    b.Property<int?>("Team2Id")
                        .HasColumnType("int");

                    b.Property<int?>("Team2Points")
                        .HasColumnType("int");

                    b.Property<int?>("TeamHostId")
                        .HasColumnType("int");

                    b.Property<int?>("TeamHostPoints")
                        .HasColumnType("int");

                    b.Property<int?>("TeamId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SeasonId");

                    b.HasIndex("Team2Id");

                    b.HasIndex("TeamHostId");

                    b.HasIndex("TeamId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("EverythingNBA.Models.GameStatistic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Assists")
                        .HasColumnType("float");

                    b.Property<double>("Blocks")
                        .HasColumnType("float");

                    b.Property<double>("FieldGoalAttempts")
                        .HasColumnType("float");

                    b.Property<double>("FieldGoalsMade")
                        .HasColumnType("float");

                    b.Property<double>("FreeThrowAttempts")
                        .HasColumnType("float");

                    b.Property<double>("FreeThrowsMade")
                        .HasColumnType("float");

                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.Property<int>("MinutesPlayed")
                        .HasColumnType("int");

                    b.Property<int>("PlayerId")
                        .HasColumnType("int");

                    b.Property<double>("Points")
                        .HasColumnType("float");

                    b.Property<double>("Rebounds")
                        .HasColumnType("float");

                    b.Property<double>("Steals")
                        .HasColumnType("float");

                    b.Property<double>("ThreeAttempts")
                        .HasColumnType("float");

                    b.Property<double>("ThreeMade")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("PlayerId");

                    b.ToTable("GameStatistics");
                });

            modelBuilder.Entity("EverythingNBA.Models.MappingTables.AllStarTeamsPlayers", b =>
                {
                    b.Property<int>("AllStarTeamId")
                        .HasColumnType("int");

                    b.Property<int>("PlayerId")
                        .HasColumnType("int");

                    b.HasKey("AllStarTeamId", "PlayerId");

                    b.HasIndex("PlayerId");

                    b.ToTable("AllStarTeamsPlayers");
                });

            modelBuilder.Entity("EverythingNBA.Models.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<int?>("CloudinaryImageId")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<string>("InstagramLink")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsStarter")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<int>("Position")
                        .HasColumnType("int");

                    b.Property<int?>("RookieYear")
                        .HasColumnType("int");

                    b.Property<int>("ShirtNumber")
                        .HasColumnType("int");

                    b.Property<int?>("TeamId")
                        .HasColumnType("int");

                    b.Property<string>("TwitterLink")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Weight")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CloudinaryImageId");

                    b.HasIndex("TeamId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("EverythingNBA.Models.Playoff", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("AreConferenceFinalsFinished")
                        .HasColumnType("bit");

                    b.Property<bool>("AreQuarterFinalsFinished")
                        .HasColumnType("bit");

                    b.Property<bool>("AreSemiFinalsFinished")
                        .HasColumnType("bit");

                    b.Property<int?>("SeasonId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SeasonId")
                        .IsUnique()
                        .HasFilter("[SeasonId] IS NOT NULL");

                    b.ToTable("Playoffs");
                });

            modelBuilder.Entity("EverythingNBA.Models.Season", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GamesPlayed")
                        .HasColumnType("int");

                    b.Property<int?>("PlayoffId")
                        .HasColumnType("int");

                    b.Property<DateTime>("SeasonEndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("SeasonStartDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("TitleWinnerId")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TitleWinnerId");

                    b.ToTable("Seasons");
                });

            modelBuilder.Entity("EverythingNBA.Models.SeasonStatistic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Losses")
                        .HasColumnType("int");

                    b.Property<int?>("SeasonId")
                        .HasColumnType("int");

                    b.Property<int>("TeamId")
                        .HasColumnType("int");

                    b.Property<int>("Wins")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SeasonId");

                    b.HasIndex("TeamId");

                    b.ToTable("SeasonStatistics");
                });

            modelBuilder.Entity("EverythingNBA.Models.Series", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Conference")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Game1Id")
                        .HasColumnType("int");

                    b.Property<int?>("Game2Id")
                        .HasColumnType("int");

                    b.Property<int?>("Game3Id")
                        .HasColumnType("int");

                    b.Property<int?>("Game4Id")
                        .HasColumnType("int");

                    b.Property<int?>("Game5Id")
                        .HasColumnType("int");

                    b.Property<int?>("Game6Id")
                        .HasColumnType("int");

                    b.Property<int?>("Game7Id")
                        .HasColumnType("int");

                    b.Property<int>("PlayoffId")
                        .HasColumnType("int");

                    b.Property<string>("Stage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StageNumber")
                        .HasColumnType("int");

                    b.Property<int?>("Team1GamesWon")
                        .HasColumnType("int");

                    b.Property<int?>("Team1Id")
                        .HasColumnType("int");

                    b.Property<int>("Team1StandingsPosition")
                        .HasColumnType("int");

                    b.Property<int?>("Team2GamesWon")
                        .HasColumnType("int");

                    b.Property<int?>("Team2Id")
                        .HasColumnType("int");

                    b.Property<int>("Team2StandingsPosition")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Game1Id");

                    b.HasIndex("Game2Id");

                    b.HasIndex("Game3Id");

                    b.HasIndex("Game4Id");

                    b.HasIndex("Game5Id");

                    b.HasIndex("Game6Id");

                    b.HasIndex("Game7Id");

                    b.HasIndex("PlayoffId");

                    b.HasIndex("Team1Id");

                    b.HasIndex("Team2Id");

                    b.ToTable("Series");
                });

            modelBuilder.Entity("EverythingNBA.Models.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AbbreviatedName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Conference")
                        .HasColumnType("int");

                    b.Property<int?>("FullImageId")
                        .HasColumnType("int");

                    b.Property<string>("Instagram")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<int?>("SmallImageId")
                        .HasColumnType("int");

                    b.Property<string>("Twitter")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Venue")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FullImageId");

                    b.HasIndex("SmallImageId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("EverythingNBA.Models.AllStarTeam", b =>
                {
                    b.HasOne("EverythingNBA.Models.Season", null)
                        .WithMany("AllStarTeams")
                        .HasForeignKey("SeasonId");
                });

            modelBuilder.Entity("EverythingNBA.Models.Award", b =>
                {
                    b.HasOne("EverythingNBA.Models.Season", "Season")
                        .WithMany("Awards")
                        .HasForeignKey("SeasonId");

                    b.HasOne("EverythingNBA.Models.Player", "Winner")
                        .WithMany("Awards")
                        .HasForeignKey("WinnerId");
                });

            modelBuilder.Entity("EverythingNBA.Models.Game", b =>
                {
                    b.HasOne("EverythingNBA.Models.Season", "Season")
                        .WithMany("Games")
                        .HasForeignKey("SeasonId");

                    b.HasOne("EverythingNBA.Models.Team", "Team2")
                        .WithMany("AwayGames")
                        .HasForeignKey("Team2Id");

                    b.HasOne("EverythingNBA.Models.Team", "TeamHost")
                        .WithMany("HomeGames")
                        .HasForeignKey("TeamHostId");

                    b.HasOne("EverythingNBA.Models.Team", null)
                        .WithMany("GamesWon")
                        .HasForeignKey("TeamId");
                });

            modelBuilder.Entity("EverythingNBA.Models.GameStatistic", b =>
                {
                    b.HasOne("EverythingNBA.Models.Game", "Game")
                        .WithMany("PlayerStats")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EverythingNBA.Models.Player", "Player")
                        .WithMany("SingleGameStatistics")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EverythingNBA.Models.MappingTables.AllStarTeamsPlayers", b =>
                {
                    b.HasOne("EverythingNBA.Models.AllStarTeam", "AllStarTeam")
                        .WithMany("Players")
                        .HasForeignKey("AllStarTeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EverythingNBA.Models.Player", "Player")
                        .WithMany("AllStarTeams")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EverythingNBA.Models.Player", b =>
                {
                    b.HasOne("EverythingNBA.Models.CloudinaryImage", "CloudinaryImage")
                        .WithMany()
                        .HasForeignKey("CloudinaryImageId");

                    b.HasOne("EverythingNBA.Models.Team", "Team")
                        .WithMany("Players")
                        .HasForeignKey("TeamId");
                });

            modelBuilder.Entity("EverythingNBA.Models.Playoff", b =>
                {
                    b.HasOne("EverythingNBA.Models.Season", "Season")
                        .WithOne("Playoff")
                        .HasForeignKey("EverythingNBA.Models.Playoff", "SeasonId");
                });

            modelBuilder.Entity("EverythingNBA.Models.Season", b =>
                {
                    b.HasOne("EverythingNBA.Models.Team", "TitleWinner")
                        .WithMany("TitlesWon")
                        .HasForeignKey("TitleWinnerId");
                });

            modelBuilder.Entity("EverythingNBA.Models.SeasonStatistic", b =>
                {
                    b.HasOne("EverythingNBA.Models.Season", "Season")
                        .WithMany("SeasonStatistics")
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("EverythingNBA.Models.Team", "Team")
                        .WithMany("SeasonsStatistics")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EverythingNBA.Models.Series", b =>
                {
                    b.HasOne("EverythingNBA.Models.Game", "Game1")
                        .WithMany()
                        .HasForeignKey("Game1Id");

                    b.HasOne("EverythingNBA.Models.Game", "Game2")
                        .WithMany()
                        .HasForeignKey("Game2Id");

                    b.HasOne("EverythingNBA.Models.Game", "Game3")
                        .WithMany()
                        .HasForeignKey("Game3Id");

                    b.HasOne("EverythingNBA.Models.Game", "Game4")
                        .WithMany()
                        .HasForeignKey("Game4Id");

                    b.HasOne("EverythingNBA.Models.Game", "Game5")
                        .WithMany()
                        .HasForeignKey("Game5Id");

                    b.HasOne("EverythingNBA.Models.Game", "Game6")
                        .WithMany()
                        .HasForeignKey("Game6Id");

                    b.HasOne("EverythingNBA.Models.Game", "Game7")
                        .WithMany()
                        .HasForeignKey("Game7Id");

                    b.HasOne("EverythingNBA.Models.Playoff", "Playoff")
                        .WithMany("Series")
                        .HasForeignKey("PlayoffId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EverythingNBA.Models.Team", "Team1")
                        .WithMany()
                        .HasForeignKey("Team1Id");

                    b.HasOne("EverythingNBA.Models.Team", "Team2")
                        .WithMany()
                        .HasForeignKey("Team2Id");
                });

            modelBuilder.Entity("EverythingNBA.Models.Team", b =>
                {
                    b.HasOne("EverythingNBA.Models.CloudinaryImage", "FullImage")
                        .WithMany()
                        .HasForeignKey("FullImageId");

                    b.HasOne("EverythingNBA.Models.CloudinaryImage", "SmallImage")
                        .WithMany()
                        .HasForeignKey("SmallImageId");
                });
#pragma warning restore 612, 618
        }
    }
}
