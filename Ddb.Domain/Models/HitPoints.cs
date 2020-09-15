using System;
using System.Collections.Generic;
using System.Linq;
using Ddb.Domain.Enums;
using Ddb.Domain.Commands;

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

        public void AddDeathSavingThrowFailure()
        {
            DeathSavingThrowFailures++;
            if (DeathSavingThrowFailures >= 3)
            {
                Status = LifeStatus.Dead;
            }
        }

        public void AddTemporaryHp(int temporaryHp)
        {
            if (Status != LifeStatus.Dead) // You can get temp hp while dying, but not while dead
            {
                // When new temporary hit points are added, the player can choose to keep their current
                // temporary hp, or take the new temporary hp. The new temporary hp cannot be added
                // to the existing teporary hp. For this implementation, it will choose the largest.
                if (temporaryHp > TemporaryHp)
                {
                    TemporaryHp = temporaryHp;
                }
            }
        }

        public void HealHp(int hp)
        {
            if (Status != LifeStatus.Dead) // You can't resurrect characters with normal healing
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

        public void DealDamages(IEnumerable<DealDamage> damages)
        {
            if (Status != LifeStatus.Dead) // Already dead!
            {
                // Add up the hp for each type of damage
                int totalHpDamage = 0;
                foreach (var damage in damages)
                {
                    totalHpDamage += CalcDamage(damage);
                }

                ApplyHpDamage(totalHpDamage);
            }
        }

        private int CalcDamage(DealDamage damage)
        {
            // First check for immunities, then resistance, then vulnerability (p. 197 Player's Handbook)
            if (Immunities.Contains(damage.DamageType))
            {
                return 0;
            }

            int actualHpDamage = damage.Hp;
            // Resistances and vulnerabilities do not stack, so one does as much as multiple.
            if (Resistances.Contains(damage.DamageType))
            {
                actualHpDamage /= 2; // integer division correctly rounds down
            }

            // It is technically possible to have a resistance and a vulnerability for the same damage type.
            // https://rpg.stackexchange.com/a/167448
            if (Vulnerabilities.Contains(damage.DamageType))
            {
                actualHpDamage *= 2;
            }

            return actualHpDamage;
            // Possible future refactor: It's a little bit inefficient to check all of the
            // lists every time, especially since this function is called from in a loop.
        }

        private void ApplyHpDamage(int totalHpDamage)
        {
            // Subtract damage from temporary hit points first
            if (totalHpDamage <= TemporaryHp)
            {
                TemporaryHp -= totalHpDamage;
                return; // No actual damage done to character!
            }

            totalHpDamage -= TemporaryHp;
            TemporaryHp = 0;

            // If character is already dying, any damage counts as 1 failure
            if (Status == LifeStatus.Dying)
            {
                // If damage meets or exceeds max hp, then character is dead
                if (totalHpDamage >= MaxHp)
                {
                    Status = LifeStatus.Dead;
                    return;
                }

                AddDeathSavingThrowFailure();
                // Out of scope for initial implementation: Critical hit counts as 2 failures
                return;
            }

            if (totalHpDamage < CurrentHp)
            {
                CurrentHp -= totalHpDamage;
                return; // Normal damage: "I'm not dead yet!"
            }

            // Otherwise, this brings the character down to 0 hp
            totalHpDamage -= CurrentHp;
            CurrentHp = 0;
            // Check for insta-kill
            if (totalHpDamage >= MaxHp)
            {
                Status = LifeStatus.Dead;
                return; // super dead
            }

            // Otherwise, just dying
            Status = LifeStatus.Dying;
        }
    }
}
