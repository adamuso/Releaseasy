using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Releaseasy.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Releaseasy.Services;
using Task = System.Threading.Tasks.Task;
using Microsoft.Extensions.Logging;
using Releaseasy.backend.Model;
using Microsoft.EntityFrameworkCore;

namespace Releaseasy.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        private readonly ReleaseasyContext context;
        private readonly System.Security.Cryptography.SHA256 hashingAlgorithm;

        private readonly ILogger<UserController> logger;
        private readonly IEmailSender emailSender;

        public UserController(IEmailSender emailSender, ReleaseasyContext context, ILogger<UserController> logger, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.emailSender = emailSender;
            this.context = context;
            hashingAlgorithm = System.Security.Cryptography.SHA256.Create();
            this.logger = logger;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet("TestMail")]
        public async Task<bool> TestMail()
        {
            var email = "andrzej.wyzgol@gmail.com";
            var subject = "Test Releaseasy";
            var message = "Test message";
            await emailSender.SendEmailAsync(email, subject, message);


            return true;
        }

        [HttpGet("InitializeDatabase")]
        public ActionResult<bool> InitializeDatabase()
        {
            context.Database.EnsureCreated();

            return true;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<User>> Get()
        {
            return context.Users.ToArray();
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<User> Get(string id)
        {
            var user = context.Users.Find(id);

            return Ok(new {
                user.Name,
                user.LastName,
                user.Type,
                user.Location,
                user.Email,
                user.EmailConfirmed,
                user.Id,
                user.PhoneNumber,
                user.PhoneNumberConfirmed,
                user.UserName
            });
        }

        [HttpGet("Current")]
        [Authorize]
        public async Task<ActionResult<User>> GetCurrentUser()
        {
            var user = await userManager.GetUserAsync(User);

            return Ok(new {
                user.Name,
                user.LastName,
                user.Type,
                user.Location,
                user.Email,
                user.EmailConfirmed,
                user.Id,
                user.PhoneNumber,
                user.PhoneNumberConfirmed,
                user.UserName
            });
        }

        [HttpGet("CreatedProjects")]
        [Authorize]
        public async Task<ActionResult<ICollection<object>>> GetCreatedProjects()
        {
            var user = await userManager.GetUserAsync(User);

            user = context.Users.Where(u => u.Id == user.Id).Include(u => u.CreatedProjects).Single();

            return Ok(user.CreatedProjects.Select(p => new
            {
                p.Description,
                p.EndTime,
                p.Id,
                p.Name,
                p.StartTime
            }));
        }



        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<bool> RegisterUser([FromBody] NewUserData value)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Name = value.Name,
                    LastName = value.LastName,
                    Location = value.Location,
                    Type = value.Type,
                    UserName = value.Email,
                    Email = value.Email,
                };

                IdentityResult result = await userManager.CreateAsync(user, value.Password);
                if (result.Succeeded)
                {
                    try
                    {

                        string ctoken = await userManager.GenerateEmailConfirmationTokenAsync(user);
                        string confirmationLink = Url.Action("ConfirmEmail", "user", new
                        {
                            userId = user.Id,
                            token = ctoken
                        }, Request.Scheme);

                        string message = "Thank you for registering, before logging in please activate your account by clicking the link below <br> <a href=\""
                            + confirmationLink + "\" > " + confirmationLink + "</a><br></div>";
                        await Task.Run(() => emailSender.SendEmailAsync(value.Email, "Account Activation", message));
                    }
                    catch (ValidationException ex)
                    {
                        throw;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                else {
                    return false;
                }
            }
            return true;
        }

        [HttpGet("ConfirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {

            if (userId == null || token == null)
            {

                return RedirectToAction("index", "home");
            }
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {

                return RedirectToAction("index", "home");
                ;
            }
            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                return Redirect("/User");
            }

            return RedirectToAction("index", "home");

        }
        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody]Mail mail)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(mail.EmailAddress);
                if (user != null && await userManager.IsEmailConfirmedAsync(user))
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResetLink = Url.Action("ResetPassword", "User",
                        new { email = mail.EmailAddress, token = token }, Request.Scheme);
                    string message = "Restart your password, clicking link below <br> <a href=\""
                            + passwordResetLink + "\" > " + passwordResetLink + "</a><br></div>";
                    await Task.Run(() => emailSender.SendEmailAsync(mail.EmailAddress, "Reset Password", message));
                    return RedirectToAction("index", "home");
                }
            }
            return RedirectToAction("index", "home");
        }
        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordUser ResetUser)
        {

            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(ResetUser.Email);

                if (user != null)
                {
                    var result = await userManager.ResetPasswordAsync(user, ResetUser.Token, ResetUser.Password);
                    if (result.Succeeded)
                    {
                        await signInManager.SignInAsync(user, isPersistent: false);
                        return Redirect("/User");
                    }
                }
            }
            return RedirectToAction("index", "home");
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        [Authorize]
        public void Put(int id, [FromBody] User value)
        {
            // Update an user
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task DeleteAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            await userManager.DeleteAsync(user);
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async System.Threading.Tasks.Task<bool> LoginAsync([FromBody] UserLoginData loginData)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(loginData.Username, loginData.Password, true, false);
                if (result.Succeeded)
                {
                    return true;

                }

            }
            return false;
        }

        [HttpPost("Logout")]
        [Authorize]
        public async Task<IActionResult> LogutAsync()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }
        public class Mail
        {
            [Required]
            [EmailAddress]
            public string EmailAddress { get; set; }
        }
        public class UserLoginData
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}