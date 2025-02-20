using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using PRN222.Assignment1.Business.DTOs;
using PRN222.Assignment1.Business.Exceptions;
using PRN222.Assignment1.Data.Models;
using PRN222.Assignment1.Data.Repositories;

namespace PRN222.Assignment1.Business.Services.Implements
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> AuthenticationAsync(string username, string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return null;
            }
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> RegisterAsync(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);

            var checkUsername = await _userRepository.GetUserByUsernameAsync(userDto.Username);
            if (checkUsername != null)
            {
                throw new UsernameAlreadyExistException("Username is already taken");
            }

            var checkEmail = await _userRepository.GetUserByEmailAsync(userDto.Email);
            if (checkEmail != null)
            {
                throw new EmailAlreadyExistException("Email is already taken");
            }

            if (user != null)
            {
                var enterPassword = user.Password;
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(enterPassword);
                user.Password = hashedPassword;
                user.Role = "USER";
                try
                {
                    var userCreated = await _userRepository.CreateUserAsync(user);
                    return _mapper.Map<UserDto>(userCreated);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else
            {
                return null;
            }
        }  

        
        
        

    }
}
