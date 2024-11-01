namespace WebApplication1.Shared
{
    public static class Constants
    {
        public static class Jwt
        {
            public const string UserIdClaim = "UserId";
            public const int TokenExpirationSeconds = 86400; // 24 hours
        }

        public static class Permissions
        {
            public const string Create = "create";
            public const string Read = "read";
            public const string Update = "update";
            public const string Delete = "delete";
        }
    }
}
