namespace Core.Interfaces
{
    public interface IKeyGenerator
    {
        string GetKey(string emailAddress);
    }
}