namespace ClientCertificatePerformancePoc.Security
{
    public interface IAuthCacheItem
    {
        bool Populated();
        bool Valid();
        bool Equals(object obj);
    }
}