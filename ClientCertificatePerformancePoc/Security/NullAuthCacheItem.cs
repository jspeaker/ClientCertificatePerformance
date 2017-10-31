namespace ClientCertificatePerformancePoc.Security
{
    public class NullAuthCacheItem : IAuthCacheItem
    {
        public bool Populated() => false;

        public bool Valid() => false;
    }
}