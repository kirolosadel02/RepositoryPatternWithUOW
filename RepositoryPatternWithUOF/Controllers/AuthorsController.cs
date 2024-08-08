using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryPatternWithUOF.Core.Models;
using RepositoryPatternWithUOW.Core.Interfaces;

namespace RepositoryPatternWithUOW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthorsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetById()
        {
            return Ok(_unitOfWork.Authors.GetById(1));
        }

        [HttpGet("GetAllAuthors")]
        public IActionResult GetAll()
        {
            return Ok(_unitOfWork.Authors.GetAll());
        }
    }
}
