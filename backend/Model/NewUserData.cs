using Releaseasy.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Releaseasy.backend.Model
{
    public class NewUserData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [RegularExpression("(employee|company)", ErrorMessage = "Type must be set to 'employee' or 'company'")]
        public string Type { get; set; }
        public string LastName { get; set; }
        public string Location {get; set; }
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
