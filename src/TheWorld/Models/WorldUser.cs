using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWorld.Models
{                           //IdentityUser has a lot of the user information we already need like username, password, email etc.. and we're just extending properties onto it below
    public class WorldUser : IdentityUser
    {
        public DateTime FirstTrip { get; set; }
    }
}
