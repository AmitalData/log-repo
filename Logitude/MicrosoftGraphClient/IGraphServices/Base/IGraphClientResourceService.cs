namespace MicrosoftGraphClient.IGraphServices.Base
{
    public interface IGraphClientResourceService<T>
    {
        T SetToken(string token);
    }
}