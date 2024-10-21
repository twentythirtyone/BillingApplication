using BillingApplication.Entities;
using BillingApplication.Models;

namespace BillingApplication.Mapper
{
    public static class UserMapper
    {
        public static Subscriber? UserEntityToUserModel(SubscriberEntity? userEntity)
        {
            if (userEntity == null)
                return null;
            return new Subscriber()
            {
                Id= userEntity.Id,
                Email = userEntity.Email ?? "",
                Salt = userEntity.Salt ?? "",
                Password = userEntity.Password ?? "",
                Number = userEntity.Number ?? "",
            };
        }

       

        
    }
}
