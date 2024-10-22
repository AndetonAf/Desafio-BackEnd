namespace BackEnd.Helpers
{
    public static class FileHelpers
    {
        public static (bool success, string extension, string contentType) DetectExtension(string base64)
        {
            Dictionary<string, (string extension, string contentType)> signatures = new()
            {
                {"iVBORw0KGgo", ("png", "image/png")},
                {"Qk", ("bmp", "image/bmp")}
            };

            foreach (var entry in signatures)
            {
                if (base64.StartsWith(entry.Key))
                {
                    return (true, entry.Value.extension, entry.Value.contentType);
                }
            }

            return (false, string.Empty, string.Empty);
        }
    }
}
