using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace EventManager.Exceptions.Login
{
    public class JwtTokenCreationException: Exception
    {
        private readonly IEnumerable<IdentityError> _errors;
        
        public JwtTokenCreationException(string message = "Token creation failed", IEnumerable<IdentityError> errors = null): base(message)
        {
            _errors = errors;
        }

        public IEnumerable<IdentityError> Errors => _errors;
    }
}