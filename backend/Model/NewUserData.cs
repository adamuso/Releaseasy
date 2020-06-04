using Releaseasy.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Releaseasy.backend.Model
{
    public class NewUserData
    {
        public int Id { get; set; }
        public virtual ICollection<Project> CreatedProjects { get; set; }
        public virtual ICollection<ProjectUser> Projects { get; set; }
        [EmailAddress(ErrorMessage = "Specified username is invalid")]
        public string Username { get; set; }
        public string Password { get; set; }
        [EmailAddress(ErrorMessage = "Specified email is invalid")]
        public string Email { get; set; }
        public bool EmailConfirmation { get; set; }
        public Role Role { get; set; }
    }
}
