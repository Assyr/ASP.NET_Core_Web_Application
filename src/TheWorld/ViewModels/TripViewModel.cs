﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWorld.ViewModels
{
    public class TripViewModel
    {
        [Required]
        [StringLength(100, MinimumLength = 4)]
        public string Name { get; set; }      //Default is now unless overriden
        public DateTime Created { get; set; } = DateTime.UtcNow;

    }
}