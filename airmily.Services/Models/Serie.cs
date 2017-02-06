using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airmily.Services.Models
{
    public class Serie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset? FirstAired { get; set; }
        public string Country { get; set; }
        public string Overview { get; set; }
        public int Runtime { get; set; }
        public string Status { get; set; }
        public string Network { get; set; }
        public DayOfWeek? AirDay { get; set; }
        public string AirTime { get; set; }
        public string ContentRating { get; set; }
        public string ImdbId { get; set; }
        public int TvdbId { get; set; }
        public int TmdbId { get; set; }
        public string Language { get; set; }
        public string Year { get; set; }
        public ImagesSerie Images { get; set; }
        public virtual ICollection<Genre> Genres { get; set; }
        public virtual ICollection<Actor> Actors { get; set; }
        public DateTime Added { get; set; }
        public DateTime LastUpdated { get; set; }
        public string SlugName { get; set; }
        public List<Season> Seasons { get; set; }
        public List<TmdbVideo> Videos { get; set; }

        public Serie()
        {
            Images = new ImagesSerie();
            Genres = new List<Genre>();
            Actors = new List<Actor>();
        }
    }
}
