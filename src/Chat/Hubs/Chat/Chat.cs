namespace Chat.Hubs.Chat
{
    using System;
    using System.Threading.Tasks;

    using global::Chat.Core;
    using global::Chat.Core.Repositories;

    using Microsoft.AspNet.SignalR;

    public class Chat : Hub<IChat>
    {
        private readonly IUserRepository userRepository;
        private readonly IMessageRepository messageRepository;

        public Chat(IUserRepository userRepository, IMessageRepository messageRepository)
        {
            this.userRepository = userRepository;
            this.messageRepository = messageRepository;
        }

        public void Join(Guid id)
        {
            var user = this.userRepository.Get(id);
            this.userRepository.AssignConnectionId(Context.ConnectionId, id);
            Clients.Others.joins(user);
        }

        public async Task Send(string message)
        {
            if (string.IsNullOrEmpty(message)) return;

            var user = this.userRepository.Get(Context.ConnectionId);

            await this.messageRepository.Add(user.Id, user.Name, message);
            await Clients.All.addNewMessageToPage(user.Name, message);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var user = this.userRepository.RemoveByConnectionId(Context.ConnectionId);
            return Clients.Others.leaves(user.Id);
        }
    }
}