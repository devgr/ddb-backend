using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ddb.Api.Models;
using Ddb.Application.Abstractions;
using Ddb.Domain.Events;

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
        public async Task<ActionResult<CharacterUpdated>> Post(CharacterRequest request)
        {
            var command = request.ToCommand();
            return await _applicationService.CreateCharacterAsync(command);
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<CharacterUpdated>> Get(Guid id)
        {
            return await _applicationService.GetCharacterAsync(id);
        }
    }
}
