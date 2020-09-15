using System;

namespace Ddb.Application.Exceptions
{
    public class ApplicationException : Exception
    {
        public ApplicationException()
        {
        }

        public ApplicationException(string message)
            : base(message)
        {
        }

        public ApplicationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
