using System;
using System.Threading.Tasks;
using Ddb.Domain.Models;

namespace Ddb.Application.Abstractions
{
    public interface ICharacterRepository
    {
        Task<Character> GetByIdAsync(Guid id);
        Task SaveAsync(Character character);
    }
}
