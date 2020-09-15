using System;
using System.Collections.Generic;
using System.Linq;
using Ddb.Domain.Models;
using Ddb.Domain.Commands;

namespace Ddb.Domain.Services
{
    public class HitDiceService
    {
        public static int CalculateInitialHP(IEnumerable<CreateCharacterClass> classes, int constitutionMod)
        {
            // Assumption: The first class in the classes list was the class that the character started as.

            var roller = DiceRollService.GetInstance();

            // First level HP is the max dice roll + const. mod.
            int hp = classes.First().HitDiceValue + constitutionMod;

            foreach (CreateCharacterClass characterClass in classes)
            {
                for (int i = 0; i < characterClass.ClassLevel; i++)
                {
                    hp += roller.RollAverageOrBetter(characterClass.HitDiceValue);
                    hp += constitutionMod;
                }
            }

            return hp;
        }
    }
}
