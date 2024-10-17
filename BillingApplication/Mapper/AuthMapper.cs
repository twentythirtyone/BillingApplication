using BillingApplication.Entities;
using BillingApplication.Models;

namespace BillingApplication.Mapper
{
    public class AuthMapper
    {
        public static Models.Subscriber? UserEntityToUserModel(Entities.SubscriberEntity? userEntity)
        {
            if (userEntity == null)
                return null;
            return new Models.Subscriber()
            {
                Email = userEntity.Email ?? "",
                Salt = userEntity.Salt ?? "",
                Password = userEntity.Password ?? "",
                Number = userEntity.Number ?? "",
            };
        }
    }
}
