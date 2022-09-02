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
        public Guid UserId { get; set;}
        public string Token { get; set;}
        public string Type { get; set; }
        public virtual User User { get; set;}
    }
}
