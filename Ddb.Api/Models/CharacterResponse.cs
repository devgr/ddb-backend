using System;
using Ddb.Domain.Views;

namespace Ddb.Api.Models
{
    public class CharacterResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public CharacterResponse(CharacterView characterView)
        {
            // Note: Currently, this is a one-to-one translation of the CharacterView. This response
            // class is used so that the domain model can change in the future, but the API contract 
            // can remain the same.
            Id = characterView.Id;
            Name = characterView.Name;
        }
    }
}
