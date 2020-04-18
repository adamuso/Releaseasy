using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Releaseasy.Model
{
    public class User
    {
        public const string UsernameRegex = @"(?=.*[a-zA-Z])^[a-zA-Z0-9_]{3,32}$";
        public const string PasswordRegex = @"(?=.*[a-zA-Z])(?=.*[0-9])^[a-zA-Z0-9_!@#$%^&*]{8,64}$";

        public int Id { get; set; }
        public virtual ICollection<Project> CreatedProjects { get; set; }
        public virtual ICollection<ProjectUser> Projects { get; set; }
        [RegularExpression(UsernameRegex)]
        public string Username { get; set; }

        [RegularExpression(PasswordRegex)]
        public string Password { get; set; }

        [EmailAddress(ErrorMessage = "Specified email is invalid")]
        public string Email { get; set; }

        public bool EmailConfirmation { get; set; }


        public Role Role { get; set; }
    }
}