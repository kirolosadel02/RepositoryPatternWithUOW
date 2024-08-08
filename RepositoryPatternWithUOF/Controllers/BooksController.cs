using Microsoft.AspNetCore.Mvc;
using RepositoryPatternWithUOF.Core.Models;
using RepositoryPatternWithUOW.Core.Interfaces;
using System.Linq;

namespace RepositoryPatternWithUOW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IUnitOfWork  _unitOfWork;

        public BooksController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var book = _unitOfWork.Books.Find(b => b.Id == id, new[] { "Author" }).FirstOrDefault();
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpGet("GetByName")]
        public IActionResult GetByName(string name)
        {
            var book = _unitOfWork.Books.Find(b => b.Title == name, new[] { "Author" }).FirstOrDefault();
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpGet("GetAllByName")]
        public IActionResult GetAllByName(string name)
        {
            var books = _unitOfWork.Books.FindAll(b => b.Title.Contains(name), new[] { "Author" }).ToList();
            if (!books.Any())
            {
                return NotFound();
            }
            return Ok(books);
        }
    }
}
