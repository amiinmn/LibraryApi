using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly LibraryContext _context;

    public BooksController(LibraryContext context)
    {
        _context = context;
    }

    //gLihat semua buku
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
    {
        var books = await _context.Books.ToListAsync();
        var order = await _context.Orders.Where(o => o.EndBorrowDate > DateTime.Now).ToListAsync();
        var bookDtos = books.Select(b => new BookDto
        {
            Id = b.Id,
            Title = b.Title,
            Author = b.Author,
            Publisher = b.Publisher,
            PublishedYear = b.PublishDate.Year,
            CoverImageUrl = b.CoverImageUrl,
            IsAvailable = b.IsAvailable,
            WillBeAvailableAt = order.Any(o => o.BookId == b.Id) ? order.Where(o => o.BookId == b.Id).OrderBy(o => o.EndBorrowDate).First().EndBorrowDate.ToString("dd MMMM yyyy") : "Tersedia saat ini"

        });

        return Ok(bookDtos);
    }

    //Tambah buku
    [HttpPost]
    public async Task<ActionResult<Books>> AddBook(AddBookInput input)
    {
        if (input.PublishedYear > DateTime.Now.Year)
        {
            return BadRequest("Tahun terbit tidak valid");
        }

        var book = new Books
        {
            Title = input.Title,
            Author = input.Author,
            Publisher = input.Publisher,
            PublishDate = new DateTime(input.PublishedYear, 1, 1),
            CoverImageUrl = input.CoverImageUrl,
            IsAvailable = true
        };

        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        var bookDto = new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Publisher = book.Publisher,
            PublishedYear = book.PublishDate.Year,
            CoverImageUrl = book.CoverImageUrl,
            IsAvailable = book.IsAvailable,
            WillBeAvailableAt = "Tersedia saat ini"
        };

        return CreatedAtAction("GetBooks", new { id = book.Id }, bookDto);
    }

    //Hapus buku
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBooks(int id)
    {
        var books = await _context.Books.FindAsync(id);
        if (books == null)
        {
            return NotFound();
        }

        _context.Books.Remove(books);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    //Cari buku berdasarkan Judul
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<BookDto>>> SearchBooks([FromQuery] string title)
    {
        var books = await _context.Books.Where(b => b.Title.Contains(title)).ToListAsync();
        var order = await _context.Orders.Where(o => o.EndBorrowDate > DateTime.Now).ToListAsync();

        var bookDtos = books.Select(b => new BookDto
        {
            Id = b.Id,
            Title = b.Title,
            Author = b.Author,
            Publisher = b.Publisher,
            PublishedYear = b.PublishDate.Year,
            CoverImageUrl = b.CoverImageUrl,
            IsAvailable = b.IsAvailable,
            WillBeAvailableAt = order.Any(o => o.BookId == b.Id) ? order.Where(o => o.BookId == b.Id).OrderBy(o => o.EndBorrowDate).First().EndBorrowDate.ToString("dd MMMM yyyy") : "Tersedia saat ini"
        });

        return Ok(bookDtos);
    }

}