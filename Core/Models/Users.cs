using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Users : IdentityUser
    {
        [Required, MaxLength(50)]
        public string? Firstname { get; set; }

        [Required, MaxLength(50)]
        public string? Lastname { get; set; }
    }
}
