using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airmily.Services.Models
{
    public class TmdbRequest
    {
        public int Id { get; set; }
        public List<TmdbVideo> Results { get; set; }
    }
}
