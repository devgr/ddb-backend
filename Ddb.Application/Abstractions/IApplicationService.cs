using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Ddb.Domain.Commands;
using Ddb.Domain.Views;

namespace Ddb.Application.Abstractions
{
    public interface IApplicationService
    {
        Task<CharacterView> CreateCharacterAsync(CreateCharacter command);
        Task<CharacterView> GetCharacterAsync(Guid id);
        Task<CharacterView> AddTemporaryHpAsync(Guid id, AddTemporaryHp command);
        Task<CharacterView> HealHpAsync(Guid id, HealHp command);
        Task<CharacterView> DealDamageAsync(Guid id, IEnumerable<DealDamage> commands);
    }
}
