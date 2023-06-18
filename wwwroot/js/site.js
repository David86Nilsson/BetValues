//Getters
const gameBox = document.querySelector('#GameBox');
const gameBoxTextContainer = document.querySelector('#gameBoxTextContainer');
const topBoxContainer = document.querySelector('#topContainer');
const closeBtn = document.querySelector('#closeGameBox');
const gameBoxTitle = document.querySelector('#gameBoxTitle');
const inputValue = document.querySelector('#inputValue');
const inputOdds = document.querySelector('#inputMaxOdds');
const countryList = document.querySelector('#countryList');
const filterForm = document.querySelector('#filterForm');
const filterButton = document.querySelector('#filterButton');
const searchButton = document.querySelector('#searchButton');
const topFirstBox = document.querySelector('#topFirstBox');
const topSecondBox = document.querySelector('#topSecondBox');
const topThirdBox = document.querySelector('#topThirdBox');

const loadingContainer = document.querySelector('#loadingContainer');
const valueGamesContainer = document.querySelector('#valueGamesContainer');
const page = document.querySelector('#page');

const searchBox = document.querySelector('#searchBar');
const unplayedContainer = document.querySelector('#unplayedContainer');

const leagueContainer = document.querySelector('#leagueList');
const leagueHeadline = document.querySelector('.leagueHeadline');

const allUnplayedGamesWithOdds = document.querySelectorAll('.unPlayedGame');

let unplayedGamesWithOdds;
let allLeagues;
let allCountries;

let inputValueValue =0.2;
let inputOddsValue = 4.0;

//Setters

getData();

//EventListeners

document.addEventListener('click', Display);
searchBox.addEventListener('input', filterGames);
inputValue.addEventListener('input', filterValueGames);
inputOdds.addEventListener('input', filterValueGames);


//Functions
function getData() {
    if (leagueHeadline != null) {
        fetch("https://betvaluesapi.azurewebsites.net/api/GamesInSerie/" + leagueHeadline.id).then(res => res.json()).then(data => {
            if (data.length > 0) {
                unplayedGamesWithOdds = data;
                displayValueGames(unplayedGamesWithOdds);
                filterGames();
                displayTopBoxes(unplayedGamesWithOdds);
                topBoxContainer.classList.remove('hidden');
                
            }
            loadingContainer.classList.add('hidden');
            page.classList.remove('hidden');

        })
    }
    else {
        fetch("https://betvaluesapi.azurewebsites.net/api/OddsGames/").then(res => res.json()).then(data => {       
            unplayedGamesWithOdds = data;
            displayValueGames(unplayedGamesWithOdds);    
            filterGames();
            displayTopBoxes();
            topBoxContainer.classList.remove('hidden');
            loadingContainer.classList.add('hidden');
            page.classList.remove('hidden');
        });
    }
    fetch("https://betvaluesapi.azurewebsites.net/api/Leagues").then(res => res.json()).then(data => {
        allLeagues = data;
        allCountries = [...new Map(allLeagues.map(league => [league.country.name, league.country])).values()];
        displayCountries();
    });
}
function displayCountries() {
    allCountries.forEach(function (country) {

        const tml = `<li class=" col-4 box m-1 country padding-0" id="@country.Id">
            <span class="country padding-0"><img src="/Images/${country.imageUrl}" alt="xG Values" class="box country padding-0" id=${country.id} /></span>
        </li>`
        countryList.innerHTML += tml;
    })
}

function displayInUnplayedGames(games) {
    unplayedContainer.innerHTML = '';
    games.forEach(function (game) {
        unplayedContainer.innerHtml += `${game}`;
        //console.log(object);
    });
}

function displayUnplayedGames(games) {
    unplayedContainer.innerHTML = '';
    let homeTeam;
    let awayTeam;
    let date;
    games.forEach(function (game) {

        homeTeam = game.homeTeam;
        awayTeam = game.awayTeam;
        date = game.date;
        const tml = `<p class="gameContainer gameBorder p-3" id=${game.id}>${date.substring(0, 10)}<br />${homeTeam.name} - ${awayTeam.name}</p>`;
        unplayedContainer.innerHTML += tml;
    })
}
function displayTopBoxes() {
    const orderdGames = unplayedGamesWithOdds.sort((a, b) => b.clicked - a.clicked);
    const firstTopGame = orderdGames[0];
    const secondTopGame = orderdGames[1];
    const thirdTopGame = orderdGames[2];
    
    topFirstBox.innerHTML = `<p class="gameContainer gameBorder text-white p-3" id=${firstTopGame.id}>${firstTopGame.date.substring(0, 10)}<br />
    ${firstTopGame.homeTeam.name} - ${firstTopGame.awayTeam.name}<br />
    Value: ${firstTopGame.betValue} </p >`;
    topSecondBox.innerHTML = `<p class="gameContainer gameBorder text-white p-3" id=${secondTopGame.id}>${secondTopGame.date.substring(0, 10)}<br />
    ${secondTopGame.homeTeam.name} - ${secondTopGame.awayTeam.name}<br />
    Value: ${secondTopGame.betValue} </p >`;
    topThirdBox.innerHTML = `<p class="gameContainer gameBorder text-white p-3" id=${thirdTopGame.id}>${thirdTopGame.date.substring(0, 10)}<br />
    ${thirdTopGame.homeTeam.name} - ${thirdTopGame.awayTeam.name}<br />
    Value: ${thirdTopGame.betValue} </p >`;
}
function filterGames() {
    if (searchBox.value.length > 0) {
        let searchedGames = unplayedGamesWithOdds.filter(g => g.homeTeam.name.toLowerCase().includes(searchBox.value.toLowerCase()) || g.awayTeam.name.toLowerCase().includes(searchBox.value.toLowerCase()));
        displayUnplayedGames(searchedGames);
        console.log(searchedGames);
    }
    else {
        displayUnplayedGames(unplayedGamesWithOdds);
    }
}
function filterValueGames() {
    if (inputValue.value.length > 0) {
        inputValueValue = inputValue.value;
    } else { inputValueValue = 0.2}

    if (inputOdds.value.length > 0) {
        inputOddsValue = inputOdds.value;
    } else { inputOddsValue = 4.0 }
    displayValueGames(unplayedGamesWithOdds);
}

function Display(event) {
    if (event.target.classList.contains('gameContainer')) {
        ShowGame(event);
    }
    else if (event.target.id == 'closeGameBox') {
        gameBox.classList.toggle('hidden');
        removeChosenGameBorder();
    }
    else if (event.target.classList.contains('country')) {
        displayLeagueList(event.target.id);
        removeChosenCountry();
        event.target.classList.toggle('chosenCountry');
    }
}

function displayLeagueList(countryId) {
    leagueContainer.innerHTML = '';
    const leaguesToShow = allLeagues.filter(l => l.countryId == countryId);
    leaguesToShow.forEach(league => {
        let tml = `
                  <a href="/Details/${league.id}" class="">
                    <div class="padding-0 box">
                      <span class="leagueLink">${league.name}</span>
                    </div>
                  </a>
                  `;
        leagueContainer.innerHTML += tml;
    })
}
function displayValueGames(games) {
    valueGamesContainer.innerHTML = '';
    let homeTeam;
    let awayTeam;
    let date;

    games.forEach(function (game) {
        if (game.betValue >= inputValueValue && game.valueOdds <= inputOddsValue) {
            homeTeam = game.homeTeam;
            awayTeam = game.awayTeam;
            date = game.date;
            const tml = `<p class="gameContainer gameBorder text-white p-3" id=${game.id}>${date.substring(0, 10)}<br />${homeTeam.name} - ${awayTeam.name}<br />Value: ${game.betValue} </p >`;
            valueGamesContainer.innerHTML += tml;
        }
    })
}



function ShowGame(event) {
    if (event.target.id > 0) {

        fetch("https://betvaluesapi.azurewebsites.net/api/OddsInGame/" + event.target.id).then(res => res.json()).then(data => {
            const GameOdds = data;
            const chosenGame = GameOdds[0].game;
            gameBoxTitle.innerHTML = `${chosenGame.homeTeam.name} - ${chosenGame.awayTeam.name}`;
            gameBoxTextContainer.innerHTML = "";
            if (gameBox.classList.contains('hidden')) {
                gameBox.classList.toggle('hidden');
            }

            removeChosenGameBorder();
            
            event.target.classList.add('chosenGameBorder');
            
            const date = new Date(chosenGame.date);
            let gameInfoHtml = "";
            gameInfoHtml = `<div class="chosenGameContainer mb-2 text-white">
                                <p class="text-white">${date.toLocaleString()}</p>
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
        fetch("https://betvaluesapi.azurewebsites.net/api/Games/" + event.target.id).then(res => res.json()).then(data => {
            console.log(data);
        });
    }

}
function toggleForm() {
    filterForm.classList.toggle('hidden');
    filterButton.innerText = filterButton.innerText == 'Filter' ? 'Hide' : 'Filter';
}
function toggleSearch() {
    searchBox.classList.toggle('hidden');
    searchButton.innerText = searchButton.innerText == 'Search' ? 'Hide' : 'Search';
}
function setAdressField() {

    // Modify the URL
    var newURL = "BetValues.net";

    // Change the URL
    window.history.replaceState({}, '', newURL);
    
}
function removeChosenGameBorder() {
    const prevoiusChosenGame = document.querySelector('.chosenGameBorder');
    if (prevoiusChosenGame != null) {
        prevoiusChosenGame.classList.remove('chosenGameBorder');
    }
}
function removeChosenCountry() {
    const prevoiusChosenGame = document.querySelector('.chosenCountry');
    if (prevoiusChosenGame != null) {
        prevoiusChosenGame.classList.remove('chosenCountry');
    }
}