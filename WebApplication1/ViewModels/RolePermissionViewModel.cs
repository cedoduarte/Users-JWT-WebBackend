namespace WebApplication1.ViewModels
{
    public class RolePermissionViewModel
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public int PermissionId { get; set; }
        public string? PermissionName { get; set; }
    }
}
