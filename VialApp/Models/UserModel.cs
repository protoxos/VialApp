using System.ComponentModel.DataAnnotations;

namespace VialApp.Models
{
    public class UserModel : BaseModel
    {
        #region holders
        string firstname = "";
        string lastname = "";
        string email = "";
        string username = "";
        #endregion

        public Guid InternalId { get; set; }
        public string Firstname { get { return firstname; } set { firstname = value.Trim(); } }
        public string Lastname { get { return lastname; } set { lastname = value.Trim(); } }
        public string Email { get { return email; } set { email = value.Trim(); } }
        public string Username { get { return username; } set { username = value.Trim(); } }
        
        public string Password { get; set; } = "";
        public bool Active { get; set; }
    }
}
