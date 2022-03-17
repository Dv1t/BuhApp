using BuhServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuhServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : Controller
    {
        private TransactionsDbContext db;
        public TransactionsController(TransactionsDbContext context)
        {
            db = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetAllTransactions()
        {
            return await db.Transactions.ToListAsync();
        }
        [HttpPost]
        public async Task<IActionResult> AddTransaction(Transaction transaction)
        {
            if (transaction == null)
            {
                return BadRequest();
            }
            db.Transactions.Add(transaction);
            await db.SaveChangesAsync();
            return Ok(transaction);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Transaction>> DeleteTransaction(int id)
        {
            Transaction transaction = db.Transactions.FirstOrDefault(x => x.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }
            db.Transactions.Remove(transaction);
            await db.SaveChangesAsync();
            return Ok(transaction);
        }
    }
}
