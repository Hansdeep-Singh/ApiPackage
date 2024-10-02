
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Logic.Extentions;

namespace ApiWeb.ApiBroadcast.Models
{
    public class Email
    {
        private string email = String.Empty;
        public Guid EmailId { get; set; }
        public string? EmailTo { get; set; }
        public string Subject { get; set; } = String.Empty;
        public string Body { get; set; } = String.Empty;
        public string HTMLBody { get; set; } = String.Empty;
        public string FromName { get; set; } = String.Empty;
        public string FromEmailAddress
        {
            get
            {
                return email.ValidEmail();
            }
            set => email = value;
        }
        public ValidEmailAddress[] ToEmailAddresses { get; set; } = Array.Empty<ValidEmailAddress>();

    }
    public class ValidEmailAddress
    {
        private string email = String.Empty;
        public string? EmailAddress
        {
            get
            {
                return email.ValidEmail();
            }
            set => email = value!;
        }
    }
}
