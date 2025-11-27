namespace TrabajoPracticoApp.Dtos
{
    public class LoginResponseDto
    {
        public string? AccesToken;
        public string? RefreshToken;
        public DateTime ExpireAt;
        public int RoleId { get; set; }
    }
}
