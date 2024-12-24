using Delivery_DAL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_BLL.Services.IServices
{
    public interface IUserService
    {
        bool IsUniqueUser(UserRegisterModel register);
        Task<string> Register(UserRegisterModel register);
        Task<string> Login(LoginCredentials credentials);
        
    }
}
