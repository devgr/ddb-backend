using System;
using Ddb.Domain.Models;
using Ddb.Domain.Enums;
using Ddb.Domain.Commands;
using Ddb.Domain.Services;

namespace Ddb.Tests.DomainTests
{
    /// <summary>
    /// Helper methods for creating various commands for different test scenarios.
    /// </summary>
    public class CreateCharacterCommandFactory
    {
        public static CreateCharacter SimpleCharacter(int constitution = 10)
        {
            return new CreateCharacter
            {
                Name = "SomeName",
                Classes = new CreateCharacterClass[]
                {
                    new CreateCharacterClass { HitDiceValue = 10, ClassLevel = 3 }
                },
                Stats = new CreateStats
                {
                    Constitution = constitution
                }
            };
        }

        public static CreateCharacter CharacterWithItemConstitutionBonus()
        {
            return new CreateCharacter
            {
                Name = "SomeName",
                Classes = new CreateCharacterClass[]
                {
                    new CreateCharacterClass { HitDiceValue = 10, ClassLevel = 3 }
                },
                Stats = new CreateStats
                {
                    Constitution = 10
                },
                Items = new CreateItem[]
                {
                    new CreateItem
                    {
                        Modifier = new CreateItemModifier
                        {
                            AffectedValue = StatNames.Constitution,
                            Value = 2
                        }
                    }
                }
            };
        }

        public static CreateCharacter MultiClassCharacter()
        {
            return new CreateCharacter
            {
                Name = "SomeName",
                Classes = new CreateCharacterClass[]
                {
                    new CreateCharacterClass { HitDiceValue = 10, ClassLevel = 3 },
                    new CreateCharacterClass { HitDiceValue = 6, ClassLevel = 4 },
                    new CreateCharacterClass { HitDiceValue = 8, ClassLevel = 10 },
                },
                Stats = new CreateStats
                {
                    Constitution = 10
                }
            };
        }

        public static CreateCharacter CharacterWithDamageModifiers()
        {
            return new CreateCharacter
            {
                Name = "SomeName",
                Classes = new CreateCharacterClass[]
                {
                    new CreateCharacterClass { HitDiceValue = 10, ClassLevel = 3 }
                },
                Stats = new CreateStats
                {
                    Constitution = 10
                },
                Items = new CreateItem[]
                {
                    new CreateItem
                    {
                        Defenses = new CreateDefense[]
                        {
                            new CreateDefense { Type = DamageTypes.Fire, Defense = DamageModifierTypes.Resistance},
                            new CreateDefense { Type = DamageTypes.Cold, Defense = DamageModifierTypes.Vulnerability}
                        }
                    }
                },
                Defenses = new CreateDefense[]
                {
                    new CreateDefense { Type = DamageTypes.Lightning, Defense = DamageModifierTypes.Immunity},
                    new CreateDefense { Type = DamageTypes.Fire, Defense = DamageModifierTypes.Resistance}
                }
            };
        }
    }
}
