using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using API.Spotify.EF.Data.EF;
using Microsoft.EntityFrameworkCore;

public class AuthService
{
    private readonly IConfiguration _configuration;
    private readonly BotifyContext _context; // Reemplaza YourDbContext con el nombre de tu contexto de datos

    public AuthService(IConfiguration configuration, BotifyContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    public async Task<string> AuthenticateAsync(string email, string password)
    {
        // Verificar si el usuario existe en la base de datos
        var user = await _context.Usuarios
            .SingleOrDefaultAsync(u => u.Email == email && u.Password == password);

        if (user == null)
        {
            // Retornar null si el usuario no existe o las credenciales son incorrectas
            return null;
        }

        // Si el usuario existe, generar el token JWT
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(double.Parse(jwtSettings["ExpiresInMinutes"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
