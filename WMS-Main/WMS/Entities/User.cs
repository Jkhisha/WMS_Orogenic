using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace WareHouseMVC.Models
{
    public class User
    {
        [Key]
        public virtual Guid UserId { get; set; }

        [Required]
        public virtual String Username { get; set; }

        public virtual String Email { get; set; }

        [Required, DataType(DataType.Password)]
        public virtual String Password { get; set; }

        public virtual String FirstName { get; set; }
        public virtual String LastName { get; set; }
        public virtual String MobileNumber { get; set; }

        [DefaultValue(-1)]
        public virtual long? WarehouseID { get; set; }
        public virtual String WarehouseName { get; set; }

        [DataType(DataType.MultilineText)]
        public virtual String Comment { get; set; }

        public virtual Boolean IsApproved { get; set; }
        public virtual int PasswordFailuresSinceLastSuccess { get; set; }
        public virtual DateTime? LastPasswordFailureDate { get; set; }
        public virtual DateTime? LastActivityDate { get; set; }
        public virtual DateTime? LastLockoutDate { get; set; }
        public virtual DateTime? LastLoginDate { get; set; }
        public virtual String ConfirmationToken { get; set; }
        public virtual DateTime? CreateDate { get; set; }
        public virtual Boolean IsLockedOut { get; set; }
        public virtual DateTime? LastPasswordChangedDate { get; set; }
        public virtual String PasswordVerificationToken { get; set; }
        public virtual DateTime? PasswordVerificationTokenExpirationDate { get; set; }

        #region Client User Logo & Text

        public string LogoUrl { get; set; }
        public string BannerText { get; set; }
        public string ThemeName { get; set; }
        #endregion

        public virtual ICollection<Role> Roles { get; set; }
        //public Guid[] roleids { get; set; }
    }

    public class UserforEdit
    {
        public virtual String Username { get; set; }
        public virtual List<string> Roles { get; set; }
        public virtual String Email { get; set; }
    }

    }