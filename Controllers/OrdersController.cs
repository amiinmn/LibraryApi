using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly LibraryContext _context;

    public OrdersController(LibraryContext context)
    {
        _context = context;
    }

    //lihat semua order by DTO
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
    {
        var orderDto = await _context.Orders.Select(o => new OrderDto
        {
            Id = o.Id,
            BorrowerName = o.BorrowerName,
            StartBorrowDate = o.StartBorrowDate.ToString("dd MMMM yyyy"),
            EndBorrowDate = o.EndBorrowDate.ToString("dd MMMM yyyy"),
            BorrowDuration = (o.EndBorrowDate - o.StartBorrowDate).Days,
            BookId = o.BookId,
            BookTitle = o.Book.Title
        }).ToListAsync();

        return Ok(orderDto);
    }

    //order buku 
    [HttpPost]
    public async Task<IActionResult> OrderBook(OrderInput input)
    {
        var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == input.BookId && b.IsAvailable); //cek apakah buku tersedia

        if (book == null)
        {
            return BadRequest();
        }

        var orders = new Orders
        {
            BookId = input.BookId,
            BorrowerName = input.BorrowerName,
            StartBorrowDate = DateTime.Now,
            EndBorrowDate = DateTime.Now.AddDays(input.BorrowDuration)
        };

        _context.Orders.Add(orders);

        book.IsAvailable = false; // Kalau berhasil diorder, buku diubah menjadi tidak tersedia

        await _context.SaveChangesAsync();

        var orderDto = new OrderDto
        {
            Id = orders.Id,
            BorrowerName = orders.BorrowerName,
            StartBorrowDate = orders.StartBorrowDate.ToString("dd MMMM yyyy"),
            BorrowDuration = input.BorrowDuration,
            EndBorrowDate = orders.EndBorrowDate.ToString("dd MMMM yyyy"),
            BookId = orders.BookId,
            BookTitle = book.Title
        };

        return CreatedAtAction("GetOrders", new { id = orders.Id }, orderDto);
    }

    //kembalikan buku
    [HttpPut("returnBook/{id}")]
    public async Task<IActionResult> ReturnBook(int id)
    {
        var order = await _context.Orders.Include(o => o.Book).FirstOrDefaultAsync(o => o.Id == id);
        if (order == null)
        {
            return NotFound();
        }

        order.Book.IsAvailable = true; // Kalau berhasil dikembalikan, buku diubah menjadi tersedia
        order.EndBorrowDate = DateTime.Now; //ubah end borrow date menjadi hari ini karena buku sudah kembali

        await _context.SaveChangesAsync();

        var orderDto = new OrderDto
        {
            Id = order.Id,
            BorrowerName = order.BorrowerName,
            StartBorrowDate = order.StartBorrowDate.ToString("dd MMMM yyyy"),
            EndBorrowDate = DateTime.Now.ToString("dd MMMM yyyy"),
            BorrowDuration = (DateTime.Now - order.StartBorrowDate).Days,
            BookId = order.BookId,
            BookTitle = order.Book.Title
        };

        return Ok(orderDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrders(int id)
    {
        var orders = await _context.Orders.FindAsync(id);
        if (orders == null)
        {
            return NotFound();
        }

        _context.Orders.Remove(orders);
        await _context.SaveChangesAsync();

        return NoContent();
    }


}