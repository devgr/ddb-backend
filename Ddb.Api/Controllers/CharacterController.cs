using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Ddb.Api.Models;
using Ddb.Application.Abstractions;
using Ddb.Application.Exceptions;
using Ddb.Domain.Views;

namespace Ddb.Api.Controllers
{
    [ApiController]
    [Route("characters")]
    public class CharacterController : ControllerBase
    {
        private readonly ILogger<CharacterController> _logger;
        private readonly IApplicationService _applicationService;

        public CharacterController(ILogger<CharacterController> logger, IApplicationService applicationService)
        {
            _logger = logger;
            _applicationService = applicationService;
        }

        /// <summary>
        /// Create a new Player Character. Hit Points will be auto-calculated
        /// based on the classes and levels provided.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /characters
        ///     {
        ///         "name": "Briv",
        ///         "level": 5,
        ///         "classes": [
        ///             {
        ///                 "name": "fighter",
        ///                 "hitDiceValue": 10,
        ///                 "classLevel": 3
        ///             },
        ///             {
        ///                 "name": "wizard",
        ///                 "hitDiceValue": 6,
        ///                 "classLevel": 2
        ///             }
        ///         ],
        ///         "stats": {
        ///             "strength": 15,
        ///             "dexterity": 12,
        ///             "constitution": 10,
        ///             "intelligence": 13,
        ///             "wisdom": 10,
        ///             "charisma": 8
        ///         },
        ///         "items": [
        ///             {
        ///                 "name": "Ioun Stone of Fortitude",
        ///                 "modifier": {
        ///                     "affectedObject": "stats",
        ///                     "affectedValue": "constitution",
        ///                     "value": 6
        ///                 },
        ///                 "defenses": [
        ///                     {
        ///                         "type": "cold",
        ///                         "defense": "vulnerability"
        ///                     }
        ///                 ]
        ///             }
        ///         ],
        ///         "defenses": [
        ///             {
        ///                 "type": "fire",
        ///                 "defense": "immunity"
        ///             },
        ///             {
        ///                 "type": "slashing",
        ///                 "defense": "resistance"
        ///             }
        ///         ]
        ///     }
        /// </remarks>
        /// <param name="request"></param>
        /// <returns>Info about the newly created character, including its Id and HP</returns>
        /// <response code="200">Returns the character info</response>
        /// <response code="400">Schema validation failed, check the request body again</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CharacterResponse>> Post([FromBody]CharacterRequest request)
        {
            try
            {
                var command = request.ToCommand();
                var view = await _applicationService.CreateCharacterAsync(command);
                return new CharacterResponse(view);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unhandled exception in /characters POST");
                return StatusCode(500, new ErrorResponse(e));
            }
        }

        /// <summary>
        /// Get info about the specified character, by id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /characters/{id}
        /// </remarks>
        /// <param name="id">Guid ID of the character</param>
        /// <returns>Info about the character's HP</returns>
        /// <response code="200">Returns the character info</response>
        /// <response code="404">Invalid ID, character not found</response>
        [HttpGet("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CharacterResponse>> Get(Guid id)
        {
            try
            {
                var view = await _applicationService.GetCharacterAsync(id);
                return new CharacterResponse(view);
            }
            catch (CharacterNotFoundException e)
            {
                return NotFound(new ErrorResponse(e));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unhandled exception in /characters GET");
                return StatusCode(500, new ErrorResponse(e));
            }
        }

        /// <summary>
        /// Give the character temporary hit points.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /characters/{id}/buff
        ///     {
        ///         "temporaryHp": 20
        ///     }
        /// </remarks>
        /// <param name="id">Guid ID of the character</param>
        /// <param name="request">Temporary hit points</param>
        /// <returns>Info about the character's HP</returns>
        /// <response code="200">Temporary hit points successfully applied</response>
        /// <response code="400">Schema validation failed, check the request body again</response>
        /// <response code="404">Invalid ID, character not found</response>
        [HttpPut("{id:Guid}/buff")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CharacterResponse>> PutTemporaryHp(Guid id, [FromBody]TemporaryHpRequest request)
        {
            try
            {
                var command = request.ToCommand();
                var view = await _applicationService.AddTemporaryHpAsync(id, command);
                return new CharacterResponse(view);
            }
            catch (CharacterNotFoundException e)
            {
                return NotFound(new ErrorResponse(e));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unhandled exception in /characters/<id>/buff PUT");
                return StatusCode(500, new ErrorResponse(e));
            }
        }

        /// <summary>
        /// Heal the character for a certain number of hit points.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /characters/{id}/heal
        ///     {
        ///         "hp": 4
        ///     }
        /// </remarks>
        /// <param name="id">Guid ID of the character</param>
        /// <param name="request">Healing hit points</param>
        /// <returns>Info about the character's HP</returns>
        /// <response code="200">Healing hit points successfilly applied</response>
        /// <response code="400">Schema validation failed, check the request body again</response>
        /// <response code="404">Invalid ID, character not found</response>
        [HttpPut("{id:Guid}/heal")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CharacterResponse>> PutHealing(Guid id, [FromBody]HealingRequest request)
        {
            try
            {
                var command = request.ToCommand();
                var view = await _applicationService.HealHpAsync(id, command);
                return new CharacterResponse(view);
            }
            catch (CharacterNotFoundException e)
            {
                return NotFound(new ErrorResponse(e));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unhandled exception in /characters/<id>/heal PUT");
                return StatusCode(500, new ErrorResponse(e));
            }
        }

        /// <summary>
        /// Hit the character with different types of damage
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /characters/{id}/damage
        ///     [
        ///         {
        ///             "damageType": "slashing",
        ///             "hp": 10
        ///         },
        ///         {
        ///             "damageType": "cold",
        ///             "hp": 2
        ///         },
        ///         {
        ///             "damageType": "fire",
        ///             "hp": 7
        ///         }
        ///     ]
        /// </remarks>
        /// <param name="id">Guid ID of the character</param>
        /// <param name="requests">Array of damage types with hit points</param>
        /// <returns>Info about the character's HP</returns>
        /// <response code="200">Damage successfilly applied</response>
        /// <response code="400">Schema validation failed, check the request body again</response>
        /// <response code="404">Invalid ID, character not found</response>
        [HttpPut("{id:Guid}/damage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CharacterResponse>> PutDamage(Guid id, [FromBody]IEnumerable<DamageRequest> requests)
        {
            try
            {
                var commands = requests.Select(x => x.ToCommand());
                var view = await _applicationService.DealDamageAsync(id, commands);
                return new CharacterResponse(view);
            }
            catch (CharacterNotFoundException e)
            {
                return NotFound(new ErrorResponse(e));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unhandled exception in /characters/<id>/damage PUT");
                return StatusCode(500, new ErrorResponse(e));
            }
        }
    }
}
