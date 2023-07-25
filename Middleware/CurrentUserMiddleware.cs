namespace shareme_backend.Middleware;

using Microsoft.AspNetCore.Identity;
using shareme_backend.DTOs.Auth;

public class CurrentUserMiddleware
{
    private readonly UserManager<CurrentUser> _userManager;
    private readonly RequestDelegate _next;

    public CurrentUserMiddleware(RequestDelegate next, UserManager<CurrentUser> userManager)
    {
        this._userManager = userManager;
        this._next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var user = await this._userManager.GetUserAsync(context.User);

        // Criar uma instância de UserRequest com as informações do usuário
        var userRequest = new CurrentUser
        {
            UserId = user.UserId,
            Email = user.Email,
            Username = user.Username,
        };

        // Armazenar o objeto UserRequest no contexto da requisição
        context.Items["CurrentUser"] = userRequest;

        // Continuar o pipeline da requisição
        await this._next(context);
    }
}
