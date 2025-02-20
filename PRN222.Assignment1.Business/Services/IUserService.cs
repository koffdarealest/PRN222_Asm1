using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN222.Assignment1.Business.DTOs;
using PRN222.Assignment1.Data.Models;

namespace PRN222.Assignment1.Business.Services
{
    public interface IUserService
    {
        Task<UserDto> AuthenticationAsync(string username, string password);
        Task<UserDto> RegisterAsync(UserDto userDto);
    }
}
