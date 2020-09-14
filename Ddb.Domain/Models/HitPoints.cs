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
        public int DeathSavingThrowCount { get; set; }

        public HitPoints(int hp)
        {
            MaxHp = hp;
            CurrentHp = hp;
            TemporaryHp = 0;
            Immunities = new List<DamageTypes>();
            Resistances = new List<DamageTypes>();
            Vulnerabilities = new List<DamageTypes>();
            Status = LifeStatus.Stable;
            DeathSavingThrowCount = 0;
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
    }
}
