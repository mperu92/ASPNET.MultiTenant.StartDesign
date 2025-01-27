namespace StartTemplateNew.Shared.Helpers.Const
{
    public static class ExceptionDefaults
    {
        public static class AppSettingNotFound
        {
            public const string DebuggerDisplay = "AppSettingNotFoundException: {Message}";
            public const string DefaultMessage = "The app setting could not be found.";
        }

        public static class TokenGeneration
        {
            public const string DebuggerDisplay = "TokenGenerationException: {Message}";
            public const string DefaultMessage = "An error occurred while generating the token.";
        }

        public static class PropertyNameNotFoundOnEntity
        {
            public const string DebuggerDisplay = "PropertyNameNotFoundOnEntityException: {Message}";
            public const string DefaultMessage = "The property name could not be found on the entity.";
        }
    }
}
