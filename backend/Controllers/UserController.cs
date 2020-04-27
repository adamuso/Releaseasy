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
        public ActionResult<IEnumerable<User>> Get()
        {
            return context.Users.ToArray();
        }

        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            return context.Users.Find(id);
        }

        [HttpGet("current")]
        public ActionResult<User> GetCurrentUser()
        {

            return null;
        }


        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<bool> RegisterUser([FromBody] NewUserData value)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
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
        public async Task<IActionResult> ForgotPassword([FromBody]string mail)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(mail);
                if (user != null && await userManager.IsEmailConfirmedAsync(user))
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResetLink = Url.Action("ResetPassword", "User",
                        new { email = mail, token = token },Request.Scheme);
                    string message = "Restart your password, clicking link below <br> <a href=\""
                            + passwordResetLink + "\" > " + passwordResetLink + "</a><br></div>";
                    await Task.Run(() => emailSender.SendEmailAsync(mail, "Reset Password", message));
                    return RedirectToAction("index", "home");
                }
            }
            return RedirectToAction("index", "home");
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] User value)
        {
            // Update an user
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            await userManager.DeleteAsync(user);

        }

        [HttpPost("Login")]
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
        public async Task<IActionResult> LogutAsync()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }
        public class UserLoginData
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}