using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using backend.Data;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseBillController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PurchaseBillController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("add-item")]
        public async Task<IActionResult> AddItem([FromBody] PurchaseBillItem item)
        {
            try
            {
                // Calculate totals
                item.TotalCost = (item.StandardCost * item.Quantity) - (item.StandardCost * item.Quantity * item.Discount / 100);
                item.TotalSelling = item.StandardPrice * item.Quantity;

                _context.PurchaseBillItems.Add(item);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Item added successfully", Item = item });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Error adding item: {ex.Message}" });
            }
        }

        [HttpGet("items")]
        public async Task<IActionResult> GetItems()
        {
            try
            {
                var items = await Task.FromResult(_context.PurchaseBillItems.ToList());
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Error retrieving items: {ex.Message}" });
            }
        }        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            try
            {
                var items = await _context.PurchaseBillItems.ToListAsync();
                var summary = new
                {
                    TotalItems = items.Count,
                    TotalQuantity = items.Sum(i => i.Quantity),
                    GrossTotal = items.Sum(i => i.TotalSelling),
                    TotalDiscount = items.Sum(i => (i.StandardCost * i.Quantity) - i.TotalCost),
                    NetTotal = items.Sum(i => i.TotalCost)
                };

                return Ok(summary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Error calculating summary: {ex.Message}" });
            }
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearItems()
        {
            try
            {
                _context.PurchaseBillItems.RemoveRange(_context.PurchaseBillItems);
                await _context.SaveChangesAsync();
                return Ok(new { Message = "All items cleared successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Error clearing items: {ex.Message}" });
            }
        }
    }
}
