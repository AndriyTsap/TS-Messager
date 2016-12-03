using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IEnumerable<User>> GetFriends(int userId)
        {
            var chatsUsers = await _chatUserRepository.FindByAsync(cu => cu.UserId == userId);
            var chatIds = chatsUsers.Select(cu => cu .ChatId).ToList();
            var chats = await _chatRepository.FindByAsync(c => chatIds.Contains(c.Id));
                
            var dialogs = chats.Where(c => c.Type.Equals("dialog")).Select(d => d.Id);
            var friendIds = chatsUsers.Where(cu => dialogs.Contains(cu.ChatId)).Select(c => c.UserId);

            var friends = await _userRepository.FindByAsync(u => friendIds.Contains(u.Id));

            return friends;
        }

        public async Task<bool> ValidateFriend(int userId1, int userId2)
        {
            var friends = await GetFriends(userId1);

            if(friends.Select(f => f.Id).Contains(userId2))
                return true;
            
            return false;
        }
    }
}