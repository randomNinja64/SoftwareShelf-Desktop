namespace SoftwareShelf_Desktop
{
    internal static class SizeFormatter
    {
        public static string Format(long bytes)
        {
            string[] units = { "B", "KB", "MB", "GB", "TB" };
            double value = bytes;
            int unit = 0;
            while (value >= 1024 && unit < units.Length - 1)
            {
                value /= 1024;
                unit++;
            }

            if (unit == 0)
            {
                return value.ToString("0") + " B";
            }
            return value.ToString("0.0") + " " + units[unit];
        }
    }
}
