using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Botify.Entidades
{
   public class Track
   {
      public Album Album { get; set; }
      public List<Artist> Artists { get; set; }
      public string Name { get; set; }
      public string Id { get; set; }
      public string Uri { get; set; }
      public int DurationMs { get; set; }
      public bool Explicit { get; set; }
      public int Popularity { get; set; }
      public string Type { get; set; }
      
      [JsonPropertyName("external_urls")]
      public ExternalUrls ExternalUrls { get; set; }
   }
}
