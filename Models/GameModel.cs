using System.ComponentModel.DataAnnotations.Schema;

namespace BetValue.Models
{
    public class GameModel
    {
        public int Id { get; set; }
        public int? HomeTeamId { get; set; }
        public TeamModel? HomeTeam { get; set; }
        public int? AwayTeamId { get; set; }
        public TeamModel? AwayTeam { get; set; }

        [ForeignKey(nameof(Serie))]
        public int SerieId { get; set; }
        public SerieModel Serie { get; set; }
        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }
        public int Round { get; set; }
        public int Winner { get; set; }
        public string? Odds1 { get; set; }
        public string? OddsX { get; set; }
        public string? Odds2 { get; set; }
        public double OddsChances1 { get; set; }
        public double OddsChancesX { get; set; }
        public double OddsChances2 { get; set; }
        public double CorrectOdds1 { get; set; }
        public double CorrectOddsX { get; set; }
        public double CorrectOdds2 { get; set; }
        public string? WhatBetHasValue { get; set; }
        public double BetValue { get; set; }
        public bool IsPlayed { get; set; }
        public string? Pitch { get; set; }
        public DateTime Date { get; set; }
        public GameModel()
        {
        }
        public GameModel(TeamModel HomeTeam, TeamModel AwayTeam, int Round, DateTime Date, SerieModel Serie)
        {
            IsPlayed = false;
            this.Round = Round;
            this.HomeTeam = HomeTeam;
            this.AwayTeam = AwayTeam;
            this.Date = Date;
            Pitch = this.HomeTeam.Pitch;
            this.Serie = Serie;
            Winner = -1;
            this.HomeTeam.Games.Add(this);
            this.AwayTeam.Games.Add(this);
        }
        public GameModel(TeamModel hTeam, TeamModel aTeam, int hGoals, int aGoals, int aRound, DateTime date, SerieModel serie)
        {
            Serie = serie;
            Round = aRound;
            HomeTeam = hTeam;
            AwayTeam = aTeam;
            Date = date;
            Pitch = HomeTeam.Pitch;
            IsPlayed = true;
            HomeGoals = hGoals;
            AwayGoals = aGoals;
            if (HomeGoals > AwayGoals)
            {
                Winner = 1;
            }
            else if (HomeGoals < AwayGoals)
            {
                Winner = 2;
            }
            else
            {
                Winner = 0;
            }

            HomeTeam.Games.Add(this);

            AwayTeam.Games.Add(this);
        }
    }
}