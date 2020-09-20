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
  public class UsersController : ControllerBase
  {
    private readonly ApplicationDbContext _context;

    public UsersController(ApplicationDbContext context)
    {
      _context = context;

    }

    // GET: api/Users
    [HttpGet]
    public async Task<IActionResult> GetApplicationUser([FromQuery] UserFilter filter)
    {
      UserFilter validFilter = new UserFilter(filter.PageNumber, filter.PageSize, filter.Search, filter.OrderBy, filter.IsSuperUser, filter.IsStaff);

      var users = from u in _context.ApplicationUser select u;
      // Filter
      if (validFilter.IsStaff != null)
      {
        users = users.Where(u => u.IsStaff == validFilter.IsStaff);
      }

      if (validFilter.IsSuperUser != null)
      {
        users = users.Where(u => u.IsSuperUser == validFilter.IsSuperUser);
      }

      if (!string.IsNullOrEmpty(validFilter.Search))
      {
        users = users.Where(u => u.FullName.Contains(validFilter.Search) || u.Email.Contains(validFilter.Search) || u.UserName.Contains(validFilter.Search));
      }

      // Sort
      switch (validFilter.OrderBy.ToLower())
      {
        case "-createdat":
          users = users.OrderByDescending(u => u.CreatedAt);
          break;

        case "username":
          users = users.OrderBy(u => u.UserName);
          break;
        case "-username":
          users = users.OrderByDescending(u => u.UserName);
          break;

        case "email":
          users = users.OrderBy(u => u.Email);
          break;
        case "-email":
          users = users.OrderByDescending(u => u.Email);
          break;
        default:
          users = users.OrderBy(u => u.CreatedAt);
          break;
      }

      // Pagination
      users = users.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize);


      var data = await users.Select(u => new UserResponse()
      {
        Id = u.Id,
        UserName = u.UserName,
        Email = u.Email,
        FullName = u.FullName,
        IsStaff = u.IsStaff,
        IsSuperUser = u.IsSuperUser,
        CreatedAt = u.CreatedAt,
      }).ToListAsync();

      var totalRecord = await _context.ApplicationUser.CountAsync();

      int totalRecords = await _context.Users.CountAsync();
      return Ok(new PagedResponse<List<UserResponse>>(data, validFilter.PageNumber, validFilter.PageSize, totalRecord));
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
      return _context.ApplicationUser.Any(e => e.Id.Equals(id));
    }

    public class UserResponse
    {
      public string Id { get; set; }
      public string UserName { get; set; }
      public string Email { get; set; }
      public string FullName { get; set; }
      public string PhoneNumber { get; set; }
      public bool IsStaff { get; set; }
      public bool IsSuperUser { get; set; }
      public DateTime CreatedAt { get; set; }

      public UserResponse() { }

    };
  }
}
