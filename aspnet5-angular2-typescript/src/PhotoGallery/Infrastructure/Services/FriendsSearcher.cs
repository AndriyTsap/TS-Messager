using System.Collections.Generic;
using System.Linq;
using PhotoGallery.Entities;
using PhotoGallery.Infrastructure.Repositories.Abstract;
using PhotoGallery.Infrastructure.Services.Abstract;

namespace PhotoGallery.Infrastructure.Services
{
    public class FriendsSearcher : IFriendsSearcher
    {
        private IUserRepository _userRepository;
        private IChatRepository _chatRepository;
        private IChatUserRepository _chatUserRepository;

        public FriendsSearcher(IUserRepository userRepository, IChatRepository chatRepository, IChatUserRepository chatUserRepository)
        {
            _userRepository = userRepository;
            _chatRepository = chatRepository;
            _chatUserRepository = chatUserRepository;
        }
        public IEnumerable<User> GetFriends(int userId)
        {
            var chatsUsers = _chatUserRepository.FindBy(cu => cu.UserId == userId);
            var chatIds = chatsUsers.Select(cu => cu .ChatId).ToList();
            var chats = _chatRepository.FindBy(c => chatIds.Contains(c.Id))
                .ToList();
            
            var dialogs = chats.Where(c => c.Type.Equals("dialog")).Select(d => d.Id);
            var friendIds = chatsUsers.Where(cu => dialogs.Contains(cu.ChatId)).Select(c => c.UserId);

            var friends = _userRepository.FindBy(u => friendIds.Contains(u.Id));

            return friends;
        }

        public bool ValidateFriend(int userId1, int userId2)
        {
            var friends = GetFriends(userId1);

            if(friends.Select(f => f.Id).Contains(userId2))
                return true;
            
            return false;
        }
    }
}