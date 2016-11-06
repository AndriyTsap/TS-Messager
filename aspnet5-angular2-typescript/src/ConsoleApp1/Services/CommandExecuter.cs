using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ConsoleApp1.Contracts.Enities;
using ConsoleApp1.Contracts.Services;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PhotoGallery.Entities;
using PhotoGallery.Infrastructure.Repositories;
using PhotoGallery.Infrastructure.Repositories.Abstract;
using PhotoGallery.Infrastructure.Services;
using PhotoGallery.Infrastructure.Services.Abstract;
using PhotoGallery.ViewModels;
using ServiceStack;

namespace ConsoleApp1.Services
{
    public class CommandExecuter : ICommandExecuter
    {
        private ILogger _loger = ServiceLocator.Instance.Resolve<ILogger>();
        private IUserRepository _userRepository= ServiceLocator.Instance.Resolve<IUserRepository>();
        private ISerializer _serializer = ServiceLocator.Instance.Resolve<ISerializer>();
        private CsvManager _csvManager= new CsvManager();
        private AppSettings _appSettings = AppSettings.Instance;
        private IAccountService _accountService = ServiceLocator.Instance.Resolve<IAccountService>();
        private FileManager _fileManager;
        private User _user; 

        public CommandExecuter()
        {
            _user = new User();
            while (true)
            {
                Console.WriteLine("Your login");
                var login = Console.ReadLine();
                Console.WriteLine("Your password:");
                var password = Console.ReadLine();
                var result = _accountService.Login(new LoginViewModel() { Username = login, Password = password, RememberMe = false });
                Console.WriteLine(result.Message);
                if (result.Succeeded)
                {
                    _user = _userRepository.GetSingleByUsername(login);
                    break;
                }
                   
            }
        }

        public void Execute(string strCommand)
        {
            List<string> command;
            
            command = strCommand.Split(' ').ToList();
            switch (command[0])
            {
                case "showUsers":
                    foreach (var user in _userRepository.GetAll())
                    {
                        Console.WriteLine($"{user.Id}.{user.Username}");
                    }
                    break;

                case "blockUser":
                    var usersForBlocking = _userRepository.GetSingle(x => x.Id == Int16.Parse(command[1]));

                    if (usersForBlocking==null)
                    {
                        throw new Exception("User is not found");
                    }
                    usersForBlocking.IsLocked = true;
                    _userRepository.Edit(usersForBlocking);
                    Console.WriteLine($"User {usersForBlocking.Username} with id={usersForBlocking.Id}are blocked");
                    _userRepository.Commit();
                    break;

                case "unblockUser":
                    var usersForUnBlocking = _userRepository.FindBy(x => x.Id == Int16.Parse(command[1]));

                    foreach (var user in usersForUnBlocking)
                    {
                        user.IsLocked = false;
                        _userRepository.Edit(user);
                    }
                    _userRepository.Commit();
                    break;

                case "addUser":
                    User newUser = new User();
                    setUserProperies(newUser);
                    _userRepository.Add(newUser);
                    _userRepository.Commit();
                    Console.WriteLine($"User {newUser.Username} with id={newUser.Id} are added");
                    break;

                case "updateUser":
                    var usersForUpdating = _userRepository.GetSingle(x => x.Id == Int16.Parse(command[1]));
                    /*foreach (var property in usersForUpdating.GetType().GetProperties())
                    {
                        if (property.Name == "Id")
                        {
                            continue;
                        }
                        Console.WriteLine($"Enter {property.Name}({property.PropertyType}) ");
                        var propertyNewValue = Console.ReadLine();
                        if (propertyNewValue.Length != 0)
                        {
                            if (property.PropertyType == typeof(int))
                            {
                                property.SetValue(usersForUpdating,Int32.Parse(propertyNewValue) );
                            }
                            if (property.PropertyType == typeof(DateTime))
                            {
                                property.SetValue(usersForUpdating, DateTime.Parse(propertyNewValue));
                            }
                            else
                            {
                                property.SetValue(usersForUpdating, propertyNewValue);
                            }  
                        }
                    }*/
                    setUserProperies(usersForUpdating);
                    _userRepository.Edit(usersForUpdating);
                    _userRepository.Commit();
                    Console.WriteLine($"User {usersForUpdating.Username} with id={usersForUpdating.Id} are updated");

                    break;
                case "deleteUser":

                    var usersForDeleting=_userRepository.FindBy(x=>x.Id== Int16.Parse(command[1]));
                    foreach (var user in usersForDeleting)
                    {
                        _userRepository.Delete(user);
                        Console.WriteLine($"User {user.Username} with id={user.Id}are deleted");
                    }
                    _userRepository.Commit();
                    break;

                case "importUsersFromXml"://not finished
                    //string usersFromXml=_fileManager.ReadFromFile(_appSettings.UsersFilePath);
                    //foreach (var user in _serializer.Deserialize<List<User>>(usersFromXml))
                    //{
                    //    _userRepository.Add(user);
                    //}
                    break;

                case "exportUsersToXml":
                    var users = _userRepository.GetAll().ToList();
                    string strUsers=_serializer.Serialize(users);
                    Console.WriteLine(strUsers);
                    _fileManager.WriteToFile(strUsers,_appSettings.UsersFilePath);
                    break;

                case "importUsersFromCsv":
                    List<User> importedUsers=_csvManager.ImportUsers(command[1]);
                    foreach (var user in importedUsers)
                    {
                        _userRepository.Add(new User
                        {
                            BirthDate = user.BirthDate,
                            DateCreated = DateTime.Now,
                            Email = user.Email,
                            HashedPassword = user.HashedPassword,
                            IsLocked = user.IsLocked,
                            Phone = user.Phone,
                            Salt = user.Salt,
                            Username = user.Username
                        });
                    }
                    _userRepository.Commit();
                    Console.WriteLine("Users are imported from "+command[1]);
                    break;

                case "exportUsersToCsv":
                    _csvManager.ExportUsersToCSV(_userRepository.GetAll().ToList(), _appSettings.UserFilePathForExport);
                    Console.WriteLine("Users are exported from " + command[1]);
                    break;

                case "exit":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Not exist command");
                    break;
            }

        }

        private static void setUserProperies(User user)
        {
            Console.WriteLine("Enter username(string)");
            string name = Console.ReadLine();
            if (name.Length != 0)
            {
                user.Username =name ;
            }
            Console.WriteLine("Enter bithdate(date)");
            while (true)
            {
                string date = Console.ReadLine();
                if (date.Length != 0)
                {
                    DateTime dateOut;
                    if (DateTime.TryParse(date, out dateOut))
                    {
                        user.BirthDate = dateOut.ToString();
                        break;
                    }
                    Console.WriteLine("Not correct format! Try again:");
                }else
                {
                    break;
                }
            }
            Console.WriteLine("Enter email(name@domain)");
            string email;
            while (true)
            {
                email = Console.ReadLine();
                if (email.Length != 0)
                {
                    if (email.Contains('@'))
                    {
                        user.Email = email;
                        break;
                    }
                    Console.WriteLine("Not right format! Try again:");
                }else
                {
                    break;
                }
            }
            Console.WriteLine("Is user locked(true/false)");
            var isLocked = Console.ReadLine();
            if (isLocked.Length != 0)
            {
                user.IsLocked = (isLocked == "true");
            }
            Console.WriteLine("Enter password");
            var password = Console.ReadLine();
            if (password.Length != 0)
            {
                user.Salt = ServiceLocator.Instance.Resolve<IEncryptionService>().CreateSalt();
                user.HashedPassword = ServiceLocator.Instance.Resolve<IEncryptionService>().EncryptPassword(password, user.Salt);
            }
            
            Console.WriteLine("Enter phone");
            var phone = Console.ReadLine();
            if (phone.Length!=0)
            {
                user.Phone = phone;
            }
        }
    }
}
