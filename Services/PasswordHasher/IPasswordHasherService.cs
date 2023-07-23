namespace shareme_backend.Services;

public interface IPasswordHasherService
{
    string Hash(string password);

    bool Verify(string passwordHash, string inputPassword);
}
