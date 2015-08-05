namespace Chat.Tests.Hubs
{
    using System;
    using System.Threading.Tasks;

    using Chat.Core;
    using Chat.Core.Model;
    using Chat.Core.Repositories;
    using Chat.Hubs.Chat;

    using Microsoft.AspNet.SignalR.Hubs;

    using Moq;

    using NUnit.Framework;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;

    [TestFixture]
    public class ChatHubTests
    {
        [Test]
        public void ShouldAssignConnectionToTheUserAndNotifyOthers()
        {
            var sutFixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var user = sutFixture.Freeze<User>();
            var userRepositoryMock = sutFixture.Freeze<Mock<IUserRepository>>();
            var hubContextMock = sutFixture.Freeze<Mock<IHubCallerConnectionContext<IChat>>>();
            var chatMock = sutFixture.Freeze<Mock<IChat>>();
            hubContextMock.SetupGet(c => c.Others).Returns(chatMock.Object);

            var sut = sutFixture.Build<Chat>()
                .With(h => h.Clients, hubContextMock.Object)
                .Create();

            sut.Join(user.Id);

            userRepositoryMock.Verify(r => r.AssignConnectionId(It.IsAny<string>(), user.Id), Times.Once);
            chatMock.Verify(c => c.joins(It.Is<User>(u => u.Id == user.Id)), Times.Once);
        }

        [Test]
        public async Task ShouldNotifyAllAboutNewMessages()
        {
            var sutFixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var user = sutFixture.Freeze<User>();
            var hubContextMock = sutFixture.Freeze<Mock<IHubCallerConnectionContext<IChat>>>();
            var chatMock = sutFixture.Freeze<Mock<IChat>>();
            hubContextMock.SetupGet(c => c.All).Returns(chatMock.Object);

            var sut = sutFixture.Build<Chat>()
                .With(h => h.Clients, hubContextMock.Object)
                .Create();

            var message = sutFixture.Create("message");
            await sut.Send(message);

            chatMock.Verify(c => c.addNewMessageToPage(user.Name, message), Times.Once);
        }

        [Test]
        public void WhenLeaveShouldNotifyOthersAndRemoveUser()
        {
            var sutFixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
            var user = sutFixture.Freeze<User>();
            var userRepositoryMock = sutFixture.Freeze<Mock<IUserRepository>>();
            var hubContextMock = sutFixture.Freeze<Mock<IHubCallerConnectionContext<IChat>>>();
            var chatMock = sutFixture.Freeze<Mock<IChat>>();
            hubContextMock.SetupGet(c => c.Others).Returns(chatMock.Object);

            var sut = sutFixture.Build<Chat>()
                .With(h => h.Clients, hubContextMock.Object)
                .Create();

            sut.OnDisconnected(false);

            userRepositoryMock.Verify(r => r.RemoveByConnectionId(It.IsAny<string>()), Times.Once);
            chatMock.Verify(c => c.leaves(user.Id), Times.Once);
        }
    }
}