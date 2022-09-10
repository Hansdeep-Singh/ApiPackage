using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Efficacy
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ApiSecurity : Attribute
    {
        public ApiAccess Access { get; set; }
        public ApiAuth Authentication { get; set; }
        public string[] Roles { get; set; }

        public ApiSecurity(ApiAccess access, ApiAuth authentication, string[] roles)
        {
            Access = access;
            Authentication = authentication;
            Roles = roles;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiAuthoriseSecurity : AuthorizeAttribute
    {
        const string POLICY_PREFIX = "MinimumAge";

        public ApiAuthoriseSecurity(int age) => Age = age;

        // Get or set the Age property by manipulating the underlying Policy property
        public int Age
        {
            get
            {
                if (int.TryParse(Policy.Substring(POLICY_PREFIX.Length), out var age))
                {
                    return age;
                }
                return default(int);
            }
            set
            {
                Policy = $"{POLICY_PREFIX}{value.ToString()}";
            }
        }
    }

    public enum ApiAccess
    {
        External,
        Internal
    }

    public enum ApiAuth
    {
        UnAuthenticated,
        Authenticated
    }
}
