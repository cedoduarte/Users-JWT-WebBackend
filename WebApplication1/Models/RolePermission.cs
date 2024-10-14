﻿using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class RolePermission
    {
        [Key]
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public virtual Role? Role { get; set; }
        public virtual Permission? Permission { get; set; }
    }
}
