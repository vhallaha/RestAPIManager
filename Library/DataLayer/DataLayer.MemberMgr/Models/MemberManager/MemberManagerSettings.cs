using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.Member.Enum;

namespace DataLayer.MemberMgr.Models
{
    public class MemberManagerSettings
    {
        //----------------------------------------
        // Member Manager Settings Properites
        //----------------------------------------

        /// <summary>
        /// PK:FK Member Manager Id
        /// </summary>
        [Key, ForeignKey("MemberManager")]
        public int Id { get; set; }

        /// <summary>
        /// Automatically validate newly created user account.
        /// </summary>
        public bool AutoValidateUser { get; set; }

        /// <summary>
        /// Make sure the uniqueness of the email address.
        /// </summary>
        public bool RestrictEmail { get; set; }

        /// <summary>
        /// Member Manager status
        /// </summary>
        public MemberManagerStatus Status { get; set; }

        //----------------------------------------
        // Db Virtual Connection
        //----------------------------------------

        public virtual MemberManager MemberManager { get; set; }
    }
}
