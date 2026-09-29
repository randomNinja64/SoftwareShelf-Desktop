using System;

namespace SoftwareShelf_Desktop
{
    // Extensions left unchecked by default in the download form, per search Type.
    internal static class FileExclusions
    {
        private static readonly string[] Sidecars = { ".xml", ".sqlite", ".torrent" };
        private static readonly string[] ImageFiles = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
        private static readonly string[] AudioSidecars = { ".ffp.txt" };

        public static bool IsExcluded(string category, string fileName)
        {
            string name = fileName.Substring(fileName.LastIndexOf('/') + 1).ToLower();
            if (name.Contains("_thumb") || Array.Exists(Sidecars, extension => name.EndsWith(extension)))
            {
                return true;
            }

            switch (category)
            {
                case "Audio":
                    return Array.Exists(ImageFiles, extension => name.EndsWith(extension)) || Array.Exists(AudioSidecars, extension => name.EndsWith(extension));
                case "Books":
                case "Movies":
                case "Software":
                    return Array.Exists(ImageFiles, extension => name.EndsWith(extension));
                default:
                    return false;
            }
        }
    }
}
