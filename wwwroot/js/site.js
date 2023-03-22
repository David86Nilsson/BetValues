//Getters
const gameBox = document.querySelector('#GameBox');
let allTeams;
fetch("https://localhost:7245/api/Teams").then(res => res.json()).then(data => {
    allTeams= data;
});

//EventListeners
document.addEventListener('click', ShowGame);
//Functions

/*ShowAll();*/

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
                                 <p>Correct Odds according to xG: ${chosenGame.correctOdds1} - ${chosenGame.correctOddsX} - ${chosenGame.correctOdds2}<br/>Actual odds on market: <span><strong>${chosenGame.odds1}</strong></span> - ${chosenGame.oddsX} - ${chosenGame.odds2} <br /> Bet: ${chosenGame.whatBetHasValue}<br /> Overvalue: ${chosenGame.betValue}</p>
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