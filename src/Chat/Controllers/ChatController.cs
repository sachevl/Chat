namespace Chat.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;

    using Chat.Core.Repositories;
    using Chat.Model;

    public class ChatController : ApiController
    {
        private readonly IUserRepository userRepository;

        public ChatController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public IHttpActionResult PostJoin([FromBody]User user)
        {
            if (string.IsNullOrEmpty(user.Name)) return BadRequest("User name should be specified");

            var userId = Guid.NewGuid();
            this.userRepository.AddUser(
                new User
                    {
                        Id = userId,
                        Name = user.Name
                    });

            return Ok(userId);
        }
    }
}