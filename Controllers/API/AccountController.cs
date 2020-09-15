using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Chim_En_DOTNET.Data;
using Chim_En_DOTNET.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Collections;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Chim_En_DOTNET.Controllers.API
{
  [Route("api/[controller]")]
  [ApiController]
  public class AccountController : ControllerBase
  {
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;

    private PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();
    public AccountController(ApplicationDbContext context, IConfiguration configuration, UserManager<ApplicationUser> userManager)
    {
      _context = context;
      _configuration = configuration;
      _userManager = userManager;
    }

    // Login
    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginParams loginParams)
    {
      // Console.WriteLine("asdddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd");
      ApplicationUser user = await _context.ApplicationUser.FirstOrDefaultAsync(a => a.Email.Equals(loginParams.Email) && a.EmailConfirmed == true);
      Console.WriteLine("asdasd");
      if (user == null)
        return BadRequest(new { msg = "Wrong Email" });

      else if (passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginParams.Password) != PasswordVerificationResult.Success)
        return BadRequest(new { msg = "Wrong Password" });

      return Ok(new
      {
        token = CreateToken(user),
        user = new
        {
          id = user.Id,
          userName = user.UserName,
          email = user.Email,
          emailConfirmed = user.EmailConfirmed,
          phoneNumber = user.PhoneNumber,
          role = user.Role
        }
      });
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterParams registerParams)
    {
      ApplicationUser registerUser = new ApplicationUser { Email = registerParams.Email, UserName = registerParams.UserName, EmailConfirmed = true };

      var result = await _userManager.CreateAsync(registerUser, registerParams.Password);

      if (result.Succeeded)
      {
        return Ok(new
        {
          token = CreateToken(registerUser)
        });
      }

      else
      {
        var response = new Hashtable();
        foreach (var error in result.Errors)
        {

          response.Add(error.Code, error.Description);
        }
        return BadRequest(response);
      }


    }

    // Get user information
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("User")]
    public async Task<IActionResult> GetUser()
    {
      Console.WriteLine(ClaimTypes.NameIdentifier);

      foreach (var val in User.Claims)
      {
        Console.Write(val.Type + " ");
        Console.WriteLine(val.Value);
      }

      string id = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value;
      ApplicationUser user = await _context.ApplicationUser.FirstOrDefaultAsync(a => a.Id.Equals(id));

      if (user == null)
      {
        return Unauthorized();
      }
      return Ok(new
      {
        id = user.Id,
        userName = user.UserName,
        email = user.Email,
        emailConfirmed = user.EmailConfirmed,
        phoneNumber = user.PhoneNumber,
        role = user.Role
      });
    }

    [HttpPost("SessionId")]
    public async Task<IActionResult> GenerateSessionId()
    {
      return Ok(new
      {
        sessionId = CreateSessionId()
      });
    }

    private Guid CreateSessionId()
    {
      Guid guid = Guid.NewGuid();
      return guid;
    }
    private string CreateToken(ApplicationUser user)
    {
      SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

      SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

      List<Claim> claims = new List<Claim> {
          new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
          new Claim(ClaimTypes.Name, user.UserName.ToString()),
          new Claim("SuperUser", user.isSuperUser.ToString()),
          new Claim("Staff", user.isStaff.ToString())
      };

      SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.Now.AddDays(1),
        SigningCredentials = creds
      };

      JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
      SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

      return tokenHandler.WriteToken(token); //token;
    }


  }

  public class LoginParams
  {
    public string Email { get; set; }
    public string Password { get; set; }
  }

  public class RegisterParams
  {
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
  }
}