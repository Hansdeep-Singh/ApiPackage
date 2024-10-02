using ApiWeb.ApiBroadcast.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiWeb.ApiBroadcast.Efficacy
{
    public static class Utils
    {
        public static ValidEmailAddress[] EmailArrayToEmailAddresses(string[] emails)
        {
            List<ValidEmailAddress> addresses = new();
            foreach (var email in emails)
            {
                ValidEmailAddress validEmailAddress = new()
                {
                    EmailAddress = email
                };
                addresses.Add(validEmailAddress);
            }
            ValidEmailAddress[] validEmailAddresses = addresses.ToArray();
            return validEmailAddresses;
        }
    }
}
