using AutoMapper;
using Delivery_BLL.Services.IServices;
using Delivery_DAL.Data;
using Delivery_DAL.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_BLL.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        private readonly IOptions<JwtConfig> _jwtOptions;

        public UserService(ApplicationDbContext context, IMapper mapper, IJwtService jwtService, IOptions<JwtConfig> jwtOptions)
        {
            _context = context;
            _mapper = mapper;
            _jwtService = jwtService;
            _jwtOptions = jwtOptions;
        }
        public bool IsUniqueUser(UserRegisterModel register)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == register.Email);
            if (user == null)
            {
                return true;
            }
            return false;
        }
        public async Task<string> Register(UserRegisterModel register)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == register.Email);
            var registerUser = _mapper.Map<User>(register);

            await _context.Users.AddAsync(registerUser);
            await _context.SaveChangesAsync();

            return new JwtSecurityTokenHandler().WriteToken(_jwtService.CreateToken(registerUser));
        }
        public async Task<string> Login(LoginCredentials login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == login.Email);

            if (user == null)
            {
                throw new BadRequestException("User does not exist. Login failed");
            }

            if (user.Password != login.Password)
            {
                throw new BadRequestException("Incorrect Password. Login failed");
            }

            return new JwtSecurityTokenHandler().WriteToken(_jwtService.CreateToken(user));
        }
        public async Task<UserDto> GetProfile(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                throw new NotFoundException("User does not exist");
            }
            return _mapper.Map<UserDto>(user);
        }
    }
}
