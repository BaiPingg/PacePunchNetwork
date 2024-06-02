using System.Collections.Generic;

public interface IService
{
}

public class SL
{
    private static Dictionary<string, IService> _services = new();

    public static void Register<T>(IService service) where T : IService
    {
        if (!_services.ContainsKey(typeof(T).Name))
        {
            _services[typeof(T).Name] = service;
        }
    }

    public static T Get<T>() where T : class, IService
    {
        if (_services.ContainsKey(typeof(T).Name))
        {
            return _services[typeof(T).Name] as T;
        }

        return null;
    }
}