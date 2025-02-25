using Planify_BackEnd.Models;

public interface IUserRepository
{
    User GetUserByEmail(string email);
    User GetUserById(Guid id);
}