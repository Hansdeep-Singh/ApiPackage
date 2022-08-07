using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiWeb.Models
{
    public class TheToken
    {
        public Guid TheTokenId { get; set; } = Guid.NewGuid();
        [ForeignKey("User")]
        public Guid UserId { get; set;} = Guid.NewGuid();
        public string RefreshToken { get; set;}
        public DateTime IssueDate { get; set; } = DateTime.Now;
        public DateTime ExpireDate { get; set; } = DateTime.Now.AddDays(1);
        public virtual User User { get; set;}
    }
}
