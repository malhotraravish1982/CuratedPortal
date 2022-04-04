﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterGenerator.Model.Model
{
    public class UserModel
    {
        public int Id { get; set; }       
        public string FirstName { get; set; }      
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
