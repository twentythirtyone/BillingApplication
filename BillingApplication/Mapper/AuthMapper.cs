using BillingApplication.Entities;
using BillingApplication.Models;

namespace BillingApplication.Mapper
{
    public class AuthMapper
    {
        public static User? UserEntityToUserModel(UserEntity? userEntity)
        {
            if (userEntity == null)
                return null;
            return new User()
            {
                Email = userEntity.Email ?? "",
                Salt = userEntity.Salt ?? "",
                Password = userEntity.Password ?? ""
            };
        }
    }
}
