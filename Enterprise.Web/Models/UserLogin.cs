namespace Enterprise.Web.Models
{
    public class UserLogin
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class LoginResponseModel
    {
        public string? LogInStatus { get; set; }
        public string? Message { get; set; }
        public string? Token { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? RoleName { get; set; }
        public string? RoleId { get; set; }
    }
    public class RegisterViewRequestModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? MobileNumber { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string RoleName { get; set; }
        public DateTime AddedOn { get; set; }
        public string AddedBy { get; set; }
    }
    public class RegisterResponseModel
    {
        public string? Status { get; set; }
        public string? Message { get; set; }
        public string? Password { get; set; }

    }
}
