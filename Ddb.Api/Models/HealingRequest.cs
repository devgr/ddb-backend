using System;
using System.ComponentModel.DataAnnotations;
using Ddb.Domain.Commands;

namespace Ddb.Api.Models
{
    public class HealingRequest
    {
        [Required]
        [Range(1, Int32.MaxValue)]
        public int Hp { get; set; }

        public HealHp ToCommand()
        {
            return new HealHp
            {
                Hp = Hp
            };
        }
    }
}
