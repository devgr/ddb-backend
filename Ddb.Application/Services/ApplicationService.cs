using System;
using System.Threading.Tasks;
using Ddb.Application.Abstractions;
using Ddb.Domain.Services;
using Ddb.Domain.Commands;
using Ddb.Domain.Events;

namespace Ddb.Application.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly ICharacterRepository _characterRepository;

        public ApplicationService(ICharacterRepository characterRepository)
        {
            _characterRepository = characterRepository;
        }

        public async Task<CharacterUpdated> CreateCharacterAsync(CreateCharacter command)
        {
            var character = CharacterFactory.New(command);
            await _characterRepository.SaveAsync(character);
            return new CharacterUpdated(character);
        }

        public async Task<CharacterUpdated> GetCharacterAsync(Guid id)
        {
            var character = await _characterRepository.GetByIdAsync(id);
            return new CharacterUpdated(character);
        }
    }
}
