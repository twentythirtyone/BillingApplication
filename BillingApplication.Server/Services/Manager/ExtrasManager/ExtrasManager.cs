using BillingApplication.Server.DataLayer.Repositories.Abstractions;
using BillingApplication.Services.Models.Utilites;

namespace BillingApplication.Server.Services.Manager.ExtrasManager
{
    public class ExtrasManager : IExtrasManager
    {
        private readonly IExtrasRepository extrasRepository;
        public ExtrasManager(IExtrasRepository extrasRepository)
        {
            this.extrasRepository = extrasRepository;
        }
        public async Task<int?> AddNewExtra(Extras extras)
        {
            int? id = await extrasRepository.Create(extras);
            return id ?? throw new Exception("Create extra exception");
        }

        public async Task<int?> Delete(int extrasId)
        {
            return await extrasRepository.Delete(extrasId) ?? throw new Exception("Delete extra exception");
        }

        public async Task<Dictionary<string, int>> GetBoughtExtrasCurrentMonthCount()
        {
            return await extrasRepository.GetBoughtExtrasCurrentMonthCount();
        }

        public async Task<IEnumerable<Extras?>> GetExtras()
        {
            IEnumerable<Extras?> extras = await extrasRepository.Get();
            return extras;
        }

        public async Task<Extras?> GetExtrasById(int extrasId)
        {
            Extras? extras = await extrasRepository.GetById(extrasId);
            return extras ?? throw new Exception("Get extra exception");
        }

        public async Task<int?> Update(Extras extras, int bundleId)
        {
            return await extrasRepository.Update(extras, bundleId) ?? throw new Exception("Update extra excepton");
        }
    }
}
