using System;
using Ddb.Domain.Models;

namespace Ddb.Domain.Views
{
    public class CharacterView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public CharacterView(Character character)
        {
            Id = character.Id;
            Name = character.Name;
        }
    }
}
