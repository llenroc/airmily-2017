using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airmily.Services.Models
{
    public class Season
    {
        public List<Episode> Episodes { get; set; }
        public string Poster { get; set; }
        public int SeasonNumber { get; set; }
    }
}
