using Microsoft.AspNetCore.Identity;

namespace GojoMarket.Models
{
    public class ApplicationUser :IdentityUser
    {
        public string Fullname { get; set; }
        public string Firstname { get; set; }
        public string lastname { get; set; }
    }
}
