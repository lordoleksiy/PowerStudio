using System.Security.Cryptography.X509Certificates;

namespace PowerStudio.Extensions
{
    public static class AuthExtensions
    {
        public static X509Certificate2 LoadCertificate(string thumbprint)
        {

            var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            var certificateCollection = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
            store.Close();

            return certificateCollection[0];
        }
    }
}
