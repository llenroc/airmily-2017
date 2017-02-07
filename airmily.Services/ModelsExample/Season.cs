using System.Collections.Generic;

namespace airmily.Services.ModelsExample
{
    public class Season
    {
        public List<Episode> Episodes { get; set; }
        public string Poster { get; set; }
        public int SeasonNumber { get; set; }
    }
}
