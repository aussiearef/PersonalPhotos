using Core.Interfaces;

namespace Core.Services
{
    public class DefaultKeyGenerator : IKeyGenerator
    {
        public string GetKey(string emailAddress)
        {
            return emailAddress.Replace("@", string.Empty).Replace(".", string.Empty);
        }
    }
}