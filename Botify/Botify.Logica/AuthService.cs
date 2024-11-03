using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using Botify.Data.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

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
            .SingleOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            Console.WriteLine("Error: Usuario no encontrado.");
            return null;
        }

        // Verificar la contraseña hasheada
        var passwordHasher = new PasswordHasher<Usuario>();
        var resultado = passwordHasher.VerifyHashedPassword(user, user.Password, password);

        if (resultado != PasswordVerificationResult.Success)
        {
            Console.WriteLine("Error: La contraseña es incorrecta.");
            return null;
        }

        // Generar el token JWT
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

        Console.WriteLine("Token generado exitosamente.");
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}
