using System.Collections.Generic;
using Ddb.Domain.Commands;

namespace Ddb.Api.Models
{
    public class CharacterRequest
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public IEnumerable<CharacterClassRequest> Classes { get; set; }
        public StatsRequest Stats { get; set; }
        public IEnumerable<ItemRequest> Items { get; set; }
        public IEnumerable<DefenseRequest> Defenses { get; set; }

        public CreateCharacter ToCommand()
        {
            return new CreateCharacter
            {
                Name = Name
            };
        }
    }

    public class CharacterClassRequest
    {
        public string Name { get; set; }
        public int HitDiceValue { get; set; }
        public int ClassLevel { get; set; }
    }

    public class StatsRequest
    {
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }
    }

    public class ItemRequest
    {
        public string Name { get; set; }
        public ItemModifierRequest Modifier { get; set; }
    }

    public class ItemModifierRequest
    {
        public string AffectedObject { get; set; }
        public string AffectedValue { get; set; }
        public int Value { get; set; }
    }

    public class DefenseRequest
    {
        public string Type { get; set; }
        public string Defense { get; set; }
    }
}
