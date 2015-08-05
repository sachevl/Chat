namespace Chat
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    using Chat.Core.Model;
    using Chat.Core.Repositories;

    public class UserInMemoryRepository : IUserRepository
    {
        private readonly ConcurrentDictionary<Guid, User> users = new ConcurrentDictionary<Guid, User>();
        private readonly ConcurrentDictionary<string, Guid> connectionToUserMapping = new ConcurrentDictionary<string, Guid>(); 

        public void AddUser(User user)
        {
            this.users.TryAdd(user.Id, user);
        }

        public User Get(Guid userId)
        {
            return this.users[userId];
        }

        public User Get(string connectionId)
        {
            Guid userId;
            if (this.connectionToUserMapping.TryGetValue(connectionId, out userId))
            {
                User user;
                if (this.users.TryGetValue(userId, out user))
                {
                    return user;
                }
            }

            throw new InvalidOperationException("Cannot find user with connection id" + connectionId);
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

            throw new InvalidOperationException("Cannot remove user with connection id " + connectionId);
        }

        public void AssignConnectionId(string connectionId, Guid userId)
        {
            this.connectionToUserMapping.TryAdd(connectionId, userId);
        }
    }
}