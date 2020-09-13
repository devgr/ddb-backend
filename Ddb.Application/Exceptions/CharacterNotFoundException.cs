using System;

namespace Ddb.Application.Exceptions
{
    public class CharacterNotFoundException : ApplicationException
    {
        public CharacterNotFoundException()
        {
        }

        public CharacterNotFoundException(string message)
            : base(message)
        {
        }

        public CharacterNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
