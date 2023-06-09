﻿// <auto-generated />
using System;
using BetValue.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BetValue.Migrations
{
    [DbContext(typeof(BetValueDbContext))]
    [Migration("20230309193052_1")]
    partial class _1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BetValue.Models.BetModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Bet")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("BetValue")
                        .HasColumnType("float");

                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.Property<double>("Odds")
                        .HasColumnType("float");

                    b.Property<string>("Result")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Bets");
                });

            modelBuilder.Entity("BetValue.Models.CountryModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("BetValue.Models.GameModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AwayGoals")
                        .HasColumnType("int");

                    b.Property<int?>("AwayTeamId")
                        .HasColumnType("int");

                    b.Property<double>("BetValue")
                        .HasColumnType("float");

                    b.Property<double>("CorrectOdds1")
                        .HasColumnType("float");

                    b.Property<double>("CorrectOdds2")
                        .HasColumnType("float");

                    b.Property<double>("CorrectOddsX")
                        .HasColumnType("float");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("HomeGoals")
                        .HasColumnType("int");

                    b.Property<int?>("HomeTeamId")
                        .HasColumnType("int");

                    b.Property<bool>("IsPlayed")
                        .HasColumnType("bit");

                    b.Property<int?>("LeagueModelId")
                        .HasColumnType("int");

                    b.Property<string>("Odds1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Odds2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("OddsChances1")
                        .HasColumnType("float");

                    b.Property<double>("OddsChances2")
                        .HasColumnType("float");

                    b.Property<double>("OddsChancesX")
                        .HasColumnType("float");

                    b.Property<string>("OddsX")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Pitch")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Round")
                        .HasColumnType("int");

                    b.Property<int>("SerieId")
                        .HasColumnType("int");

                    b.Property<int?>("TeamModelId")
                        .HasColumnType("int");

                    b.Property<string>("WhatBetHasValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Winner")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AwayTeamId");

                    b.HasIndex("HomeTeamId");

                    b.HasIndex("LeagueModelId");

                    b.HasIndex("SerieId");

                    b.HasIndex("TeamModelId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("BetValue.Models.LeagueModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CountryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TxtOdds")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UrlChances")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UrlOdds")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UrlSchedule")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("Leagues");
                });

            modelBuilder.Entity("BetValue.Models.SerieMemberModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Difficulty")
                        .HasColumnType("float");

                    b.Property<int>("EndPoint")
                        .HasColumnType("int");

                    b.Property<int>("GamesPlayed")
                        .HasColumnType("int");

                    b.Property<int>("GamesPlayedOnGrass")
                        .HasColumnType("int");

                    b.Property<int>("GamesPlayedOnPlastic")
                        .HasColumnType("int");

                    b.Property<int>("GoalDiff")
                        .HasColumnType("int");

                    b.Property<int>("GoalsAgainst")
                        .HasColumnType("int");

                    b.Property<int>("GoalsFor")
                        .HasColumnType("int");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.Property<int>("PointsOnGrass")
                        .HasColumnType("int");

                    b.Property<int>("PointsOnPlastic")
                        .HasColumnType("int");

                    b.Property<int>("Rank")
                        .HasColumnType("int");

                    b.Property<int>("SerieId")
                        .HasColumnType("int");

                    b.Property<double>("SpecialAverage")
                        .HasColumnType("float");

                    b.Property<int>("TeamId")
                        .HasColumnType("int");

                    b.Property<int>("xP")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SerieId");

                    b.HasIndex("TeamId");

                    b.ToTable("SerieMembers");
                });

            modelBuilder.Entity("BetValue.Models.SerieModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("LeagueId")
                        .HasColumnType("int");

                    b.Property<int>("NbrOfTeams")
                        .HasColumnType("int");

                    b.Property<string>("TxtChances")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TxtSchedule")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Year")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("aGoals")
                        .HasColumnType("int");

                    b.Property<int>("hGoals")
                        .HasColumnType("int");

                    b.Property<int>("maxTeamNameLength")
                        .HasColumnType("int");

                    b.Property<int>("nbrOfGames")
                        .HasColumnType("int");

                    b.Property<int>("omg")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LeagueId");

                    b.ToTable("Series");
                });

            modelBuilder.Entity("BetValue.Models.TeamModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("LeagueModelId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Pitch")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortNames")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LeagueModelId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("BetValue.Models.BetModel", b =>
                {
                    b.HasOne("BetValue.Models.GameModel", "Game")
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("BetValue.Models.GameModel", b =>
                {
                    b.HasOne("BetValue.Models.TeamModel", "AwayTeam")
                        .WithMany()
                        .HasForeignKey("AwayTeamId");

                    b.HasOne("BetValue.Models.TeamModel", "HomeTeam")
                        .WithMany()
                        .HasForeignKey("HomeTeamId");

                    b.HasOne("BetValue.Models.LeagueModel", null)
                        .WithMany("Games")
                        .HasForeignKey("LeagueModelId");

                    b.HasOne("BetValue.Models.SerieModel", "Serie")
                        .WithMany("Games")
                        .HasForeignKey("SerieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BetValue.Models.TeamModel", null)
                        .WithMany("Games")
                        .HasForeignKey("TeamModelId");

                    b.Navigation("AwayTeam");

                    b.Navigation("HomeTeam");

                    b.Navigation("Serie");
                });

            modelBuilder.Entity("BetValue.Models.LeagueModel", b =>
                {
                    b.HasOne("BetValue.Models.CountryModel", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId");

                    b.Navigation("Country");
                });

            modelBuilder.Entity("BetValue.Models.SerieMemberModel", b =>
                {
                    b.HasOne("BetValue.Models.SerieModel", "Serie")
                        .WithMany("SerieMembers")
                        .HasForeignKey("SerieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BetValue.Models.TeamModel", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Serie");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("BetValue.Models.SerieModel", b =>
                {
                    b.HasOne("BetValue.Models.LeagueModel", "League")
                        .WithMany("Series")
                        .HasForeignKey("LeagueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("League");
                });

            modelBuilder.Entity("BetValue.Models.TeamModel", b =>
                {
                    b.HasOne("BetValue.Models.LeagueModel", null)
                        .WithMany("Teams")
                        .HasForeignKey("LeagueModelId");
                });

            modelBuilder.Entity("BetValue.Models.LeagueModel", b =>
                {
                    b.Navigation("Games");

                    b.Navigation("Series");

                    b.Navigation("Teams");
                });

            modelBuilder.Entity("BetValue.Models.SerieModel", b =>
                {
                    b.Navigation("Games");

                    b.Navigation("SerieMembers");
                });

            modelBuilder.Entity("BetValue.Models.TeamModel", b =>
                {
                    b.Navigation("Games");
                });
#pragma warning restore 612, 618
        }
    }
}
