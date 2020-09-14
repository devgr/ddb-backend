using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ddb.Domain.Commands;
using Ddb.Domain.Enums;

namespace Ddb.Api.Models
{
    public class CharacterRequest
    {
        public string Name { get; set; }
        public int Level { get; set; }

        [Required]
        [MinLength(1)]
        public IEnumerable<CharacterClassRequest> Classes { get; set; }

        [Required]
        public StatsRequest Stats { get; set; }
        public IEnumerable<ItemRequest> Items { get; set; }
        public IEnumerable<DefenseRequest> Defenses { get; set; }

        public CreateCharacter ToCommand()
        {
            // Note: Currently, this is a one-to-one translation to the CreateCharacter command. This
            // request class is used so that the domain model can change in the future, but the API 
            // contract can remain the same.
            // Additional validation could also be added to this request class.
            return new CreateCharacter
            {
                Name = Name
            };
        }
    }

    public class CharacterClassRequest
    {
        public string Name { get; set; }

        [Required]
        [Range(1, Int32.MaxValue)] // hit dice validation is more complicated than this,
        // but this is sufficient for this initial implementation.
        public int HitDiceValue { get; set; }

        [Required]
        [Range(1, Int32.MaxValue)]
        public int ClassLevel { get; set; }
    }

    public class StatsRequest
    {
        // Although stats are meant to be in the range of 0 to 20,
        // the validation isn't needed for this initial implementation.
        [Required]
        public int Strength { get; set; }

        [Required]
        public int Dexterity { get; set; }

        [Required]
        public int Constitution { get; set; }

        [Required]
        public int Intelligence { get; set; }

        [Required]
        public int Wisdom { get; set; }

        [Required]
        public int Charisma { get; set; }
    }

    public class ItemRequest
    {
        public string Name { get; set; }
        // Items can modify stats when equipted
        public ItemModifierRequest Modifier { get; set; }
        // Items can also include defenses when equipted
        public IEnumerable<DefenseRequest> Defenses { get; set; }
    }

    public class ItemModifierRequest
    {
        // In this implementation, AffectedObject is unused because the only
        // object to affect is the Stats.
        public string AffectedObject { get; set; }

        [Required]
        public StatNames AffectedValue { get; set; }

        [Required]
        public int Value { get; set; }
    }

    public class DefenseRequest
    {
        [Required]
        public DamageTypes Type { get; set; }

        [Required]
        public DamageModifierTypes Defense { get; set; }
    }
}
