namespace AgileActorsDemo.Spotify
{
    public record SpotifyAccessToken(string access_token, string token_type, long expires_in);
}
