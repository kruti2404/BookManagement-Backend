namespace AuthDTOs
{
    public class UserDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string JWT { get; set; }
        public string RefreshToken { get; set; }
    }
}
