using System;

namespace EventManager.Exceptions.Login
{
    public class AuthenticationFailedException: Exception
    {
        public AuthenticationFailedException(string message = "Authentication failed") : base(message)
        {
            
        }
    }
}