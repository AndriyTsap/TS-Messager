using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1.Contracts.Enities;
using ConsoleApp1.Contracts.Services;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PhotoGallery.Entities;
using PhotoGallery.Infrastructure.Repositories;
using PhotoGallery.Infrastructure.Services;

namespace ConsoleApp1.Services
{
    public class CommandExecuter
    {
        private ILogger _loger = ServiceLocator.Instance.Resolve<ILogger>();
        private IUserRepository _userRepository= ServiceLocator.Instance.Resolve<IUserRepository>();
        private IEncryptionService _encryption = ServiceLocator.Instance.Resolve<IEncryptionService>();

        private CsvManager _csvManager= new CsvManager();

        public void Execute(string strCommand)
        {
            List<string> command;
            List<string> arguments;
            command = strCommand.Split(' ').ToList();
            switch (command[0])
            {
                case "showUsers":

                    foreach (var user in _userRepository.GetAll())
                    {
                        Console.WriteLine($"{user.Id}.{user.Username}");
                    }
                    break;
                case "blockUser"://edit work weird
                    var usersForBlocking = _userRepository.FindBy(x => x.Id == Int16.Parse(command[1]));
                    foreach (var user in usersForBlocking)
                    {
                        _userRepository.Edit(user);
                        //Console.WriteLine($"User {user.Username} with id={user.Id}are deleted");
                    }
                    _userRepository.Commit();
                    break;
                case "addUser":
                    arguments = command[1].Split(',').ToList();
                    var salt = _encryption.CreateSalt();
                    _userRepository.Add(new User
                    {
                        BirthDate = arguments[0],
                        DateCreated = DateTime.Now,
                        Email = arguments[1],
                        Salt = salt,
                        HashedPassword = _encryption.EncryptPassword(arguments[2],salt),
                        IsLocked = arguments[3]=="true",
                        Phone = arguments[4],
                        Username = arguments[5]
                    });
                    _userRepository.Commit();
                    Console.WriteLine("User added");
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
                case "importUsersFromCsv"://add writing to db
                    try
                    {
                        List<User> importedUsers=_csvManager.ImportUsers(command[1]);
                        foreach (var user in importedUsers)
                        {
                            _userRepository.Add(new User{BirthDate = user.BirthDate,
                                                         DateCreated = DateTime.Now,
                                                         Email=user.Email,
                                                         HashedPassword = user.HashedPassword,
                                                         IsLocked = user.IsLocked,
                                                         Phone = user.Phone,
                                                         Salt = user.Salt,
                                                         Username = user.Username});
                        }
                        _userRepository.Commit();
                        Console.WriteLine("Users are imported from "+command[1]);
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + command[1]+"");
                        _loger.Log(new LogEntry(LoggingEventType.Error, ex.Message +command[1]+ ""));
                    }
                    break;

                case "exportUsersToCsv":
                    AppSettings appSettings = AppSettings.Instance;
                    var users = _userRepository.GetAll().ToList();
                    try
                    {
                        _csvManager.ExportUsersToCSV(users, appSettings.UserFilePathForExport);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + "");
                        _loger.Log(new LogEntry(LoggingEventType.Error,ex.Message+""));
                    }
                    break;
                case "exit":
                    Environment.Exit(0);
                    break;
            }

        }
    }
}
