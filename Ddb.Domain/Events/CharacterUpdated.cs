using System;
using Ddb.Domain.Models;

namespace Ddb.Domain.Events
{
    public class CharacterUpdated
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public CharacterUpdated(Character character)
        {
            Id = character.Id;
            Name = character.Name;
        }
    }
}
