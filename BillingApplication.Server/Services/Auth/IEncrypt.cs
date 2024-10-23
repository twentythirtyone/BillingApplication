﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingApplication.Services.Auth
{
    public interface IEncrypt
    {
        string HashPassword(string password, string salt);
    }
}
