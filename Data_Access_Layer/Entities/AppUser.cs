﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Data_Access_Layer.Entities
{
    public class AppUser: IdentityUser
    {
        [Required]
        public string Name { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber {  get; set; }

        public virtual ShoppingCart ShoppingCart { get; set; }
    }
}
