namespace Utilities.Member.Enum
{
    public enum MemberManagerStatus
    {
        /// <summary>
        /// Waiting for email or other form of confirmation.
        /// </summary>
        Pending = 0,

        /// <summary>
        /// Member Manager activated.
        /// </summary>
        Active = 1,

        /// <summary>
        /// Member Manager deactivated.
        /// </summary>
        Inactive = 2,

        /// <summary>
        /// Member Manager banned from the website.
        /// </summary>
        Banned = 3
    }
}
