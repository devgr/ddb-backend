using System;
using System.Linq;
using System.Collections.Generic;
using Ddb.Domain.Models;
using Ddb.Domain.Commands;
using Ddb.Domain.Enums;

namespace Ddb.Domain.Services
{
    public class CharacterFactory
    {
        public static Character New(CreateCharacter command)
        {
            int constitutionMod = StatService.CalcCurrentModifier(StatNames.Constitution, command.Stats.Constitution, command.Items);
            int hp = HitDiceService.CalculateInitialHP(command.Classes, constitutionMod);
            var hitPoints = new HitPoints(hp);
            if (command.Defenses != null)
            {
                AddDamageModifiers(hitPoints, command.Defenses);
            }
            if (command.Items != null)
            {
                var itemDefenses = command.Items
                    .Where((x) => x.Defenses != null)
                    .SelectMany((x) => x.Defenses);
                AddDamageModifiers(hitPoints, itemDefenses);
            }
            return new Character(command.Name, hitPoints);
        }

        private static void AddDamageModifiers(HitPoints hitPoints, IEnumerable<CreateDefense> defenses)
        {
            foreach (var defense in defenses)
            {
                switch (defense.Defense)
                {
                    case DamageModifierTypes.Immunity:
                        hitPoints.AddImmunity(defense.Type);
                        break;
                    case DamageModifierTypes.Resistance:
                        hitPoints.AddResistance(defense.Type);
                        break;
                    case DamageModifierTypes.Vulnerability:
                        hitPoints.AddVulnerability(defense.Type);
                        break;
                }
            }
        }
    }
}
