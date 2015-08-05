namespace Chat.Core
{
    using System;
    using System.Threading.Tasks;

    using Chat.Core.Model;

    public interface IChat
    {
        void joins(User user);

        Task addNewMessageToPage(string name, string message);

        Task leaves(Guid id);
    }
}