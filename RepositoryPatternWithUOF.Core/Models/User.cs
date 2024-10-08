﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Models
{
    public class User : IdentityUser
    {
        [Required, MaxLength(50)]
        public string? FirstName { get; set; }
        [Required,MaxLength(50)]
        public string? LastName { get; set; }

    }
}
