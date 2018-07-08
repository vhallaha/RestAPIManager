namespace Utilities.Resource.Enums
{
    public enum ClientKeyStatus
    {
        /// <summary>
        /// Closed API key, unless opened no connection using this key will be executed.
        /// Close Keys are usually generated based on when the user started to ask for a new key.
        /// or when the key itself is compromised. 
        /// NOTE: cannot be re-open.
        /// </summary>
        Revoked = 1,

        /// <summary>
        /// Closed API key, that is waiting to be opened by the app provider.
        /// </summary>
        Pending = 2,

        /// <summary>
        /// Opened API key, ready to be use to access endpoints in the server.
        /// </summary>
        Open = 3
    }
}
