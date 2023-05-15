using System.ComponentModel.DataAnnotations.Schema;

namespace BetValue.Models
{
    public class GameModel
    {
        public int Id { get; set; }
        [ForeignKey(nameof(HomeTeam))]
        public int? HomeTeamId { get; set; }
        public TeamModel? HomeTeam { get; set; }
        [ForeignKey(nameof(AwayTeam))]
        public int? AwayTeamId { get; set; }
        public TeamModel? AwayTeam { get; set; }
        public int SerieId { get; set; }
        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }
        public int Round { get; set; }
        public int Winner { get; set; }
        public int homeCorners { get; set; }
        public int awayCorners { get; set; }
        public double CorrectOdds1 { get; set; }
        public double CorrectOddsX { get; set; }
        public double CorrectOdds2 { get; set; }
        public double CorrectOdds1DNB { get; set; }
        public double CorrectOdds2DNB { get; set; }
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
            SerieId = Serie.Id;
            Winner = -1;
            BetValue = -10;
        }
        public GameModel(TeamModel hTeam, TeamModel aTeam, int hGoals, int aGoals, int aRound, DateTime date, SerieModel serie)
        {
            SerieId = serie.Id;
            Round = aRound;
            HomeTeam = hTeam;
            AwayTeam = aTeam;
            Date = date;
            Pitch = HomeTeam.Pitch;
            IsPlayed = true;
            HomeGoals = hGoals;
            AwayGoals = aGoals;
            BetValue = -10;
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
        }
    }
}