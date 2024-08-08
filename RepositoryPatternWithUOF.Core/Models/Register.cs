using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Models
{
    public class Register
    {
        [Required, StringLength(100)]
        public string FirtName { get; set; }
        [Required, StringLength(100)]
        public string LastName { get; set; }
        [Required, StringLength(50)]
        public string UserName { get; set; }
        [Required, StringLength(150)]
        public string Email { get; set; }
        [Required, StringLength(200)]
        public string Password { get; set; }


    }
}
