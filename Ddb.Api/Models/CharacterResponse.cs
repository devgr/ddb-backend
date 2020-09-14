using System;
using Ddb.Domain.Views;
using Ddb.Domain.Enums;

namespace Ddb.Api.Models
{
    public class CharacterResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int MaxHp { get; set; }
        public int CurrentHp { get; set; }
        public int TemporaryHp { get; set; }
        public LifeStatus Status { get; set; }
        public int DeathSavingThrowCount { get; set; }

        public CharacterResponse(CharacterView characterView)
        {
            // Note: Currently, this is a one-to-one translation of the CharacterView. This response
            // class is used so that the domain model can change in the future, but the API contract 
            // can remain the same.
            Id = characterView.Id;
            Name = characterView.Name;
            MaxHp = characterView.MaxHp;
            CurrentHp = characterView.CurrentHp;
            TemporaryHp = characterView.TemporaryHp;
            Status = characterView.Status;
            DeathSavingThrowCount = characterView.DeathSavingThrowCount;
        }
    }
}
