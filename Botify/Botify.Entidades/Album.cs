using System.Collections.Generic;

namespace Botify.Entidades
{
  
public class Album
{
    public string Name { get; set; }
    public List<Artist> Artists { get; set; }
    public string ReleaseDate { get; set; }
    public List<Image> Images { get; set; }
}
}
