namespace StartTemplateNew.Shared.Models.Dto.Identity
{
    public class UserRole
    {
        public UserRole() { }
        public UserRole(Guid userId, Guid roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        public Guid UserId { get; set; }
        public User? User { get; set; }
        public Guid RoleId { get; set; }
        public Role? Role { get; set; }
    }
}
