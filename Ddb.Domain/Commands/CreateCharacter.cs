using System;
using System.Collections.Generic;
using Ddb.Domain.Enums;

namespace Ddb.Domain.Commands
{
    public class CreateCharacter
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public IEnumerable<CreateCharacterClass> Classes { get; set; }
        public CreateStats Stats { get; set; }
        public IEnumerable<CreateItem> Items { get; set; }
        public IEnumerable<CreateDefense> Defenses { get; set; }
    }

    public class CreateCharacterClass
    {
        public string Name { get; set; }
        public int HitDiceValue { get; set; }
        public int ClassLevel { get; set; }
    }

    public class CreateStats
    {
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }
    }

    public class CreateItem
    {
        public string Name { get; set; }
        public CreateItemModifier Modifier { get; set; }
        public IEnumerable<CreateDefense> Defenses { get; set; }
    }

    public class CreateItemModifier
    {
        public string AffectedObject { get; set; }
        public StatNames AffectedValue { get; set; }
        public int Value { get; set; }
    }

    public class CreateDefense
    {
        public DamageTypes Type { get; set; }
        public DamageModifierTypes Defense { get; set; }
    }

}
