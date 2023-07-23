namespace shareme_backend.Utils;

public static class Env
{
    private static IConfiguration _configuration;

    public static void Initialize(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public static string Secret => _configuration["Application:Secret"];
}