using System;
using ConsoleApp1.Contracts.Services;
using PhotoGallery.Entities;
using PhotoGallery.Infrastructure.Core;
using System.Security.Cryptography;
using PhotoGallery.Infrastructure.Repositories;
using PhotoGallery.ViewModels;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Identity;
using PhotoGallery.Infrastructure.Services;

namespace ConsoleApp1.Services
{
    public class AccountService: IAccountService
    {
        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;
        private IUserRoleRepository _userRoleRepository;
        private IEncryptionService _encriptionService;
        

        public AccountService(IUserRepository userRepository, IRoleRepository roleRepository, IUserRoleRepository userRoleRepository,  IEncryptionService encriptionService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _encriptionService = encriptionService;
        }

        private string GetSalt(string userName)
        {
            string salt = _userRepository.GetSingleByUsername(userName).Salt;
            return salt;
        }



        public GenericResult Login(LoginViewModel user)
        {
            string salt = GetSalt(user.Username);
            var password = user.Password;

            var hashedPassword = _userRepository.GetSingleByUsername(user.Username).HashedPassword;

            var encriptedPassword = _encriptionService.EncryptPassword(password, salt);

            Console.WriteLine(hashedPassword+"  "+encriptedPassword);

            if(encriptedPassword.Equals(hashedPassword))
                return  new GenericResult()
                {
                    Succeeded = true,
                    Message = "Authentication success!!!"
                };

            return new GenericResult()
            {
                Succeeded = false,
                Message = "Authentication failed!!!"
            };
        }

        public GenericResult Register(RegistrationViewModel user, int[] roles)
        {
            if (_userRepository.GetAll().ToList().Select(userRepo => userRepo.Username).Contains(user.Username))
                throw new Exception("User alredy exist!!!");

            if (roles.Length==0)
                throw new Exception("User must have minimum 1 role!!!");

            var salt = _encriptionService.CreateSalt();

            var hashedPassword = _encriptionService.EncryptPassword(user.Password, salt);

            User newUser;
            _userRepository.Add(newUser = new User()
            {
                DateCreated = DateTime.Now,
                Email = user.Email,
                IsLocked = false,
                HashedPassword = hashedPassword,
                Salt = salt,
                Username = user.Username
            });

            _userRepository.Commit();

            var repoLoles = _roleRepository.GetAll().Select(role => role.Id);
            foreach (var role in roles)
            {
                if (repoLoles.Contains(role))
                    _userRoleRepository.Add(new UserRole() {RoleId = role, UserId = newUser.Id});
                else
                    throw new Exception("role does not exist");
            }

            _userRoleRepository.Commit();
            _userRepository.Commit();

            return new GenericResult()
            {
                Succeeded = true,
                Message = "Registration success!!!"
            };
        }
    }
}