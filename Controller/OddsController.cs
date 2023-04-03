using BetValue.Models;
using BetValue.Repos;
using Microsoft.AspNetCore.Mvc;

namespace BetValue.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class OddsController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public OddsController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAllOdds()
        {
            var odds = _unitOfWork.OddsModelRepository.GetOdds();
            return Ok(odds);
        }

        [HttpGet("{id}")]
        public IActionResult GetOdds(int id)
        {
            var odds = _unitOfWork.OddsModelRepository.GetOdds(id);
            return Ok(odds);
        }
        [HttpGet("/GetOddsFromGame/{sign}{gameiId}")]

        public IActionResult GetOddsFromGame(string sign, int gameId)
        {
            var odds = _unitOfWork.OddsModelRepository.GetOddsFromGame(sign, gameId);
            return Ok(odds);
        }

        [HttpPost]
        public IActionResult CreateOdds(OddsModel odds)
        {
            return Ok(odds);
        }

        [HttpPut]
        public IActionResult UpdateOdds(OddsModel odds)
        {
            return Ok(odds);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOdds(int id)
        {
            return Ok();
        }

    }
}
