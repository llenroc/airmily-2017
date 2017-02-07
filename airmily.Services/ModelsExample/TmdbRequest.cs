using System.Collections.Generic;
using airmily.Services.Models;

namespace airmily.Services.ModelsExample
{
    public class TmdbRequest
    {
        public int Id { get; set; }
        public List<TmdbVideo> Results { get; set; }
    }
}
