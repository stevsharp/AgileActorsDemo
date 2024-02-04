
namespace AgileActorsDemo.Services
{
    public interface IUserService
    {
        Task<bool> ValidateUser(string username, string password, CancellationToken cancellationToken);

        string GenerateToken(string username, CancellationToken cancellationToken);
    }
}


