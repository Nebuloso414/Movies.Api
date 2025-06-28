namespace Movies.Api.Auth
{
    public static class AuthConstants
    {
        public const string AdminUserPolicyName = "Admin";
        public const string AdminUserClaimName = "admin";

        public const string TrustedMemberPolicyName = "Trusted";
        public const string TrustedMemberClaimName = "trusted_member";

        public const string ApiKeyHeaderName = "x-api-key";
        public const string ApiKeyUserId = "74e20de1-8dd0-4bc2-a9f5-8aa3203ad209";
    }
}
