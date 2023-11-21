using ECommerceMovies.API.Data;
using ECommerceMovies.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceMovies.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ActorController(IRepository<Actor> actorsRepository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var actors = _unitOfWork.Actor.GetAllAsync();

            return Ok(actors);

        }
    }
}
