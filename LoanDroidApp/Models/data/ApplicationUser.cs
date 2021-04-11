// =============================
// Email: bluestar1027@hotmail.com

// =============================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Models.data.Interfaces;

namespace Models.data
{
    public class ApplicationUser : IdentityUser, IAuditableEntity
    {
        public virtual string FriendlyName
        {
            get
            {
                string friendlyName = string.IsNullOrWhiteSpace(LastName) ? FirstName : $"{FirstName} {LastName}";
                return friendlyName;
            }
        }

        public Byte[] AvatarImage { get; set; } 
        public string JobTitle { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Sex { get; set; }
        public DateTime Birth { get; set; }
        public string Operator { get; set; }
        public string OtherPhone { get; set; }
        public string OfficeNumber { get; set; }
        public string Address { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsLockedOut => this.LockoutEnabled && this.LockoutEnd >= DateTimeOffset.UtcNow;

        public string CreatedBy { get; set; }
        public string CreatedDevice { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDevice { get; set; }
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Navigation property for the roles this user belongs to.
        /// </summary>
        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        /// <summary>
        /// Navigation property for the claims this user possesses.
        /// </summary>
        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        /// <summary>
        /// Demo Navigation property for orders this user has processed
        /// </summary>
    }
}
