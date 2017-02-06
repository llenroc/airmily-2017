using System.Collections.Generic;
using System.Threading.Tasks;
using airmily.Services.Models;

namespace airmily.Services.TrackSeries
{
    public class TrackSeries : TrackSeriesBase, ITrackSeries
    {
        public TrackSeries()
        {
            BaseUrl = "https://api.trackseries.tv/v1/";
        }

        public async Task<Serie> GetSerieByIdAll(int id)
        {
            return await Get<Serie>($"Series/{id}/All");
        }

        public async Task<SerieInfo> GetSerieById(int id)
        {
            return await Get<SerieInfo>($"Series/{id}");
        }

        public async Task<List<SerieFollowers>> GetStatsTopSeries()
        {
            return await Get<List<SerieFollowers>>("Stats/TopSeries");
        }

        public async Task<List<SerieSearch>> GetSeriesSearch(string name)
        {
            return await Get<List<SerieSearch>>($"Series/Search?query={name}");
        }

        public async Task<SerieFollowers> GetStatsSerieHighlighted()
        {
            return await Get<SerieFollowers>("Stats/SerieHighlighted");
        }
    }
}
