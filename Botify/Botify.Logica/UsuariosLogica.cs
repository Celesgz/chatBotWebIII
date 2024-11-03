
using Botify.Data.EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Botify.Logica
{
    public interface IUsuariosLogica
    {
        Task<Usuario> AgregarUsuario(Usuario nuevo);
        Task<Usuario> BuscarUsuarioPorEmail(string email);
    }

    public class UsuariosLogica : IUsuariosLogica
    {
        private readonly BotifyContext _context;
        private readonly PasswordHasher<Usuario> _passwordHasher = new PasswordHasher<Usuario>();

        public UsuariosLogica(BotifyContext context)
        {
            _context = context;
        }

        public async Task<Usuario> BuscarUsuarioPorEmail(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Usuario> AgregarUsuario(Usuario nuevo)
        {
            // Hashea la contraseña antes de guardar
            nuevo.Password = _passwordHasher.HashPassword(nuevo, nuevo.Password);
            _context.Usuarios.Add(nuevo);
            await _context.SaveChangesAsync();
            return nuevo;
        }

    }
}
