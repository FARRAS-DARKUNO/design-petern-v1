namespace final_project_be.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}