using System;
using System.Collections.Generic;
using Ddb.Domain.Enums;

namespace Ddb.Domain.Models
{
    public class Character
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public HitPoints Hp { get; set; }

        // For this initial implementation, the character's classes only matter
        // when calculating the initial hit points, so they are not kept here.

        public Character(string name, HitPoints hp)
        {
            Id = Guid.NewGuid();
            Name = name;
            Hp = hp;
        }
    }
}
