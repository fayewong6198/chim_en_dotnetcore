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
    public class PaymentProductDetailsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PaymentProductDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PaymentProductDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentProductDetail>>> GetPaymentProductDetails()
        {
            return await _context.PaymentProductDetails.ToListAsync();
        }

        // GET: api/PaymentProductDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentProductDetail>> GetPaymentProductDetail(int id)
        {
            var paymentProductDetail = await _context.PaymentProductDetails.FindAsync(id);

            if (paymentProductDetail == null)
            {
                return NotFound();
            }

            return paymentProductDetail;
        }

        // PUT: api/PaymentProductDetails/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaymentProductDetail(int id, PaymentProductDetail paymentProductDetail)
        {
            if (id != paymentProductDetail.Id)
            {
                return BadRequest();
            }

            _context.Entry(paymentProductDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentProductDetailExists(id))
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

        // POST: api/PaymentProductDetails
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PaymentProductDetail>> PostPaymentProductDetail(PaymentProductDetail paymentProductDetail)
        {
            _context.PaymentProductDetails.Add(paymentProductDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPaymentProductDetail", new { id = paymentProductDetail.Id }, paymentProductDetail);
        }

        // DELETE: api/PaymentProductDetails/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PaymentProductDetail>> DeletePaymentProductDetail(int id)
        {
            var paymentProductDetail = await _context.PaymentProductDetails.FindAsync(id);
            if (paymentProductDetail == null)
            {
                return NotFound();
            }

            _context.PaymentProductDetails.Remove(paymentProductDetail);
            await _context.SaveChangesAsync();

            return paymentProductDetail;
        }

        private bool PaymentProductDetailExists(int id)
        {
            return _context.PaymentProductDetails.Any(e => e.Id == id);
        }
    }
}
