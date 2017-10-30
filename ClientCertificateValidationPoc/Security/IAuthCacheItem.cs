namespace ClientCertificateValidationPoc.Security
{
    public interface IAuthCacheItem
    {
        string PublicKey();
        bool Valid();
    }
}