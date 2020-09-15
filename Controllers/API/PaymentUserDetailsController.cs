using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Chim_En_DOTNET.Data;
using Chim_En_DOTNET.Models;

namespace Chim_En_DOTNET.Controllers_API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentUserDetailsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PaymentUserDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PaymentUserDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentUserDetail>>> GetPaymentUserDetails()
        {
            return await _context.PaymentUserDetails.ToListAsync();
        }

        // GET: api/PaymentUserDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentUserDetail>> GetPaymentUserDetail(int id)
        {
            var paymentUserDetail = await _context.PaymentUserDetails.FindAsync(id);

            if (paymentUserDetail == null)
            {
                return NotFound();
            }

            return paymentUserDetail;
        }

        // PUT: api/PaymentUserDetails/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaymentUserDetail(int id, PaymentUserDetail paymentUserDetail)
        {
            if (id != paymentUserDetail.Id)
            {
                return BadRequest();
            }

            _context.Entry(paymentUserDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentUserDetailExists(id))
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

        // POST: api/PaymentUserDetails
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PaymentUserDetail>> PostPaymentUserDetail(PaymentUserDetail paymentUserDetail)
        {
            _context.PaymentUserDetails.Add(paymentUserDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPaymentUserDetail", new { id = paymentUserDetail.Id }, paymentUserDetail);
        }

        // DELETE: api/PaymentUserDetails/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PaymentUserDetail>> DeletePaymentUserDetail(int id)
        {
            var paymentUserDetail = await _context.PaymentUserDetails.FindAsync(id);
            if (paymentUserDetail == null)
            {
                return NotFound();
            }

            _context.PaymentUserDetails.Remove(paymentUserDetail);
            await _context.SaveChangesAsync();

            return paymentUserDetail;
        }

        private bool PaymentUserDetailExists(int id)
        {
            return _context.PaymentUserDetails.Any(e => e.Id == id);
        }
    }
}
