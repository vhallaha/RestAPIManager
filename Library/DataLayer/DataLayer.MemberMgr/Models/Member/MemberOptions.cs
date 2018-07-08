using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.MemberMgr.Models
{
    public class MemberOptions
    {

        //----------------------------------------
        // Member Option Required Properites
        //----------------------------------------
        [Key, ForeignKey("Member")]
        public int Id { get; set; }

        /// <summary>
        /// Validated
        /// </summary>
        public bool IsValidated { get; set; }

        /// <summary>
        /// Reset Token
        /// </summary>
        public string ResetToken { get; set; }

        /// <summary>
        /// Email Token
        /// </summary>
        public string EmailToken { get; set; }

        //----------------------------------------
        // Db Virtual Connection
        //----------------------------------------

        /// <summary>
        /// Member
        /// </summary>   
        public virtual Member Member { get; set; }

    }
}
