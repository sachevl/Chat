namespace Chat.Core.Repositories
{
    using System;
    using System.Collections.Generic;

    using Chat.Model;

    public interface IUserRepository
    {
        void AddUser(User user);

        User Get(Guid userId);

        User Get(string connectionId);

        IEnumerable<User> GetAll();

        User RemoveByConnectionId(string connectionId);

        void AssignConnectionId(string connectionId, Guid userId);
    }
}