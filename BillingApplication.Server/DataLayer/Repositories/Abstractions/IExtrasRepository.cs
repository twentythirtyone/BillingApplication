﻿using BillingApplication.Services.Models.Utilites;

namespace BillingApplication.Server.DataLayer.Repositories.Abstractions
{
    public interface IExtrasRepository
    {
        Task<Extras?> GetById(int? id);
        Task<IEnumerable<Extras?>> Get();
        Task<int?> Create(Extras extras);
        Task<int?> Update(Extras extras, int bundleId);
        Task<int?> Delete(int extraId);
        Task<Dictionary<string, int>> GetBoughtExtrasCurrentMonthCount();
    }
}
