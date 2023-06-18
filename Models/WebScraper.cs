
using BetValue.Repos;
using PuppeteerSharp;
using System.Text;

namespace BetValue.Models
{
    public class WebScraper
    {
        private readonly UnitOfWork uow;
        public Dictionary<string, string> TeamNameDictionary { get; set; } = new();
        public StringBuilder NotFoundGames { get; set; }
        public StringBuilder AddedGames { get; set; }
        public StringBuilder AddedLeagues { get; set; }
        public StringBuilder AddedSeries { get; set; }
        public StringBuilder AddedTeams { get; set; }
        public StringBuilder UpdatedOdds { get; set; }
        public StringBuilder UpdatedChances { get; set; }

        public string? text { get; set; }

        public WebScraper(UnitOfWork unitOfWork)
        {
            this.uow = unitOfWork;
            PopulateDictionary();
            text = "";
            NotFoundGames = new();
            UpdatedOdds = new();
        }
        public async Task<string> UrlToStringAsync(string fullUrl, string? pushButton = null)
        {
            string textString = "";

            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                ExecutablePath = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe"
            }, null))
            {
                using (var page = await browser.NewPageAsync())
                {
                    await page.GoToAsync(fullUrl); // Go to web page

                    var buttons = await page.QuerySelectorAllAsync("button"); // All buttons
                    foreach (var button in buttons) // Checks if theres a accept-cookies button and clicks it
                    {
                        var text = await button.EvaluateFunctionAsync<string>("(element) => element.innerText");
                        if (text == "GODKÄNN")
                        {
                            // Click the button
                            await button.ClickAsync();
                            //await page.WaitForNavigationAsync();
                            await page.WaitForTimeoutAsync(1000);
                            break;
                        }
                    }
                    if (pushButton != null)
                    {
                        var labels = await page.QuerySelectorAllAsync("label"); // All Labels
                        foreach (var label in labels) // Clicks label with "pushButton" text
                        {
                            var text = await label.EvaluateFunctionAsync<string>("(element) => element.innerText");
                            if (text == pushButton)
                            {
                                // Click the button
                                await label.ClickAsync();
                                await page.WaitForTimeoutAsync(1000);
                                var seeMoreButton1 = await page.QuerySelectorAsync(".more-upcoming.btn-cta.btn-cta-oneline");
                                if (seeMoreButton1 != null)
                                {
                                    await seeMoreButton1.ClickAsync();
                                    //await page.WaitForTimeoutAsync(1000);
                                }
                                var seeMoreButton2 = await page.QuerySelectorAsync(".more-completed.btn-cta.btn-cta-oneline");
                                if (seeMoreButton2 != null)
                                {
                                    await seeMoreButton2.ClickAsync();
                                    //await page.WaitForTimeoutAsync(1000);
                                }
                                break;
                            }
                        }
                    }
                    await page.WaitForTimeoutAsync(1000);
                    //await page.WaitForNavigationAsync();
                    textString = await page.EvaluateExpressionAsync<string>("document.body.innerText"); // Save all innertext in web page
                }
            }
            //text = textString;
            return textString;
        }
        public async Task LoadUnibetOddsAsync(string innerHtml)
        {
            string[] lines = innerHtml.Split("\n"); // Split text into lines
            if (lines.Length > 1)
            {
                for (int j = 0; j < lines.Length; j++) // For every line of text
                {

                    if (lines[j].Contains("+"))
                    {
                        string time = lines[j - 6];
                        string homeTeamName = lines[j - 5];
                        string awayTeamName = lines[j - 4];
                        TeamNameDictionary.TryGetValue(homeTeamName, out string dictionaryHomeName);
                        TeamNameDictionary.TryGetValue(awayTeamName, out string dictionaryAwayName);
                        if (dictionaryHomeName != null) homeTeamName = dictionaryHomeName;
                        if (dictionaryAwayName != null) awayTeamName = dictionaryAwayName;

                        string odds1 = lines[j - 3];
                        string oddsX = lines[j - 2];
                        string odds2 = lines[j - 1];
                        if (!await LoadOddsToGameAsync(homeTeamName, awayTeamName, odds1, oddsX, odds2, time, "Unibet"))
                        {
                            NotFoundGames.Append($"{homeTeamName} - {awayTeamName} | Unibet");
                        }

                    }
                    else if (lines[j].Contains("A-Ö"))
                    {
                        break;
                    }
                }
            }
            else if (lines.Length == 1)
            {
                NotFoundGames.Append(lines[0]);
            }
        }
        public async Task LoadChancesAsync(string HTMLstring, string leagueName, string startYear, string endYear, string countryName)
        {
            int allGames = 0;
            //string[] lines = HTMLstring.Split("\n"); // Split text into lines
            LeagueModel? league = uow.LeagueModelRepository.GetLeague(leagueName); // Get the League
            if (league == null) // Add new League
            {
                CountryModel? country = await uow.CountryModelRepository.GetCountryAsync(countryName); //Get Country
                if (country == null)
                {
                    CountryModel newCountry = new()
                    {
                        Name = countryName,
                        ImageUrl = $"Flag_Of_{countryName}.png"
                    };
                    await uow.CountryModelRepository.AddCountryAsync(newCountry);
                    await uow.SaveAsync();
                    country = await uow.CountryModelRepository.GetCountryAsync(countryName);
                }
                LeagueModel newLeague = new LeagueModel(leagueName)
                {
                    Country = country,
                    TxtOdds = $"{leagueName}Odds.txt",
                };
                await uow.LeagueModelRepository.AddLeagueAsync(newLeague);
                await uow.SaveAsync();
                league = await uow.LeagueModelRepository.GetLeagueAsync(leagueName);
            }
            SerieModel? serie;
            if (startYear == endYear) serie = await uow.SerieModelRepository.GetSerieAsync(league, $"{startYear}"); // Get the Serie
            else serie = await uow.SerieModelRepository.GetSerieAsync(league, $"{startYear}-{endYear}");

            if (serie == null) // Add new serie
            {
                string newSerieYear;
                if (startYear == endYear) newSerieYear = startYear;
                else newSerieYear = $"{startYear}-{endYear}";

                serie = new SerieModel()
                {
                    League = league,
                    Year = newSerieYear,
                };
                await uow.SerieModelRepository.AddSerieAsync(serie);
                await uow.SaveAsync();
            }

            string[] lines = HTMLstring.Split("\n"); // Get All text lines
            NotFoundGames.Append(lines.Length);
            bool reachedCompletedMatches = false;

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] == "Completed matches") reachedCompletedMatches = true;
                if (lines[i] == "MORE SPORTS") break;
                string? homeTeamName = null;
                string? awayTeamName = null;
                string? shortName = null;

                if (!reachedCompletedMatches)
                {
                    if (lines[i].Length >= 2 && lines[i].Length <= 5 && lines[i].Contains("/"))
                    {
                        allGames++;
                        homeTeamName = lines[i + 2];
                        awayTeamName = lines[i + 7];

                        string dictionaryHomeName = null;
                        string dictionaryAwayName = null;
                        if (homeTeamName != null && awayTeamName != null)
                        {
                            TeamNameDictionary.TryGetValue(homeTeamName, out dictionaryHomeName);
                            TeamNameDictionary.TryGetValue(awayTeamName, out dictionaryAwayName);
                            if (dictionaryHomeName != null) homeTeamName = dictionaryHomeName;
                            if (dictionaryAwayName != null) awayTeamName = dictionaryAwayName;

                            GameModel? game = await uow.GameModelRepository.GetGameAsync(homeTeamName, awayTeamName, serie.Id);
                            if (game != null)
                            {
                                string[] dateSplit = lines[i].Split("/");
                                int month = int.Parse(dateSplit[0]);
                                int day = int.Parse(dateSplit[1]);
                                int year;

                                if (startYear == endYear || month > 6) year = int.Parse(startYear);
                                else year = int.Parse(endYear);
                                if (day != game.Date.Day && month != game.Date.Month)
                                {
                                    NotFoundGames.AppendLine($"Diffrent date:{homeTeamName} - {awayTeamName} / new Day: {day} / old Day: {game.Date.Day} / Month: {month}");
                                    game.Date = new(year, month, day);
                                }
                                game.CorrectOdds1 = Math.Round(1 / (double.Parse(lines[i + 3].Replace("%", "").Trim()) / 100), 2);
                                game.CorrectOddsX = Math.Round(1 / (double.Parse(lines[i + 4].Replace("%", "").Trim()) / 100), 2);
                                game.CorrectOdds2 = Math.Round(1 / (double.Parse(lines[i + 8].Replace("%", "").Trim()) / 100), 2);
                                game.CorrectOdds1DNB = Math.Round(GetDNBOdds(game.CorrectOdds1, game.CorrectOddsX), 2);
                                game.CorrectOdds2DNB = Math.Round(GetDNBOdds(game.CorrectOdds2, game.CorrectOddsX), 2);

                                uow.GameModelRepository.UpdateGame(game);
                                await uow.SaveAsync();
                            }
                            else
                            {
                                NotFoundGames.AppendLine($"Not Found: {homeTeamName} - {awayTeamName}\n");
                                TeamModel? homeTeam = await uow.TeamModelRepository.GetTeamAsync(homeTeamName);
                                if (homeTeam == null)
                                {
                                    homeTeam = new TeamModel(homeTeamName);
                                    //TeamNameDictionary.TryGetValue(homeTeamName, out shortName);
                                    //homeTeam.ShortNames.Add(homeTeamName);
                                    //if (shortName != null)
                                    //{
                                    //    homeTeam.ShortNames.Add(shortName);
                                    //    shortName = null;
                                    //}

                                    await uow.TeamModelRepository.AddTeamAsync(homeTeam);
                                    await uow.SaveAsync();
                                }
                                TeamModel? awayTeam = await uow.TeamModelRepository.GetTeamAsync(awayTeamName);
                                if (awayTeam == null)
                                {
                                    awayTeam = new TeamModel(awayTeamName);
                                    //TeamNameDictionary.TryGetValue(awayTeamName, out shortName);
                                    //awayTeam.ShortNames.Add(awayTeamName);
                                    //if (shortName != null)
                                    //{
                                    //    awayTeam.ShortNames.Add(shortName);
                                    //    shortName = null;
                                    //}
                                    await uow.TeamModelRepository.AddTeamAsync(awayTeam);
                                    await uow.SaveAsync();
                                }
                                string[] dateSplit = lines[i].Split("/");
                                int month = int.Parse(dateSplit[0]);
                                int day = int.Parse(dateSplit[1]);
                                int year;
                                if (startYear == endYear || month > 6) year = int.Parse(startYear);
                                else year = int.Parse(endYear);

                                DateTime dateTime = new(year, month, day);

                                GameModel newGame = new GameModel(homeTeam, awayTeam, 0, dateTime, serie);
                                newGame.CorrectOdds1 = Math.Round(1 / (double.Parse(lines[i + 3].Replace("%", "").Trim()) / 100), 2);
                                newGame.CorrectOddsX = Math.Round(1 / (double.Parse(lines[i + 4].Replace("%", "").Trim()) / 100), 2);
                                newGame.CorrectOdds2 = Math.Round(1 / (double.Parse(lines[i + 8].Replace("%", "").Trim()) / 100), 2);
                                newGame.CorrectOdds1DNB = Math.Round(GetDNBOdds(newGame.CorrectOdds1, newGame.CorrectOddsX), 2);
                                newGame.CorrectOdds2DNB = Math.Round(GetDNBOdds(newGame.CorrectOdds2, newGame.CorrectOddsX), 2);
                                await uow.GameModelRepository.AddGameAsync(newGame);
                                await uow.SaveAsync();

                            }
                        }
                        homeTeamName = null;
                        awayTeamName = null;
                    }
                }
                else
                {       // Played games
                    if (lines[i].Length >= 2 && lines[i].Length <= 5 && lines[i].Contains("/"))
                    {
                        allGames++;
                        string homeTeamString = lines[i + 1];
                        string awayTeamString = lines[i + 4];
                        homeTeamName = homeTeamString.Remove(homeTeamString.Length - 1, 1);
                        awayTeamName = awayTeamString.Remove(awayTeamString.Length - 1, 1);

                        if (homeTeamName != null && awayTeamName != null)
                        {
                            string dictionaryHomeName = null;
                            string dictionaryAwayName = null;
                            TeamNameDictionary.TryGetValue(homeTeamName, out dictionaryHomeName);
                            TeamNameDictionary.TryGetValue(awayTeamName, out dictionaryAwayName);
                            if (dictionaryHomeName != null) homeTeamName = dictionaryHomeName;
                            if (dictionaryAwayName != null) awayTeamName = dictionaryAwayName;

                            GameModel? game = await uow.GameModelRepository.GetGameAsync(homeTeamName, awayTeamName, serie.Id);

                            if (game != null)
                            {
                                string[] dateSplit = lines[i].Split("/");
                                int month = int.Parse(dateSplit[0]);
                                int day = int.Parse(dateSplit[1]);
                                int year;

                                if (startYear == endYear || month > 6) year = int.Parse(startYear);
                                else year = int.Parse(endYear);
                                if (day != game.Date.Day && month != game.Date.Month)
                                {
                                    NotFoundGames.AppendLine($"Diffrent date:{homeTeamName} - {awayTeamName} / new Day: {day} / old Day: {game.Date.Day} / Month: {month}");
                                    game.Date = new(year, month, day);
                                }
                                game.CorrectOdds1 = Math.Round(1 / (double.Parse(lines[i + 2].Split("%")[0].Replace("%", "").Trim()) / 100), 2);
                                game.CorrectOddsX = Math.Round(1 / (double.Parse(lines[i + 2].Split("%")[1].Replace("%", "").Trim()) / 100), 2);
                                game.CorrectOdds2 = Math.Round(1 / (double.Parse(lines[i + 5].Replace("%", "").Trim()) / 100), 2);
                                game.CorrectOdds1DNB = Math.Round(GetDNBOdds(game.CorrectOdds1, game.CorrectOddsX), 2);
                                game.CorrectOdds2DNB = Math.Round(GetDNBOdds(game.CorrectOdds2, game.CorrectOddsX), 2);

                                if (!game.IsPlayed)
                                {
                                    int homeGoals = int.Parse(homeTeamString.Substring(homeTeamString.Length - 1));
                                    int awayGoals = int.Parse(awayTeamString.Substring(awayTeamString.Length - 1));
                                    await ChangeToPlayedGameAsync(game.Id, homeGoals, awayGoals);
                                }

                                uow.GameModelRepository.UpdateGame(game);
                                await uow.SaveAsync();
                            }
                            else
                            {
                                TeamModel? homeTeam = await uow.TeamModelRepository.GetTeamAsync(homeTeamName);
                                if (homeTeam == null)
                                {
                                    homeTeam = new TeamModel(homeTeamName);
                                    //TeamNameDictionary.TryGetValue(homeTeamName, out shortName);
                                    //homeTeam.ShortNames.Add(homeTeamName);
                                    //if (shortName != null)
                                    //{
                                    //    homeTeam.ShortNames.Add(shortName);
                                    //    shortName = null;
                                    //}

                                    await uow.TeamModelRepository.AddTeamAsync(homeTeam);
                                    await uow.SaveAsync();
                                }
                                TeamModel? awayTeam = await uow.TeamModelRepository.GetTeamAsync(awayTeamName);
                                if (awayTeam == null)
                                {
                                    awayTeam = new TeamModel(awayTeamName);
                                    //TeamNameDictionary.TryGetValue(awayTeamName, out shortName);
                                    //awayTeam.ShortNames.Add(homeTeamName);
                                    //if (shortName != null)
                                    //{
                                    //    awayTeam.ShortNames.Add(shortName);
                                    //    shortName = null;
                                    //}

                                    await uow.TeamModelRepository.AddTeamAsync(awayTeam);
                                    await uow.SaveAsync();
                                }

                                int homeGoals = int.Parse(homeTeamString.Substring(homeTeamString.Length - 1));
                                int awayGoals = int.Parse(awayTeamString.Substring(awayTeamString.Length - 1));

                                string[] dateSplit = lines[i].Split("/");
                                int month = int.Parse(dateSplit[0]);
                                int day = int.Parse(dateSplit[1]);
                                int year;
                                if (startYear == endYear || month > 6) year = int.Parse(startYear);
                                else year = int.Parse(endYear);

                                DateTime dateTime = new(year, month, day);
                                GameModel newGame = new GameModel(homeTeam, awayTeam, homeGoals, awayGoals, 0, dateTime, serie);
                                newGame.CorrectOdds1 = Math.Round(1 / (double.Parse(lines[i + 2].Split("%")[0].Replace("%", "").Trim()) / 100), 2);
                                newGame.CorrectOddsX = Math.Round(1 / (double.Parse(lines[i + 2].Split("%")[1].Replace("%", "").Trim()) / 100), 2);
                                newGame.CorrectOdds2 = Math.Round(1 / (double.Parse(lines[i + 5].Replace("%", "").Trim()) / 100), 2);
                                newGame.CorrectOdds1DNB = Math.Round(GetDNBOdds(game.CorrectOdds1, game.CorrectOddsX), 2);
                                newGame.CorrectOdds2DNB = Math.Round(GetDNBOdds(game.CorrectOdds2, game.CorrectOddsX), 2);
                                await uow.GameModelRepository.AddGameAsync(newGame);
                                await uow.SaveAsync();
                                await UpdateSerieMembersAsync(newGame);
                                await uow.SaveAsync();
                            }
                        }
                        homeTeamName = null;
                        awayTeamName = null;
                    }
                }
            }
        }
        private async Task ChangeToPlayedGameAsync(int id, int homeGoals, int awayGoals)
        {
            GameModel? game = uow.GameModelRepository.GetGame(id);
            if (game != null)
            {
                game.IsPlayed = true;
                game.HomeGoals = homeGoals;
                game.AwayGoals = awayGoals;
                if (homeGoals > awayGoals) game.Winner = 1;
                else if (awayGoals > homeGoals) game.Winner = 2;
                await UpdateSerieMembersAsync(game);
                uow.GameModelRepository.UpdateGame(game);
                await uow.SaveAsync();
            }

        }
        private async Task UpdateSerieMembersAsync(GameModel game)
        {
            SerieMemberModel? homeSerieMember = await uow.SerieMemberModelRepository.GetSerieMemberAsync(game.HomeTeam, game.SerieId);
            if (homeSerieMember == null)
            {
                SerieModel? serie = await uow.SerieModelRepository.GetSerieAsync(game.SerieId);
                SerieMemberModel newMember = new()
                {
                    Team = await uow.TeamModelRepository.GetTeamAsync(game.HomeTeam.Name),
                    Serie = serie,
                };
                await uow.SerieMemberModelRepository.AddSerieMemberAsync(newMember);
                await uow.SaveAsync();
                homeSerieMember = await uow.SerieMemberModelRepository.GetSerieMemberAsync(game.HomeTeam, game.SerieId);
            }
            SerieMemberModel? awaySerieMember = await uow.SerieMemberModelRepository.GetSerieMemberAsync(game.AwayTeam, game.SerieId);
            if (awaySerieMember == null)
            {
                SerieModel? serie = await uow.SerieModelRepository.GetSerieAsync(game.SerieId);
                SerieMemberModel newMember = new()
                {
                    Team = await uow.TeamModelRepository.GetTeamAsync(game.AwayTeam.Name),
                    Serie = serie
                };
                await uow.SerieMemberModelRepository.AddSerieMemberAsync(newMember);
                await uow.SaveAsync();
                awaySerieMember = await uow.SerieMemberModelRepository.GetSerieMemberAsync(game.AwayTeam, game.SerieId);

            }
            if (homeSerieMember != null && awaySerieMember != null)
            {

                int homePoints = GetPoints("home", game);
                homeSerieMember.Points += homePoints;
                homeSerieMember.GamesPlayed++;
                homeSerieMember.GoalsFor += game.HomeGoals;
                homeSerieMember.GoalsAgainst += game.AwayGoals;
                homeSerieMember.GoalDiff = homeSerieMember.GoalsFor - homeSerieMember.GoalsAgainst;

                int awayPoints = GetPoints("away", game);
                awaySerieMember.Points += awayPoints;
                awaySerieMember.GamesPlayed++;
                awaySerieMember.GoalsFor += game.AwayGoals;
                awaySerieMember.GoalsAgainst += game.HomeGoals;
                awaySerieMember.GoalDiff = awaySerieMember.GoalsFor - awaySerieMember.GoalsAgainst;

                if (game.HomeTeam.Pitch == "Plast")
                {
                    homeSerieMember.PointsOnPlastic += homePoints;
                    awaySerieMember.PointsOnPlastic += awayPoints;
                }
                else
                {
                    homeSerieMember.PointsOnGrass += homePoints;
                    awaySerieMember.PointsOnGrass += awayPoints;
                }
                uow.SerieMemberModelRepository.UpdateSerieMember(homeSerieMember);
                uow.SerieMemberModelRepository.UpdateSerieMember(awaySerieMember);
                await uow.SaveAsync();
            }

        }
        private int GetPoints(string v, GameModel game)
        {
            if (v == "home")
            {
                if (game.Winner == 1)
                {
                    return 3;
                }
                else if (game.Winner == 2)
                {
                    return 0;
                }
            }
            else
            {
                if (game.Winner == 2)
                {
                    return 3;
                }
                else if (game.Winner == 1)
                {
                    return 0;
                }
            }
            return 1;
        }
        private async Task<bool> LoadOddsToGameAsync(string homeTeamName, string awayTeamName, string odds1String, string oddsXString, string odds2String, string time, string company)
        {
            bool success = false;
            GameModel? game = await uow.GameModelRepository.GetGameAsync(homeTeamName, awayTeamName);
            UpdatedOdds.AppendLine($"{homeTeamName} - {awayTeamName}");
            if (game != null)
            {
                success = true;
                OddsModel? existingOdds = await uow.OddsModelRepository.GetOddsAsync(game.Id, company);
                double odds1 = double.Parse(odds1String.Replace(".", ","));
                double oddsX = double.Parse(oddsXString.Replace(".", ","));
                double odds2 = double.Parse(odds2String.Replace(".", ","));
                double odds1DNB = Math.Round(GetDNBOdds(odds1, oddsX), 2);
                double odds2DNB = Math.Round(GetDNBOdds(odds2, oddsX), 2);

                TimeSpan hoursAndMinutes = TimeSpan.Parse(time);
                if (game.Date.TimeOfDay != hoursAndMinutes)
                {
                    UpdatedOdds.AppendLine($"{game.HomeTeam.Name} - {game.AwayTeam.Name} {company} {odds1} {oddsX} {odds2} {odds1DNB} {odds2DNB} {hoursAndMinutes}");
                    DateTime newGameTime = new(game.Date.Year, game.Date.Month, game.Date.Day, hoursAndMinutes.Hours, hoursAndMinutes.Minutes, 0);
                    game.Date = newGameTime;
                    uow.GameModelRepository.UpdateGame(game);
                    await uow.SaveAsync();
                }

                success = true;

                if (existingOdds == null)
                {
                    OddsModel newOdds = new OddsModel()
                    {
                        Operator = company,
                        HomeWin = odds1,
                        Draw = oddsX,
                        AwayWin = odds2,
                        HomeWinDNB = odds1DNB,
                        AwayWinDNB = odds2DNB,

                        FavoriteDNB = Math.Min(odds1DNB, odds2DNB),
                        FavoriteOdds = Math.Min(odds1, odds2),
                        Game = game
                    };
                    await uow.OddsModelRepository.AddOddsAsync(newOdds);
                    await uow.SaveAsync();
                    await SetBetValueAsync(game);

                }
                else //if (odds1 != existingOdds.HomeWin || odds2 != existingOdds.AwayWin)
                {
                    existingOdds.HomeWin = odds1;
                    existingOdds.AwayWin = odds2;
                    existingOdds.HomeWinDNB = odds1DNB;
                    existingOdds.AwayWinDNB = odds2DNB;
                    existingOdds.FavoriteDNB = Math.Min(odds1DNB, odds2DNB);
                    existingOdds.FavoriteOdds = Math.Min(odds1, odds2);
                    uow.OddsModelRepository.UpdateOdds(existingOdds);
                    await uow.SaveAsync();
                    await SetBetValueAsync(game);

                }
            }
            return success;
        }
        private double GetDNBOdds(double odds, double drawOdds)
        {
            return ((double)1 - ((double)1 / drawOdds)) * odds;

        }
        private async Task SetBetValueAsync(GameModel game)
        {
            GameModel? dbGame = await uow.GameModelRepository.GetGameAsync(game.Id);
            List<OddsModel>? oddsList = await uow.OddsModelRepository.GetAllOddsInGameAsync(dbGame);
            dbGame.BetValue = -10;
            dbGame.WhatBetHasValue = "";
            double topOdds1 = 0d;
            double topOdds2 = 0d;
            foreach (OddsModel odds in oddsList)
            {
                if (odds.HomeWin > topOdds1)
                {
                    topOdds1 = odds.HomeWin;
                }
                if (odds.AwayWin > topOdds2)
                {
                    topOdds2 = odds.AwayWin;
                }

                double value1 = odds.HomeWin - game.CorrectOdds1;
                double value2 = odds.AwayWin - game.CorrectOdds2;
                double value = Math.Max(value1, value2);
                if (value > dbGame.BetValue)
                {
                    dbGame.BetValue = Math.Round(value, 2);
                    if (value1 > value2)
                    {
                        dbGame.WhatBetHasValue = "1";
                        dbGame.ValueOdds = topOdds1;
                    }
                    else
                    {
                        dbGame.WhatBetHasValue = "2";
                        dbGame.ValueOdds = topOdds2;
                    }

                }
            }
            uow.GameModelRepository.UpdateGame(dbGame);
            await uow.SaveAsync();
        }
        private void PopulateDictionary()
        {
            TeamNameDictionary.Add("Man. City", "Manchester City");
            TeamNameDictionary.Add("Man. United", "Manchester United");
            TeamNameDictionary.Add("Nottm Forest", "Nottingham");
            TeamNameDictionary.Add("Öster", "Östers IF");
            TeamNameDictionary.Add("Almería", "Almeria");
            TeamNameDictionary.Add("AIK", "AIK Solna");
            TeamNameDictionary.Add("Bor. Dortmund", "Borussia Dortmund");
            TeamNameDictionary.Add("Bor. M'gladbach", "Borussia Mönchengladbach");
            TeamNameDictionary.Add("Bor. M'gladbach U19", "Borussia Mönchengladbach U19");
            TeamNameDictionary.Add("Sheffield Utd", "Sheffield United");
            TeamNameDictionary.Add("BP", "Brommapojkarna");
            TeamNameDictionary.Add("QPR", "Queens Park Rangers");
            TeamNameDictionary.Add("Cádiz", "Cadiz");
            TeamNameDictionary.Add("Leeds", "Leeds United");
            TeamNameDictionary.Add("Bayern München", "Bayern Munich");
            TeamNameDictionary.Add("Stuttgart", "Vfb Stuttgart");
            TeamNameDictionary.Add("Freiburg", "SC Freiburg");
            TeamNameDictionary.Add("Inter", "Inter Milan");
            TeamNameDictionary.Add("Milan", "AC Milan");
            TeamNameDictionary.Add("Hellas Verona", "Verona");

        }

        public async Task UpdateAllChancesAsync()
        {
            //var Leagues = await uow.LeagueModelRepository.GetLeaguesAsync();

            //await LoadChancesAsync(await UrlToStringAsync("https://projects.fivethirtyeight.com/soccer-predictions/allsvenskan/", "Matches"), "Allsvenskan", "2023", "2023", "Sweden");
            //await LoadChancesAsync(await UrlToStringAsync("https://projects.fivethirtyeight.com/soccer-predictions/premier-league/", "Matches"), "Premier League", "2022", "2023", "England");
            //await LoadChancesAsync(await UrlToStringAsync("https://projects.fivethirtyeight.com/soccer-predictions/la-liga/", "Matches"), "La Liga", "2022", "2023", "Spain");
            //await LoadChancesAsync(await UrlToStringAsync("https://projects.fivethirtyeight.com/soccer-predictions/bundesliga/", "Matches"), "Bundesliga", "2022", "2023", "Germany");
            //await LoadChancesAsync(await UrlToStringAsync("https://projects.fivethirtyeight.com/soccer-predictions/serie-a/", "Matches"), "Serie A", "2022", "2023", "Italy");
            await LoadChancesAsync(await UrlToStringAsync("https://projects.fivethirtyeight.com/soccer-predictions/ligue-1/", "Matches"), "Ligue 1", "2022", "2023", "France");
            //await LoadChancesAsync(await UrlToStringAsync("https://projects.fivethirtyeight.com/soccer-predictions/eredivisie/", "Matches"), "Eredivisie", "2022", "2023", "Netherlands");
            //await LoadChancesAsync(await UrlToStringAsync("https://projects.fivethirtyeight.com/soccer-predictions/primeira-liga/", "Matches"), "Primeira Liga", "2022", "2023", "Portugal");

        }

        public async Task UpdateAllOddsAsync()
        {
            await LoadUnibetOddsAsync(await UrlToStringAsync("https://www.unibet.se/betting/sports/filter/football/sweden/allsvenskan/all/matches"));
        }
    }
}
