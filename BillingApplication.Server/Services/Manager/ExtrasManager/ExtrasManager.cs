using BillingApplication.DataLayer.Repositories;
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
            return id ?? throw new Exception("extras_create_exception");
        }

        public async Task<IEnumerable<Extras?>> GetExtras()
        {
            IEnumerable<Extras?> extras = await extrasRepository.Get();
            return extras;
        }

        public async Task<Extras?> GetExtrasById(int extrasId)
        {
            Extras? extras = await extrasRepository.GetById(extrasId);
            return extras ?? throw new Exception("extra_does_not_exists");
        }
    }
}
