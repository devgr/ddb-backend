using System;

namespace Ddb.Domain.Enums
{
    public enum StatNames
    {
        Strength,
        Dexterity,
        Constitution,
        Intelligence,
        Wisdom,
        Charisma
    }

    public enum DamageTypes
    {
        Slashing,
        Piercing,
        Bludgeoning,
        Poison,
        Acid,
        Fire,
        Cold,
        Radiant,
        Necrotic,
        Lightning,
        Thunder,
        Force,
        Psychic,
    }

    public enum DamageModifierTypes
    {
        Resistance,
        Immunity,
        Vulnerability
    }

    public enum LifeStatus
    {
        Stable,
        Dying,
        Dead
    }
}
