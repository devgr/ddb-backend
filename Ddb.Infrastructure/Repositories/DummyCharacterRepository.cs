using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Ddb.Application.Abstractions;
using Ddb.Domain.Models;

namespace Ddb.Infrastructure.Repositories
{
    public class DummyCharacterRepository : ICharacterRepository
    {
        // Dummy in-memory repository. Characters are stored in a Dictionary by their Guid Id.
        private readonly Dictionary<Guid, Character> _characterCollection = new Dictionary<Guid, Character>();

        public async Task<Character> GetByIdAsync(Guid id)
        {
            // Dummy async wrapper for the sync methods. In a real implementation, this might
            // make an asyncronous query to a database, for example.
            return await Task.Run(() => GetById(id));
        }

        public async Task SaveAsync(Character character)
        {
            await Task.Run(() => Save(character));
        }

        public Character GetById(Guid id)
        {
            // TODO: Check for existance, then throw application exception
            return _characterCollection[id];
        }

        public void Save(Character character)
        {
            _characterCollection[character.Id] = character;
        }
    }
}
