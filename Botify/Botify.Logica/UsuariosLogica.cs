﻿
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

    }

    public class UsuariosLogica : IUsuariosLogica
    {
        private readonly BotifyContext _context;

        public UsuariosLogica(BotifyContext context)
        {
            _context = context;
        }
    }
}