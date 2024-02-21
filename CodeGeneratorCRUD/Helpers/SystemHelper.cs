namespace System 
{
    public static class SystemHelper
    {
        public static bool IsNullOrWhitespace(this string source) => string.IsNullOrWhiteSpace(source);
    }
}