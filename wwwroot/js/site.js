//Getters
const gameBox = document.querySelector('#GameBox');
const searchBox = document.querySelector('#searchBar');
const unplayedContainer = document.querySelector('#unplayedContainer');
const leagueContainer = document.querySelector('#leagueList');

let allTeams;
fetch("https://localhost:7245/api/Teams").then(res => res.json()).then(data => {
    allTeams= data;
});

let unplayedGamesWithOdds;
fetch("https://localhost:7245/api/unplayedGames/").then(res => res.json()).then(data => {
    unplayedGamesWithOdds = data;
});

let allLeagues;
fetch("https://localhost:7245/api/Leagues").then(res => res.json()).then(data => {
    allLeagues = data;
});

//EventListeners
document.addEventListener('click', Display);
searchBox.addEventListener('input', filterGames);
//Functions

function displayUnplayedGames(games) {
    unplayedContainer.innerHTML = '';
    games.forEach(game => {
        //const tml = `<p class="gameContainer border bg-light m-1 p-3" id=${game.Id}>@Html.Raw("${game.Serie.League.Name}<br />${game.Date.ToShortDateString()} | ${game.HomeTeam.Name} - ${game.AwayTeam.Name}")</p>`;
        //const tml = `<p> ${game.name}</p>`;
        const tml = `<p>${game.homeTeam.name}</p>`
        unplayedContainer.innerHTML += tml;
    })
}
function filterGames() {
    if (searchBox.value.length > 3) {
        let searchedTeams = allTeams.filter(t => t.name.toLowerCase().includes(searchBox.value.toLowerCase()));
        //let searchedGames;
        //searchedTeams.forEach(t => searchedGames == t.Games);
        displayUnplayedGames(searchedTeams);
    }
    else {
        displayUnplayedGames(unplayedGamesWithOdds);
    }
}

function Display(event) {
    if (event.target.classList.contains('gameContainer')) {
        ShowGame(event);
    }
    else if (event.target.classList.contains('country')) {
        displayLeagueList(event.target.id);
    }
}

function displayLeagueList(countryId) {
    leagueContainer.innerHTML = '<ul>';
    const leaguesToShow = allLeagues.filter(l => l.countryId == countryId);
    leaguesToShow.forEach(league => {
        let tml = `
  <a href="/Details/${league.id}">
    <li class="padding-0 d-flex flex-column box m-5 align-items-start justify-content-start m-5 flagbackground">
      <span class="">${league.name}</span>
    </li>
  </a>
`;
        leagueContainer.innerHTML += tml;
    })
    leagueContainer.innerHTML += '</ul>';
}



let chosenGame;
function ShowGame(event) {
    if (event.target.id >1) {
        fetch("https://localhost:7245/api/Games/" + event.target.id).then(res => res.json()).then(data => {         
            gameBox.innerHTML = "";
            chosenGame = data;            
            const date = new Date(chosenGame.date);
            let gameInfoHtml = "";
            if (chosenGame.whatBetHasValue == 1) {
                gameInfoHtml = `<div class="chosenGameContainer">
                                <h5>${date.toLocaleDateString()}<br /> ${chosenGame.homeTeam.name} - ${chosenGame.awayTeam.name}</h5>
                                 <p>Correct Odds according to xG: ${chosenGame.correctOdds1} - ${chosenGame.correctOddsX} - ${chosenGame.correctOdds2}<br/>
                                 Actual odds on market: <span><strong>${chosenGame.odds1}</strong></span> - ${chosenGame.oddsX} - ${chosenGame.odds2} <br /> 
                                 Bet: ${chosenGame.whatBetHasValue}<br /> Overvalue: ${chosenGame.betValue}</p>
                                 </div>`;
            }
            else if (chosenGame.whatBetHasValue == 2) {
                gameInfoHtml = `<div class="chosenGameContainer">
                                <h5>${date.toLocaleDateString()}<br /> ${chosenGame.homeTeam.name} - ${chosenGame.awayTeam.name}</h5>
                                 <p>Correct Odds according to xG: ${chosenGame.correctOdds1} - ${chosenGame.correctOddsX} - ${chosenGame.correctOdds2}<br/>Actual odds on market:${chosenGame.odds1} - ${chosenGame.oddsX} - <span><strong>${chosenGame.odds2}</strong></span> <br /> Bet: ${chosenGame.whatBetHasValue}<br /> Overvalue: ${chosenGame.betValue}</p>
                                 </div>`;

            }
            gameBox.innerHTML += gameInfoHtml;
        });
    }
}