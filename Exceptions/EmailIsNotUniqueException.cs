using System;

namespace Exceptions
{
    public class EmailIsNotUniqueException : Exception
    {
        
        public EmailIsNotUniqueException(string name, object key)
            : base($"Entity {name} already registered with the E-mail {key}.")
        {
            
        }
    }
}