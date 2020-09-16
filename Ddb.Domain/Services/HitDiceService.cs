using System;
using System.Collections.Generic;
using System.Linq;
using Ddb.Domain.Models;
using Ddb.Domain.Commands;

namespace Ddb.Domain.Services
{
    public class HitDiceService
    {
        private readonly DiceRollService _diceRollService;

        public HitDiceService(DiceRollService diceRollService = null)
        {
            _diceRollService = diceRollService != null ? diceRollService : DiceRollService.GetInstance();
        }

        public int CalculateInitialHP(IEnumerable<CreateCharacterClass> classes, int constitutionMod)
        {
            // Assumption: The first class in the classes list was the class that the character started as.

            // First level HP is the max dice roll + const. mod.
            int hp = classes.First().HitDiceValue + constitutionMod;

            foreach (CreateCharacterClass characterClass in classes)
            {
                // start at 2 rather than 1 because first level was calculated above
                for (int i = 2; i <= characterClass.ClassLevel; i++)
                {
                    hp += _diceRollService.RollAverageOrBetter(characterClass.HitDiceValue);
                    hp += constitutionMod;
                }
            }

            return hp;
        }
    }
}
