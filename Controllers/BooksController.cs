using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Bookstore.Data;
using Online_Bookstore.DTO;
using Online_Bookstore.Models;


	namespace OnlineBookstoreAPI.Controllers
	{
		[Route("api/books")]
		[ApiController]
		public class BookController : ControllerBase
		{
			private readonly ApplicationDbContext _context;

			public BookController(ApplicationDbContext context)
			{
				_context = context;
			}

			
			[HttpGet]
			public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks(
				[FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
			{
				var totalRecords = await _context.Books.CountAsync();
				var books = await _context.Books
					.Include(b => b.Author)
					.Skip((pageNumber - 1) * pageSize)
					.Take(pageSize)
					.Select(b => new BookDto
					{
						Title = b.Title,
						Genre = b.Genre,
						AuthorId = b.AuthorId
					})
					.ToListAsync();

				return Ok(new { TotalRecords = totalRecords, PageNumber = pageNumber, PageSize = pageSize, Data = books });
			}

			
			[HttpGet("{id}")]
			public async Task<ActionResult<BookDto>> GetBookById(int id)
			{
				var book = await _context.Books.Include(b => b.Author)
					.Where(b => b.Id == id)
					.Select(b => new BookDto
					{
						Title = b.Title,
						Genre = b.Genre,
						AuthorId = b.AuthorId
					})
					.FirstOrDefaultAsync();

				if (book == null)
				{
					return NotFound(new { message = "Book not found" });
				}

				return Ok(book);
			}

			
			[HttpPost]
			public async Task<ActionResult<Book>> AddBook(BookDto bookDto)
			{
				var book = new Book
				{
					Title = bookDto.Title,
					Genre = bookDto.Genre,
					AuthorId = bookDto.AuthorId
				};

				_context.Books.Add(book);
				await _context.SaveChangesAsync();
				return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
			}

			
			[HttpPut("{id}")]
			public async Task<IActionResult> UpdateBook(int id, BookDto bookDto)
			{
				var book = await _context.Books.FindAsync(id);
				if (book == null)
				{
					return NotFound(new { message = "Book not found" });
				}

				book.Title = bookDto.Title;
				book.Genre = bookDto.Genre;
				book.AuthorId = bookDto.AuthorId;

				await _context.SaveChangesAsync();
				return Ok(new { message = "Book updated successfully" });
			}

			
			[HttpDelete("{id}")]
			public async Task<IActionResult> DeleteBook(int id)
			{
				var book = await _context.Books.FindAsync(id);
				if (book == null)
				{
					return NotFound(new { message = "Book not found" });
				}

				_context.Books.Remove(book);
				await _context.SaveChangesAsync();
				return Ok(new { message = "Book deleted successfully" });
			}
		}
	}

