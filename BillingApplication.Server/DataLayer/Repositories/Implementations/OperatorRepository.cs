using BillingApplication.Server.DataLayer.Repositories.Abstractions;
using BillingApplication.Server.Mapper;
using BillingApplication.Services.Models.Roles;
using Microsoft.EntityFrameworkCore;

namespace BillingApplication.Server.DataLayer.Repositories.Implementations
{
    public class OperatorRepository : IOperatorRepository
    {
        private readonly BillingAppDbContext context;

        public OperatorRepository(BillingAppDbContext context)
        {
            this.context = context;
        }

        public async Task<int?> Create(Operator operatorModel)
        {
            var operatorEntity = OperatorMapper.OperatorModelToOperatorEntity(operatorModel);

            await context.Operators.AddAsync(operatorEntity);
            await context.SaveChangesAsync();

            return operatorEntity.Id;
        }

        public async Task<int?> Delete(int? id)
        {
            var operatorEntity = await context.Operators.FindAsync(id);
            context.Operators.Remove(operatorEntity!);
            await context.SaveChangesAsync();
            return operatorEntity!.Id;
        }

        public async Task<IEnumerable<Operator>> GetAll()
        {
            var operators = await context.Operators
                        .AsNoTracking()
                        .ToListAsync();
            return operators.Select(OperatorMapper.OperatorEntityToOperatorModel);
        }

        public async Task<Operator?> GetOperatorById(int? id)
        {
            return OperatorMapper.OperatorEntityToOperatorModel(await context.Operators.FindAsync(id));
        }

        public async Task<Operator?> GetOperatorByEmail(string email)
        {
            return OperatorMapper.OperatorEntityToOperatorModel(
                await context.Operators
                    .Where(x=>x.Email == email)
                    .FirstOrDefaultAsync());
        }

        public async Task<int?> Update(Operator operatorModel)
        {
            var operatorEntity = await context.Operators.FindAsync(operatorModel.Id);
            OperatorMapper.UpdateEntity(operatorEntity!, operatorModel);
            await context.SaveChangesAsync();

            return operatorEntity.Id;
        }
    }
}
