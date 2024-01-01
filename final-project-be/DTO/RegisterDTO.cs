namespace final_project_be.DTO
{
    public class RegisterDTO
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public bool IsActive { get; set; }
    }
}