using AgileActorsDemo.Spotify;

namespace AgileActorsDemo.Services
{
    public interface ISpotifyApiHttpRepository : IHttpRepository
    {
        Task<SpotifyAccessToken> GetAccessToken();
    }
}

