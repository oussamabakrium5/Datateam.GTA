namespace Datateam.Security
{
    public class LoginUser
    {
        public Guid? Id { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
    }
}
