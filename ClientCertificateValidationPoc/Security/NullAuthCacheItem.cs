namespace ClientCertificateValidationPoc.Security
{
    public class NullAuthCacheItem : IAuthCacheItem
    {
        public string PublicKey()
        {
            return string.Empty;
        }

        public bool Valid()
        {
            return false;
        }
    }
}