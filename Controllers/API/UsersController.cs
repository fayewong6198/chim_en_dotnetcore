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
  public class UsersController : ControllerBase
  {
    private readonly ApplicationDbContext _context;

    public UsersController(ApplicationDbContext context)
    {
      _context = context;

    }

    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetApplicationUser()
    {

      return await _context.ApplicationUser.ToListAsync();
    }

    // GET: api/Users/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ApplicationUser>> GetApplicationUser(string id)
    {
      var applicationUser = await _context.ApplicationUser.FindAsync(id);

      if (applicationUser == null)
      {
        return NotFound();
      }

      return applicationUser;
    }

    // PUT: api/Users/5
    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    [HttpPut("{id}")]
    public async Task<IActionResult> PutApplicationUser(string id, ApplicationUser applicationUser)
    {
      if (id != applicationUser.Id)
      {
        return BadRequest();
      }

      _context.Entry(applicationUser).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!ApplicationUserExists(id))
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

    // POST: api/Users
    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    [HttpPost]
    public async Task<ActionResult<ApplicationUser>> PostApplicationUser(ApplicationUser applicationUser)
    {
      _context.ApplicationUser.Add(applicationUser);
      try
      {

        await _context.SaveChangesAsync();
      }
      catch (DbUpdateException)
      {
        if (ApplicationUserExists(applicationUser.Id))
        {
          return Conflict();
        }
        else
        {
          throw;
        }
      }

      return CreatedAtAction("GetApplicationUser", new { id = applicationUser.Id }, applicationUser);
    }

    // DELETE: api/Users/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApplicationUser>> DeleteApplicationUser(string id)
    {
      var applicationUser = await _context.ApplicationUser.FindAsync(id);
      if (applicationUser == null)
      {
        return NotFound();
      }

      _context.ApplicationUser.Remove(applicationUser);
      await _context.SaveChangesAsync();

      return applicationUser;
    }

    private bool ApplicationUserExists(string id)
    {
      return _context.ApplicationUser.Any(e => e.Id == id);
    }
  }
}
