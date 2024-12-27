using Delivery_DAL.Entity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_BLL.Services.IServices
{
    public interface IJwtService
    {
        JwtSecurityToken CreateToken(User user);
    }
}
