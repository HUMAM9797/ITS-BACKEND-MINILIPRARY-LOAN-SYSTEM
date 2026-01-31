using LoanSystem.Application.DTOs;
using LoanSystem.Application.Interfaces;
using LoanSystem.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ISimpleCacheService _cacheService;

        public BooksController(IBookService bookService, ISimpleCacheService cacheService)
        {
            _bookService = bookService;
            _cacheService = cacheService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<int>> CreateBook([FromBody] CreateBookRequest request)
        {
            var bookId = await _bookService.CreateBookAsync(request);
            _cacheService.Remove("books_all"); // Invalidate cache
            return CreatedAtAction(nameof(CreateBook), new { id = bookId }, bookId);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<BookDto>>> GetBooks([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var cacheKey = $"books_page_{page}_size_{pageSize}";
            var cachedBooks = _cacheService.Get<PagedResult<BookDto>>(cacheKey);

            if (cachedBooks != null)
            {
                return Ok(cachedBooks);
            }

            var books = await _bookService.GetBooksPagedAsync(page, pageSize);
            _cacheService.Set(cacheKey, books, TimeSpan.FromSeconds(30));

            return Ok(books);
        }
    }
}
