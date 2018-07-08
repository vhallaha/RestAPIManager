namespace Utilities.Resource.Settings
{
    public static class ResourceManagerMessages
    {

        public static class Error
        {
            public const string CLAIM_ADD_ALREADY_EXISTS = "The claim that you are trying to add already exists.";
            public const string CLAIM_UPDATE_ALREADY_EXISTS = "The claim that you are trying to use already exists.";
            public const string CLAIM_UPDATE_NOT_FOUND = "The claim that you are trying to update does not exits.";
            public const string CLAIM_NOT_FOUND = "The claim that you are trying to use does not exits.";

            public const string CLIENT_ADD_ALREADY_EXISTS = "The client that you are trying to add already exists.";
            public const string CLIENT_NOT_FOUND = "The client that you are trying to access does not exists.";
            public const string CLIENT_KEY_NOT_FOUND = "The key that you are trying to access does not exists.";


            public const string RESOURCE_ALREADY_EXISTS = "The resource name that you are tryng to use already exists";
            public const string RESOURCE_NOT_FOUND = "The resource that you are trying to access does not exists.";
        }

        public static class Success
        {
            public const string RESOURCE_CREATED = "Resource has been successfully created.";
            public const string CLAIM_CREATED = "Resouce Claim has been successfully created.";
            public const string CLAIM_KEY_CREATED = "Claim Key has been created";
            public const string ACCESS_CREATED = "New Access to the resource has been added to the client.";
            public const string RESOUCE_UPDATED = "Resource has been successfully updated.";
            public const string CLAIM_UPDATED = "Resource Claim has been succesfully updated";
            public const string CLAIM_KEY_UPDATED = "Claim Key has been successfully updated"; 

            public const string RESOURCE_DELETED = "Resource has been successfully deleted.";
            public const string CLAIM_DELETED = "Claim has been successfully deleted.";
            public const string CLIENT_DELETED = "Client has successfully deleted.";
            public const string CLIENT_KEY_DELETED = "Client key has successfully deleted.";

        }

    }
}
