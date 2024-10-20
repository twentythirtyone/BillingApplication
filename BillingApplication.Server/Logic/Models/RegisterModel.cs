﻿using BillingApplication.Models;

namespace BillingApplication.Logic.Models
{
    public class RegisterModel
    {
        public Subscriber User { get; set; } = new Subscriber { Number = "+7 000 000 00 00" };
        public PassportInfo Passport { get; set; } = new PassportInfo();
        public Tariff Tariff { get; set; } = new Tariff { Id = 1 , Title = "Default"};
    }
}

