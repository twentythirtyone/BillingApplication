﻿namespace BillingApplication.Services.Auth
{
    public interface IAuth
    {
        public string GenerateJwtToken<T>(T user);
    }
}
