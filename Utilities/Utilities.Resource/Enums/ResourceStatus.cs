namespace Utilities.Resource.Enums
{
    public enum ResourceStatus
    {
        /// <summary>
        /// Live in Public to use
        /// </summary>
        Live = 1,

        /// <summary>
        /// Live but only admins has access
        /// </summary>
        Restricted = 2,

        /// <summary>
        /// Unaccessible to both admins and normal users.
        /// </summary>
        Pending = 3,

        /// <summary>
        /// Closed all traffic from this resource.
        /// </summary>
        Blocked = 4

    }
}
