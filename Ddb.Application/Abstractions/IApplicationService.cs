using System;
using System.Threading.Tasks;
using Ddb.Domain.Commands;
using Ddb.Domain.Events;

namespace Ddb.Application.Abstractions
{
    public interface IApplicationService
    {
        Task<CharacterUpdated> CreateCharacterAsync(CreateCharacter command);
        Task<CharacterUpdated> GetCharacterAsync(Guid id);
    }
}
