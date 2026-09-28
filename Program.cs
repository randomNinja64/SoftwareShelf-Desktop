using System;
using System.Net;
using System.Windows.Forms;

namespace SoftwareShelf_Desktop
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            EnableTlsOnPatchedRuntime();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        // .NET 2.0 names only Ssl3 and Tls (TLS 1.0). 768 and 3072 are TLS 1.1
        // and 1.2. Patched System.dll on Windows 7 and newer honors them.
        // The original runtime throws, and the SSL 3.0 / TLS 1.0 default stays.
        static void EnableTlsOnPatchedRuntime()
        {
            try
            {
                ServicePointManager.SecurityProtocol =
                    SecurityProtocolType.Tls
                    | (SecurityProtocolType)768
                    | (SecurityProtocolType)3072;
            }
            catch (NotSupportedException)
            {
            }
        }
    }
}
