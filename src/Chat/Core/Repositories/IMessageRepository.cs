namespace Chat.Core.Repositories
{
    using System;
    using System.Threading.Tasks;

    public interface IMessageRepository
    {
        Task Add(Guid userId, string userName, string message);
    }
}