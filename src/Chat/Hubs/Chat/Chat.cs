namespace Chat.Hubs.Chat
{
    using System;
    using System.Threading.Tasks;

    using global::Chat.Core.Repositories;
    using global::Chat.Model;

    using Microsoft.AspNet.SignalR;

    public class Chat : Hub
    {
        private readonly IUserRepository userRepository;

        public Chat(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public void Join(Guid id)
        {
            Clients.Caller.Id = id;
            var user = this.userRepository.Get(id);
            Clients.Caller.Name = user.Name;
            this.userRepository.AssignConnectionId(Context.ConnectionId, id);
            Clients.Others.joins(user);
        }

        public void Send(string message)
        {
            Clients.All.addNewMessageToPage(Clients.Caller.Name, message);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var user = this.userRepository.RemoveByConnectionId(Context.ConnectionId);
            return Clients.All.leaves(user.Id);
        }
    }
}