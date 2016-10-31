using PhotoGallery.Entities;
using PhotoGallery.Infrastructure.Core;
using PhotoGallery.ViewModels;

namespace ConsoleApp1.Contracts.Services
{
    public interface IAccountService
    {
        GenericResult Login(LoginViewModel user);
        GenericResult Register(RegistrationViewModel user, int[] roles);
    }
}