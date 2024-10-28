using System.Collections.Generic;

namespace Botify.Entidades
{
    public class Artista
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Popularity { get; set; }
        public ExternalUrls External_urls { get; set; } 
        public string Href { get; set; }
        public List<Imagen> Images { get; set; } = new List<Imagen>(); 
        public Seguidores Followers { get; set; } = new Seguidores(); 
        public string Uri { get; set; }
        public List<string> Genres { get; set; } = new List<string>(); 
        public string Type { get; set; }
    }

    public class Imagen
    {
        public string Url { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
    }

    public class Seguidores
    {
        public string Href { get; set; }
        public int Total { get; set; }
    }
    public class ExternalUrls
    {
        public string Spotify { get; set; }
    }
}
