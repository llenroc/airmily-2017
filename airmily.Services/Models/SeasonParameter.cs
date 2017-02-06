using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airmily.Services.Models
{
    public class SeasonParameter
    {
        public int SeriesId { get; set; }

        public Season SelectedSeason { get; set; }

        public SeasonParameter(int id, Season season)
        {
            SeriesId = id;
            SelectedSeason = season;
        }
    }
}
