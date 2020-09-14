using System;
using System.Collections.Generic;
using System.Linq;
using Ddb.Domain.Commands;
using Ddb.Domain.Enums;

namespace Ddb.Domain.Services
{
    public class StatService
    {
        public static int CalcModifier(int statValue)
        {
            // Modifier is (statValue - 10) / 2 rounded to the lowest integer.
            // For negative numbers, round to the integer further from zero.
            int mod = (statValue - 10) / 2;
            if ((statValue - 10) % 2 == -1)
            {
                mod -= 1;
            }
            return mod;
        }

        public static int CalcCurrentModifier(StatNames stat, int statValue, IEnumerable<CreateItem> items)
        {
            // Based on the equipted items and given an initial stat value,
            // returns the effective stat modifier.
            int effectiveStatValue = statValue;
            if (items != null)
            {
                effectiveStatValue += items
                    .Where((x) => x.Modifier != null && x.Modifier.AffectedValue == stat)
                    .Select((x) => x.Modifier.Value)
                    .Sum();
            }
            int mod = CalcModifier(effectiveStatValue);
            return mod;
            // A future refactor might involve tracking the effective stat values
            // as items are equipted and un-equipted.
        }
    }
}
