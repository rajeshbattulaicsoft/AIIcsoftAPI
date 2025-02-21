using System.ComponentModel.DataAnnotations;

namespace AIIcsoftAPI.Dto
{
    public class UserAuthorizeDTO
    {
        [Required]
        public int userId { get; set; }
        [Required]
        public string masterName { get; set; }
        [Required]
        public int locationId { get; set; }
        public string pAction { get; set; }
        [Required]
        public int userRoleId { get; set; }
        public string permissionAllowed { get; set; } = "N";

    }
}
