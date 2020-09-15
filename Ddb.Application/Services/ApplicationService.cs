using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Ddb.Application.Abstractions;
using Ddb.Domain.Services;
using Ddb.Domain.Commands;
using Ddb.Domain.Views;

namespace Ddb.Application.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly ICharacterRepository _characterRepository;
        private readonly IEventBus _eventBus;

        public ApplicationService(ICharacterRepository characterRepository, IEventBus eventBus)
        {
            _characterRepository = characterRepository;
            _eventBus = eventBus;
        }

        public async Task<CharacterView> CreateCharacterAsync(CreateCharacter command)
        {
            var character = CharacterFactory.New(command);
            await _characterRepository.SaveAsync(character);
            var characterView = new CharacterView(character);
            await _eventBus.PublishAsync<CharacterView>(characterView);
            return characterView;
        }

        public async Task<CharacterView> GetCharacterAsync(Guid id)
        {
            var character = await _characterRepository.GetByIdAsync(id);
            return new CharacterView(character);
        }

        public async Task<CharacterView> AddTemporaryHpAsync(Guid id, AddTemporaryHp command)
        {
            var character = await _characterRepository.GetByIdAsync(id);
            character.Hp.AddTemporaryHp(command.TemporaryHp);
            await _characterRepository.SaveAsync(character);
            var characterView = new CharacterView(character);
            await _eventBus.PublishAsync<CharacterView>(characterView);
            return characterView;
        }

        public async Task<CharacterView> HealHpAsync(Guid id, HealHp command)
        {
            var character = await _characterRepository.GetByIdAsync(id);
            character.Hp.HealHp(command.Hp);
            await _characterRepository.SaveAsync(character);
            var characterView = new CharacterView(character);
            await _eventBus.PublishAsync<CharacterView>(characterView);
            return characterView;
        }

        public async Task<CharacterView> DealDamageAsync(Guid id, IEnumerable<DealDamage> commands)
        {
            var character = await _characterRepository.GetByIdAsync(id);
            character.Hp.DealDamages(commands);
            await _characterRepository.SaveAsync(character);
            var characterView = new CharacterView(character);
            await _eventBus.PublishAsync<CharacterView>(characterView);
            return characterView;
        }
    }
}
