using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Chim_En_DOTNET.Models;
using Chim_En_DOTNET.Data;
using Microsoft.EntityFrameworkCore;

namespace Chim_En_DOTNET.Areas.Identity.Pages.Account
{
  [AllowAnonymous]
  public class LoginModel : PageModel
  {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ApplicationDbContext _context;

    private readonly ILogger<LoginModel> _logger;
    private PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();


    public LoginModel(SignInManager<ApplicationUser> signInManager,
        ILogger<LoginModel> logger,
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _logger = logger;
      _context = context;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public IList<AuthenticationScheme> ExternalLogins { get; set; }

    public string ReturnUrl { get; set; }

    [TempData]
    public string ErrorMessage { get; set; }

    public class InputModel
    {
      [Required]

      [EmailAddress]
      public string Email { get; set; }

      [Required]
      [DataType(DataType.Password)]
      public string Password { get; set; }

      [Display(Name = "Remember me?")]
      public bool RememberMe { get; set; }
    }

    public async Task OnGetAsync(string returnUrl = null)
    {
      if (!string.IsNullOrEmpty(ErrorMessage))
      {
        ModelState.AddModelError(string.Empty, ErrorMessage);
      }

      returnUrl = returnUrl ?? Url.Content("~/");

      // Clear the existing external cookie to ensure a clean login process
      await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

      ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

      ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
      returnUrl = returnUrl ?? Url.Content("~/");

      if (ModelState.IsValid)
      {
        // This doesn't count login failures towards account lockout
        // To enable password failures to trigger account lockout, set lockoutOnFailure: true
        ApplicationUser user = await _context.ApplicationUser.FirstOrDefaultAsync(a => a.Email.Equals(Input.Email) && a.EmailConfirmed == true);
        var passwordHaser = new PasswordHasher<ApplicationUser>();
        if (user == null)
        {
          Console.WriteLine("User null");
          ModelState.AddModelError(string.Empty, "Email not found.");
          return Page();

        }

        // var result = await _signInManager.PasswordSignInAsync("trandaosimannh1", "1conkiencan", Input.RememberMe, lockoutOnFailure: false);
        if (user != null && passwordHasher.VerifyHashedPassword(user, user.PasswordHash, Input.Password) == PasswordVerificationResult.Success)
        {
          Console.WriteLine("Success");
          await _signInManager.SignInAsync(user, true, null);
          _logger.LogInformation("User logged in.");
          return LocalRedirect(returnUrl);
        }
        // if (result.RequiresTwoFactor)
        // {
        //   return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
        // }
        // if (result.IsLockedOut)
        // {
        //   Console.WriteLine("Lockout");

        //   _logger.LogWarning("User account locked out.");
        //   return RedirectToPage("./Lockout");
        // }
        else
        {
          Console.WriteLine("Error");
          Console.WriteLine(Input.Email);
          Console.WriteLine(Input.Password);
          ModelState.AddModelError(string.Empty, "Invalid login attempt.");
          return Page();
        }
      }

      // If we got this far, something failed, redisplay form
      return Page();
    }
  }
}
