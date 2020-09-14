using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        [HttpPost]
        public async Task<ActionResult<CharacterResponse>> Post(CharacterRequest request)
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

        [HttpGet("{id:Guid}")]
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

        [HttpPut("{id:Guid}/buff")]
        public async Task<ActionResult<CharacterResponse>> PutTemporaryHp(Guid id, [FromBody] TemporaryHpRequest request)
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
                _logger.LogError(e, "Unhandled exception in /characters GET");
                return StatusCode(500, new ErrorResponse(e));
            }
        }
    }
}
