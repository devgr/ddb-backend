using System;
using Ddb.Domain.Models;
using Ddb.Domain.Enums;

namespace Ddb.Domain.Views
{
    public class CharacterView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int MaxHp { get; set; }
        public int CurrentHp { get; set; }
        public int TemporaryHp { get; set; }
        public LifeStatus Status { get; set; }
        public int DeathSavingThrowCount { get; set; }

        public CharacterView(Character character)
        {
            Id = character.Id;
            Name = character.Name;
            MaxHp = character.Hp.MaxHp;
            CurrentHp = character.Hp.CurrentHp;
            TemporaryHp = character.Hp.TemporaryHp;
            Status = character.Hp.Status;
            DeathSavingThrowCount = character.Hp.DeathSavingThrowCount;
        }
    }
}
