using ECommerceMovies.API.Data;
using ECommerceMovies.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceMovies.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public MovieController(IRepository<Movie> moviesRepository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var movies =  _unitOfWork.Movie.GetAllAsync();

            return Ok(movies);

        }
    }
}
