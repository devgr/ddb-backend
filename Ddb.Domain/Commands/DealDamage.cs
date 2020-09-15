using System;
using Ddb.Domain.Enums;

namespace Ddb.Domain.Commands
{
    public class DealDamage
    {
        public DamageTypes DamageType { get; set; }
        public int Hp { get; set; }
    }
}
