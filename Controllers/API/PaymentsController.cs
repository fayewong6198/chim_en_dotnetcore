using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Chim_En_DOTNET.Data;
using Chim_En_DOTNET.Models;
using Chim_En_DOTNET.Helpers;

namespace Chim_En_DOTNET.Controllers_API
{
  [Route("api/[controller]")]
  [ApiController]
  public class PaymentsController : ControllerBase
  {
    private readonly ApplicationDbContext _context;

    public PaymentsController(ApplicationDbContext context)
    {
      _context = context;
    }

    // GET: api/Payments
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Payment>>> GetPayments([FromQuery] PaymentFilter paymentFilter)
    {
      var validFilter = new PaymentFilter(paymentFilter.PageNumber, paymentFilter.PageSize, paymentFilter.Search, paymentFilter.OrderBy, paymentFilter.UserId, paymentFilter.DeviceId, paymentFilter.PaymentMethod, paymentFilter.Status, paymentFilter.Total, paymentFilter.CreatedAt);

      var payments = from p in _context.Payments select p;

      // Filter
      if (!string.IsNullOrEmpty(validFilter.UserId))
        payments = payments.Where(p => p.ApplicationUserId.Equals(validFilter.UserId));

      if (!string.IsNullOrEmpty(validFilter.DeviceId))
        payments = payments.Where(p => p.DeviceId.Equals(validFilter.DeviceId));

      if (validFilter.PaymentMethod != null)
        payments = payments.Where(p => p.PaymentMethod == validFilter.PaymentMethod);

      if (validFilter.Status != null)
        payments = payments.Where(p => p.Status == validFilter.Status);

      if (validFilter.Total != null)
        payments = payments.Where(p => p.Total >= validFilter.Total);

      // Sort
      switch (validFilter.OrderBy.ToLower())
      {
        case "-createdat":
          payments = payments.OrderByDescending(p => p.CreatedAt);
          break;
        default:
          payments = payments.OrderBy(p => p.CreatedAt);
          break;
      }

      // Pagination
      payments = payments.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize);

      var data = await payments.Include(p => p.PaymentUserDetails).Include(p => p.PaymentProductDetails).AsNoTracking().ToListAsync();
      var totalRecord = await _context.Payments.CountAsync();


      return Ok(new PagedResponse<List<Payment>>(data, validFilter.PageNumber, validFilter.PageSize, totalRecord));
    }

    // GET: api/Payments/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Payment>> GetPayment(int id)
    {
      var payment = await _context.Payments.FindAsync(id);

      if (payment == null)
      {
        return NotFound();
      }

      return payment;
    }

    // PUT: api/Payments/5
    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPayment(int id, Payment payment)
    {
      if (id != payment.Id)
      {
        return BadRequest();
      }

      _context.Entry(payment).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!PaymentExists(id))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }

      return NoContent();
    }

    // POST: api/Payments
    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    [HttpPost]
    public async Task<ActionResult<Payment>> PostPayment(Payment payment)
    {
      _context.Payments.Add(payment);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetPayment", new { id = payment.Id }, payment);
    }

    // DELETE: api/Payments/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<Payment>> DeletePayment(int id)
    {
      var payment = await _context.Payments.FindAsync(id);
      if (payment == null)
      {
        return NotFound();
      }

      _context.Payments.Remove(payment);
      await _context.SaveChangesAsync();

      return payment;
    }

    private bool PaymentExists(int id)
    {
      return _context.Payments.Any(e => e.Id == id);
    }
  }
}
