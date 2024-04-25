using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo.AuthModels
{
    public class Register
    {
        [Required, MaxLength(50)]
        public string? Firstname { get; set; }

        [Required, MaxLength(50)]
        public string? Lastname { get; set; }

        [Required, MaxLength(100)]
        public string? Username { get; set; }

        [Required, MaxLength(150)]
        public string? Email { get; set; }

        [Required, MaxLength(50)]
        public string? Password { get; set; }
    }
}
