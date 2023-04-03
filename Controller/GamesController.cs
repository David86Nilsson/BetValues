using BetValue.Models;
using BetValue.Repos;
using Microsoft.AspNetCore.Mvc;

namespace BetValue.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        public UnitOfWork unitOfWork;

        public GamesController(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameModel>>> GetGamesAsync()
        {
            return await unitOfWork.GameModelRepository.GetGamesAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GameModel>> GetGame(int id)
        {
            var game = await unitOfWork.GameModelRepository.GetGameAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            return game;
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutGame(int id, GameModel game)
        //{
        //    if (id != game.Id)
        //    {
        //        return BadRequest();
        //    }

        //    unitOfWork.GameModelRepository.UpdateGame(game);

        //    try
        //    {
        //        await unitOfWork.SaveAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!GameExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //[HttpPost]
        //public async Task<ActionResult<GameModel>> PostGame(GameModel game)
        //{
        //    unitOfWork.GameModelRepository.AddGame(game);
        //    await unitOfWork.SaveAsync();

        //    return CreatedAtAction("GetGame", new { id = game.Id }, game);
        //}

        //[HttpDelete("{id}")]
        //public async Task<ActionResult<GameModel>> DeleteGame(int id)
        //{
        //    var game = await unitOfWork.GameModelRepository.GetGameAsync(id);
        //    if (game == null)
        //    {
        //        return NotFound();
        //    }

        //    unitOfWork.GameModelRepository.DeleteGame(game);
        //    await unitOfWork.SaveAsync();

        //    return game;
        //}

        //private bool GameExists(int id)
        //{
        //    return unitOfWork.GameModelRepository.GameExists(id);
        //}


    }
}
