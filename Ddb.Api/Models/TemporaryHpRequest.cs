using System;
using System.ComponentModel.DataAnnotations;
using Ddb.Domain.Commands;

namespace Ddb.Api.Models
{
    public class TemporaryHpRequest
    {
        [Required]
        [Range(1, Int32.MaxValue)]
        public int TemporaryHp { get; set; }

        public AddTemporaryHp ToCommand()
        {
            return new AddTemporaryHp
            {
                TemporaryHp = TemporaryHp
            };
        }
    }
}
