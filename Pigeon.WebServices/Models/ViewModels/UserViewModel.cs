namespace Pigeon.WebServices.Models.ViewModels
{
    using Pigeon.Models;

    public class UserViewModel
    {
        public UserViewModel()
        {
        }

        public UserViewModel(User userDbModel)
        {
            this.Username = userDbModel.UserName;
            this.Email = userDbModel.Email;
            this.FirstName = userDbModel.FirstName;
            this.LastName = userDbModel.LastName;
        }

        public string Username { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}