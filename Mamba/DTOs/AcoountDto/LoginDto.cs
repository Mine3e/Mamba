﻿using System.ComponentModel.DataAnnotations;

namespace Mamba.DTOs.AcoountDto
{
    public class LoginDto
    {
        [Required]
        public string UsernameOrEmail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsRemembered { get; set; }
    }
}
