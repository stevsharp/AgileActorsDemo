namespace AgileActorsDemo.Services
{
    public interface IHttpRepository
    {
        Task<T?> GetAsync<T>(string endpoint, CancellationToken cancellationToken = default) where T : class;
        Task<T?> GetAsync<T>(Uri endpoint, string token, CancellationToken cancellationToken = default) where T : class;
        Task<T?> GetAsync<T>(string endpoint, string token, CancellationToken cancellationToken = default) where T : class;
        Task<T?> GetAsyncWithJsonConvert<T>(Uri endpoint, string token, CancellationToken cancellationToken = default) where T : class;
        Task<T?> PostAsync<T>(object data, Uri endpoint, CancellationToken cancellationToken = default) where T : class;
    }
}

