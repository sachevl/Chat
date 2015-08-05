namespace Chat.Tests.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
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

    using Ploeh.AutoFixture;

    [TestFixture]
    public class UserControllerTests
    {
        [Test]
        public async Task ShouldReturnRememberedUsers()
        {
            var persistedUsers = new Fixture().CreateMany<User>().ToList();
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(r => r.GetAll()).Returns(persistedUsers);

            var sut = new UserController(userRepositoryMock.Object)
            {
                Request = new HttpRequestMessage(),
                RequestContext = new HttpRequestContext
                {
                    Configuration = new HttpConfiguration()
                }
            };

            var users = await(await sut.Get().ExecuteAsync(CancellationToken.None)).Content.ReadAsAsync<ICollection<User>>();

            CollectionAssert.AreEquivalent(persistedUsers, users);
        }
    }
}