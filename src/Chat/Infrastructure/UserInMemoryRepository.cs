namespace Chat
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    using Chat.Core.Repositories;
    using Chat.Model;

    public class UserInMemoryRepository : IUserRepository
    {
        private readonly ConcurrentDictionary<Guid, User> users = new ConcurrentDictionary<Guid, User>();
        private readonly ConcurrentDictionary<string, Guid> connectionToUserMapping = new ConcurrentDictionary<string, Guid>(); 

        public void AddUser(User user)
        {
            this.users.TryAdd(user.Id, user);
        }

        public User Get(Guid id)
        {
            return this.users[id];
        }

        public IEnumerable<User> GetAll()
        {
            return this.users.Values;
        }

        public User RemoveByConnectionId(string connectionId)
        {
            Guid userId;
            if (this.connectionToUserMapping.TryRemove(connectionId, out userId))
            {
                User user;
                if (this.users.TryRemove(userId, out user))
                {
                    return user;
                }
            }

            throw new InvalidOperationException("Cannot remove user with id " + connectionId);
        }

        public void AssignConnectionId(string connectionId, Guid userId)
        {
            this.connectionToUserMapping.TryAdd(connectionId, userId);
        }
    }
}