namespace Chat.Tests.Controllers
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Controllers;

    using Chat.Controllers;
    using Chat.Core.Model;
    using Chat.Core.Repositories;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class ChatControllerTests
    {
        [TestCase("testUser")]
        public async Task ShouldRememberJoinedUser(string userName)
        {
            var userRepositoryMock = new Mock<IUserRepository>();

            var sut = new ChatController(userRepositoryMock.Object)
                          {
                              Request = new HttpRequestMessage(),
                              RequestContext = new HttpRequestContext
                                                   {
                                                       Configuration = new HttpConfiguration()
                                                   }
                          };

            var response = sut.PostJoin(
                new User
                    {
                        Name = userName
                    });

            var userId = await (await response.ExecuteAsync(CancellationToken.None)).Content.ReadAsAsync<Guid>();

            userRepositoryMock.Verify(r => r.AddUser(It.Is<User>(u => u.Name == userName && u.Id == userId)), Times.Once);
        }
    }
}