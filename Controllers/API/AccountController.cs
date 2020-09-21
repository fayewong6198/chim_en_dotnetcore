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
using MailKit.Net.Smtp;
using MimeKit;
using Chim_En_DOTNET.Helpers;
using System.Security.Authentication;

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
      ApplicationUser user = await _context.ApplicationUser.FirstOrDefaultAsync(a => a.Email.Equals(loginParams.Email));
      Console.WriteLine("asdasd");
      if (user == null)
        return BadRequest(new { msg = "Wrong Email" });

      else if (passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginParams.Password) != PasswordVerificationResult.Success)
        return BadRequest(new { msg = "Wrong Password" });

      else if (user.EmailConfirmed == false)
      {
        return BadRequest(new { msg = "Please confirm your email" });
      }

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
      Console.WriteLine(1);
      ApplicationUser registerUser = new ApplicationUser { Email = registerParams.Email, UserName = registerParams.UserName, EmailConfirmed = false };
      Console.WriteLine(2);
      var result = await _userManager.CreateAsync(registerUser, registerParams.Password);
      Console.WriteLine(3);

      if (result.Succeeded)
      // if (true)
      {
        Console.WriteLine(4);
        var emailConfirmedToken = await _userManager.GenerateEmailConfirmationTokenAsync(registerUser);
        var confirmLink = Url.Action(nameof(ConfirmEmail), "Account", new { emailConfirmedToken, email = registerUser.Email }, Request.Scheme);
        Console.WriteLine(5);

        EmailMessage message = new EmailMessage();
        message.Sender = new MailboxAddress("Self", "Trandaosimanh@gmail.com");
        message.Reciever = new MailboxAddress("Self", registerUser.Email);
        message.Subject = "Confirm Email";
        message.Content = confirmLink;
        Console.WriteLine(6);

        var mimeMessage = message.CreateMimeMessageFromEmailMessage(message);
        Console.WriteLine(7);

        using (var smtpClient = new SmtpClient())
        {
          smtpClient.CheckCertificateRevocation = false;
          smtpClient.SslProtocols = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;

          smtpClient.Connect("smtp.mailtrap.io", 2525);
          Console.WriteLine(8);

          await smtpClient.AuthenticateAsync("4d8fccd77e8800", "6fdb8b54c5aa6b");
          Console.WriteLine(9);

          await smtpClient.SendAsync(mimeMessage);
          Console.WriteLine(10);

          await smtpClient.DisconnectAsync(true);
        }


        return Ok(new
        {
          msg = "Email sent"
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

    [HttpGet("ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmail(string emailConfirmedToken, string email)
    {
      Console.WriteLine(emailConfirmedToken);
      Console.WriteLine(email);
      var user = await _userManager.FindByEmailAsync(email);

      if (user == null)
      {
        return BadRequest();
      }

      var result = await _userManager.ConfirmEmailAsync(user, emailConfirmedToken);

      if (result.Succeeded)
      {
        return Ok(new
        {
          token = CreateToken(user)
        });
      }
      return BadRequest();
    }

    [HttpPost("GetEmailConfirm")]
    public async Task<IActionResult> GetEmailConfirmLink([FromBody] string email)
    {
      var user = await _userManager.FindByEmailAsync(email);

      if (user == null)
      {
        return BadRequest(new { msg = "Email not found" });
      }

      var emailConfirmedToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
      var confirmLink = Url.Action(nameof(ConfirmEmail), "Account", new { emailConfirmedToken, email = user.Email }, Request.Scheme);


      EmailMessage message = new EmailMessage();
      message.Sender = new MailboxAddress("Self", "Trandaosimanh@gmail.com");
      message.Reciever = new MailboxAddress("Self", email);
      message.Subject = "Confirm Email";
      message.Content = confirmLink;


      var mimeMessage = message.CreateMimeMessageFromEmailMessage(message);


      using (var smtpClient = new SmtpClient())
      {
        smtpClient.CheckCertificateRevocation = false;
        smtpClient.SslProtocols = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;

        smtpClient.Connect("smtp.mailtrap.io", 2525);


        await smtpClient.AuthenticateAsync("4d8fccd77e8800", "6fdb8b54c5aa6b");


        await smtpClient.SendAsync(mimeMessage);


        await smtpClient.DisconnectAsync(true);
      }


      return Ok(new
      {
        msg = "Email sent"
      });

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

    [Authorize]
    [HttpPost("ChangePassword")]
    public async Task<IActionResult> ChangePassword(ChangePasswordParams changePasswordParams)
    {
      if (!changePasswordParams.NewPassword.Equals(changePasswordParams.ConfirmPassword))
      {
        return BadRequest(new { msg = "Wrong password confirm" });
      }

      string id = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value;
      ApplicationUser user = await _context.ApplicationUser.FindAsync(id);

      if (user == null)
      {
        return BadRequest(new { msg = "User not found" });
      }

      if (passwordHasher.VerifyHashedPassword(user, user.PasswordHash, changePasswordParams.OldPassword) != PasswordVerificationResult.Success)
      {
        return BadRequest(new { msg = "Password does not match" });
      }


      user.PasswordHash = passwordHasher.HashPassword(user, changePasswordParams.NewPassword);

      await _context.SaveChangesAsync();

      return Ok();
    }

    [HttpPost("ChangeInfo")]
    [Authorize]
    public async Task<IActionResult> ChangeInfo(ChangeInfoParams changeInfoParams)
    {
      string id = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value;
      var user = await _context.ApplicationUser.FindAsync(id);

      if (user == null)
      {
        return NotFound(new { msg = "User not found" });
      }

      user.FullName = changeInfoParams.FullName;
      user.PhoneNumber = changeInfoParams.PhoneNumber;
      user.Gender = changeInfoParams.Gender;
      user.DateOfBirth = changeInfoParams.DateOfBirth;

      await _context.SaveChangesAsync();
      user = await _context.ApplicationUser.FindAsync(id);

      return Ok(user);
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
          new Claim("SuperUser", user.IsSuperUser.ToString()),
          new Claim("Staff", user.IsStaff.ToString())
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

  public class ChangePasswordParams
  {
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }
  }

  public class ChangeInfoParams
  {
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public GenderChoices Gender { get; set; }

    public DateTime DateOfBirth { get; set; }
  }
}