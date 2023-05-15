//Getters
const gameBox = document.querySelector('#GameBox');
const gameBoxTextContainer = document.querySelector('#gameBoxTextContainer');
const closeBtn = document.querySelector('#closeGameBox');
const gameBoxTitle = document.querySelector('#gameBoxTitle');

const loadingContainer = document.querySelector('#loadingContainer');
const page = document.querySelector('#page');

const searchBox = document.querySelector('#searchBar');
const unplayedContainer = document.querySelector('#unplayedContainer');

const leagueContainer = document.querySelector('#leagueList');
const leagueHeadline = document.querySelector('.leagueHeadline');

const allUnplayedGamesWithOdds = document.querySelectorAll('.unPlayedGame');

let unplayedGamesWithOdds;
let allLeagues;

getData();

//EventListeners
document.addEventListener('click', Display);
searchBox.addEventListener('input', filterGames);
//Functions
function getData() {
    if (leagueHeadline != null) {
        fetch("https://betvaluesapi.azurewebsites.net/api/GamesInSerie/" + leagueHeadline.id).then(res => res.json()).then(data => {
            unplayedGamesWithOdds = data;
            loadingContainer.style.display = "none";
            page.classList.remove('hidden');
        });
    }
    else {
        fetch("https://betvaluesapi.azurewebsites.net/api/OddsGames/").then(res => res.json()).then(data => {
            unplayedGamesWithOdds = data;
            loadingContainer.style.display = "none";
            page.classList.remove('hidden');
        });
    }
    fetch("https://betvaluesapi.azurewebsites.net/api/Leagues").then(res => res.json()).then(data => {
        allLeagues = data;
    });
}

function displayInUnplayedGames(games) {
    unplayedContainer.innerHTML = '';
    games.forEach(function (game) {
        unplayedContainer.innerHtml += `${game}`;
        //console.log(object);
    });
}

function displayUnplayedGames(games) {
    unplayedContainer.innerHTML = '<h3>Upcoming games</h3>';
    let homeTeam;
    let awayTeam;
    let date;
    games.forEach(function (game) {

        homeTeam = game.homeTeam;
        awayTeam = game.awayTeam;
        date = game.date;
        const tml = `<p class="gameContainer border bg-light m-1 p-3" id=${game.id}>${date.substring(0, 10)}<br />${homeTeam.name} - ${awayTeam.name}</p>`;
        unplayedContainer.innerHTML += tml;
    })
}
function filterGames() {
    if (searchBox.value.length > 0) {
        let searchedGames = unplayedGamesWithOdds.filter(g => g.homeTeam.name.toLowerCase().includes(searchBox.value.toLowerCase()) || g.awayTeam.name.toLowerCase().includes(searchBox.value.toLowerCase()));
        displayUnplayedGames(searchedGames);
    }
    else {
        displayUnplayedGames(unplayedGamesWithOdds);
    }
}

function Display(event) {
    if (event.target.classList.contains('gameContainer')) {
        ShowGame(event);
    }
    else if (event.target.id == 'closeGameBox') {
        gameBox.style.display = "none";
    }
    else if (event.target.classList.contains('country')) {
        displayLeagueList(event.target.id);
    }
}

function displayLeagueList(countryId) {
    leagueContainer.innerHTML = '<div class="w-100 d-flex flex-column leagueLink">';
    const leaguesToShow = allLeagues.filter(l => l.countryId == countryId);
    leaguesToShow.forEach(league => {
        let tml = `
                  <a href="/Details/${league.id}" class="leagueLink">
                    <div class="padding-0 box">
                      <span class="">${league.name}</span>
                    </div>
                  </a>
                  `;
        leagueContainer.innerHTML += tml;
    })
    leagueContainer.innerHTML += '</ul>';
}



let chosenGame;
let GameOdds;
function ShowGame(event) {

    if (event.target.id > 0) {

        fetch("https://betvaluesapi.azurewebsites.net/api/OddsInGame/" + event.target.id).then(res => res.json()).then(data => {
            GameOdds = data;
            chosenGame = GameOdds[0].game;
            gameBoxTitle.innerHTML = `${chosenGame.homeTeam.name} - ${chosenGame.awayTeam.name}`;
            gameBoxTextContainer.innerHTML = "";
            gameBox.style.display = "block";
            const date = new Date(chosenGame.date);
            let gameInfoHtml = "";
            gameInfoHtml = `<div class="chosenGameContainer mb-2 text-white">
                                <p class="text-white">${date.toLocaleDateString()}</p>
                                 <p class="text-white">Correct Odds (from xG): ${chosenGame.correctOdds1} - ${chosenGame.correctOddsX} - ${chosenGame.correctOdds2}</p>
                                 <p class="text-white border-bottom pb-2">Actual odds on market: `;
            GameOdds.forEach(odds => {                         
                if (chosenGame.whatBetHasValue == 1 && chosenGame.betValue > 0) {
                    gameInfoHtml += `<span><strong class="green-text">${odds.homeWin}</strong></span> - ${odds.draw} - ${odds.awayWin} </p>`;
                }
                else if (chosenGame.whatBetHasValue == 2 && chosenGame.betValue > 0) {
                        gameInfoHtml += `${odds.homeWin} - ${odds.draw} - <span><strong class="green-text">${odds.awayWin}</strong></span></p>`               
                }
                else {
                        gameInfoHtml += `${odds.homeWin} - ${odds.draw} - ${odds.awayWin} </p>`
                    }
            })

            gameInfoHtml += `<p class="text-white border-bottom pb-2">Correct DNB Odds (from xG): ${chosenGame.correctOdds1DNB} - ${chosenGame.correctOdds2DNB}</p>`;

            if (chosenGame.betValue > 0) {
                gameInfoHtml += `<p class="text-white">Bet: ${chosenGame.whatBetHasValue}<br /> Overvalue: <span class="green-text"> ${chosenGame.betValue}</span></p>
                                 </div>`;
            }
            else if (chosenGame.betValue <= 0) {
                gameInfoHtml += `<p class="text-white">Bet: ${chosenGame.whatBetHasValue}<br /> Overvalue: <span class="red-text"> ${chosenGame.betValue}</span></p>
                                 </div>`;
            }
            gameBoxTextContainer.innerHTML += gameInfoHtml;
        });
    }

}
function toggleForm() {
    var form = document.getElementById("filterForm");
    if (form.style.display === "none") {
        form.style.display = "block";
    } else {
        form.style.display = "none";
    }
}