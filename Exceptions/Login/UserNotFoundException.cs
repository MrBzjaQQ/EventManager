using System;

namespace EventManager.Exceptions.Login
{
    public class UserNotFoundException: Exception
    {
        public UserNotFoundException(string message = "User not found") : base(message)
        {
            
        }
    }
}