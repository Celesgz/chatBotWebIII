
using Botify.Data.EF;
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
        Task<Usuario> BuscarUsuario(Usuario usuario);
    }

    public class UsuariosLogica : IUsuariosLogica
    {
        private readonly BotifyContext _context;

        public UsuariosLogica(BotifyContext context)
        {
            _context = context;
        }

        public async Task<Usuario> AgregarUsuario(Usuario nuevo)
        {
            _context.Usuarios.Add(nuevo);
            await _context.SaveChangesAsync();
            return nuevo;
        }

        public async Task<Usuario> BuscarUsuario(Usuario usuario)
        {
            return await _context.Usuarios.FindAsync(usuario.Id);
        }
    }
}
