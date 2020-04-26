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
using System.Text.RegularExpressions;
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

        /* public UserController(ReleaseasyContext context)
         {
             this.context = context;
             hashingAlgorithm = System.Security.Cryptography.SHA256.Create();
         }*/


        private readonly IEmailSender _emailSender;

        public UserController(IEmailSender emailSender, ReleaseasyContext context,ILogger<UserController> logger,UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _emailSender = emailSender;
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
          await _emailSender.SendEmailAsync(email, subject, message);


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

    
       
                //View(result.Succeeded ? "ConfirmEmail" : "Error");


        [HttpPost("Register")]
      [AllowAnonymous]
        public async Task<bool> RegisterUser([FromBody] NewUserData value)
        {

           // var emailValidator = new EmailAddressAttribute();
           
          //  if (!emailValidator.IsValid(value.UserName)) {
           //     throw new ArgumentException("Specified username is invalid. The username must be an email address", "value");
          //  }

            // if (!Regex.IsMatch(value.Username, @"(?=.*[a-zA-Z])^[a-zA-Z0-9_]{3,32}$"))
            //     throw new ArgumentException("Specified username is invalid. The username must have " +
            //         "at least 3 characters and one letter. It can contain letters, numbers and undescore " +
            //         "character.", "value");

           /* if (!Regex.IsMatch(value.Password, @"(?=.*[a-zA-Z])(?=.*[0-9])^[a-zA-Z0-9_!@#$%^&*]{8,64}$"))
                throw new ArgumentException("Specified password is invalid. The password must have " +
                    "at least 8 characters, one letter and one number. It can contain letters, numbers and undescore " +
                    "character.", "value");*/



            //byte[] passwordBytes = Encoding.ASCII.GetBytes(value.Password);
            //byte[] passwordHashed = hashingAlgorithm.ComputeHash(passwordBytes);

            //            value.Password = Convert.ToBase64String(passwordHashed);*/
            // value.EmailConfirmation = false;


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

                    // logger.Log(LogLevel.Warning, confirmationLink);

                    string message = " Dziekujemy za rejestracje <br> <a href=\"" + confirmationLink+ "\" > "+ confirmationLink +"</a><br></div>";
                    await Task.Run(() => _emailSender.SendEmailAsync(value.Email, "Account Activation", message));

                    //context.Add(value);
                    // context.SaveChanges();


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
                
                return RedirectToAction("index", "home");
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
        public void Delete(int id)
        {
        }

        [HttpPost("Login")]
        public async System.Threading.Tasks.Task<bool> LoginAsync([FromBody] UserLoginData loginData)
        {
            User user = context.Users.SingleOrDefault(u => u.UserName == loginData.Username);

            if (user == null)
                throw new ArgumentException("User or password are invalid", "loginData");

            if(user.Password != Convert.ToBase64String(hashingAlgorithm.ComputeHash(Encoding.ASCII.GetBytes(loginData.Password))))
                throw new ArgumentException("User or password are invalid", "loginData");

            Claim[] claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, "")
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties()
            {

            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            return true;
        }

        public class UserLoginData
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}