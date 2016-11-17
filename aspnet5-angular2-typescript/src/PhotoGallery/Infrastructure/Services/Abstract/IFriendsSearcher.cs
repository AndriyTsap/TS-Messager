using System.Collections.Generic;
using PhotoGallery.Entities;

namespace PhotoGallery.Infrastructure.Services.Abstract
{
    public interface IFriendsSearcher
    {
        bool ValidateFriend(int userId1, int userId2);
        IEnumerable<User> GetFriends(int userId);
    }
}