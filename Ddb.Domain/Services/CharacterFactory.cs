using System;
using Ddb.Domain.Models;
using Ddb.Domain.Commands;

namespace Ddb.Domain.Services
{
    public class CharacterFactory
    {
        public static Character New(CreateCharacter command)
        {
            return new Character
            {
                Id = Guid.NewGuid(),
                Name = command.Name
            };
        }
    }
}
