using System;
using System.ComponentModel.DataAnnotations;
using Ddb.Domain.Enums;
using Ddb.Domain.Commands;

namespace Ddb.Api.Models
{
    public class DamageRequest
    {
        [Required]
        public DamageTypes DamageType { get; set; }

        [Required]
        [Range(0, Int32.MaxValue)]
        public int Hp { get; set; }

        public DealDamage ToCommand()
        {
            return new DealDamage
            {
                DamageType = DamageType,
                Hp = Hp
            };
        }
    }
}
