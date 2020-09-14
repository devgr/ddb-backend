using System;
using System.Threading.Tasks;
using Ddb.Domain.Commands;
using Ddb.Domain.Views;

namespace Ddb.Application.Abstractions
{
    public interface IApplicationService
    {
        Task<CharacterView> CreateCharacterAsync(CreateCharacter command);
        Task<CharacterView> GetCharacterAsync(Guid id);
        Task<CharacterView> AddTemporaryHpAsync(Guid id, AddTemporaryHp command);
    }
}
