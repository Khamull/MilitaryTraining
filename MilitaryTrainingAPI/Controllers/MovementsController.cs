using BLL.Services;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using MilitaryTrainingAPI.DTO;

namespace MilitaryTrainingAPI.Controllers
{
    [ApiController]  // Este atributo informa ao ASP.NET Core que o controller responde a requisições HTTP
    [Route("api/[controller]")]
    public class MovementsController : ControllerBase
    {
        private readonly MovementService _movementService;
        private readonly ILogger<MovementsController> _logger;

        public MovementsController(MovementService movementService, ILogger<MovementsController> logger)
        {
            _movementService = movementService;
            _logger = logger;
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Post([FromBody] List<MovimentsDTO> movements)
        {
            if (movements == null || !movements.Any())
            {
                return BadRequest("Movements data is null or empty.");
            }

            try
            {
                // Converte MovementsDTO para DAL.Movement
                var movementsList = movements.Select(dto => new Movement
                {
                    Id = dto.Id,
                    UnitId = dto.UnitId,
                    Latitude = dto.Latitude,
                    Longitude = dto.Longitude,
                    Timestamp = dto.Timestamp
                    , Status = dto.Status
                }).ToList();

                _movementService.AddMovements(movementsList);
                return Ok("Movements saved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving movements.");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                // Retrieve all movements from the service layer
                var movements = _movementService.GetAllMovements();

                var movementsDto = movements.Select(m => new MovimentsDTO
                {
                    Id = m.Id,
                    UnitId = m.UnitId,
                    Latitude = m.Latitude,
                    Longitude = m.Longitude,
                    Timestamp = m.Timestamp
                    , Status = m.Status
                }).ToList();

                if (movementsDto == null || !movementsDto.Any())
                {
                    return NotFound("No movements found.");
                }

                return Ok(movementsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving movements.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}


