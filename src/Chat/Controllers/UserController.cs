namespace Chat.Controllers
{
    using System.Linq;
    using System.Web.Http;

    using Chat.Core.Repositories;

    public class UserController : ApiController
    {
        private readonly IUserRepository userRepository;

        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public IHttpActionResult Get()
        {
            return Ok(this.userRepository.GetAll());
        }
    }
}