namespace Utilities.Resource.Enums
{
    public enum ClientResourceAccessStatus
    {
        /// <summary>
        /// Allow Users to retrieve resource from this client
        /// </summary>
        Allow = 1,

        /// <summary>
        /// Client already requested access to the resource
        /// which is currently in review by the resource holders.
        /// </summary>
        Pending = 2,

        /// <summary>
        /// Resource holders blocks access to the data using 
        /// the said client.
        /// </summary>
        Block = 3

    }
}
