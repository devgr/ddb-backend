using System;
using System.Collections.Generic;
using Ddb.Domain.Enums;

namespace Ddb.Domain.Models
{
    public class HitPoints
    {
        public int MaxHp { get; set; }
        public int CurrentHp { get; set; }
        public int TemporaryHp { get; set; }

        public IList<DamageTypes> Immunities { get; set; }
        public IList<DamageTypes> Resistances { get; set; }
        public IList<DamageTypes> Vulnerabilities { get; set; }
        // Possible future refactor: Rather than simple lists of enums, make a complex object
        // with an id to allow for removing specific immunities / resistances / vulnerabilities.

        public LifeStatus Status { get; set; }
        public int DeathSavingThrowSuccesses { get; set; }
        public int DeathSavingThrowFailures { get; set; }

        public HitPoints(int hp)
        {
            MaxHp = hp;
            CurrentHp = hp;
            TemporaryHp = 0;
            Immunities = new List<DamageTypes>();
            Resistances = new List<DamageTypes>();
            Vulnerabilities = new List<DamageTypes>();
            Status = LifeStatus.Stable;
            DeathSavingThrowSuccesses = 0;
            DeathSavingThrowFailures = 0;
        }

        public void AddImmunity(DamageTypes damageType)
        {
            Immunities.Add(damageType);
        }

        public void AddResistance(DamageTypes damageType)
        {
            Resistances.Add(damageType);
        }

        public void AddVulnerability(DamageTypes damageType)
        {
            Vulnerabilities.Add(damageType);
        }

        public void AddTemporaryHp(int temporaryHp)
        {
            // When new temporary hit points are added, the player can choose to keep their current
            // temporary hp, or take the new temporary hp. The new temporary hp cannot be added
            // to the existing teporary hp. For this implementation, it will choose the largest.
            if (temporaryHp > TemporaryHp)
            {
                TemporaryHp = temporaryHp;
            }
        }

        public void HealHp(int hp)
        {
            // Hit points are added to the current hit points, up to the max.
            // Temporary hit points are not healed.
            // If the character was dying, they are now stable.
            CurrentHp += hp;
            if (CurrentHp > MaxHp)
            {
                CurrentHp = MaxHp;
            }

            if (Status == LifeStatus.Dying)
            {
                Status = LifeStatus.Stable;
                DeathSavingThrowSuccesses = 0;
                DeathSavingThrowFailures = 0;
            }
        }
    }
}
