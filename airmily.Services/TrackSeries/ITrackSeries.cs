using System.Collections.Generic;
using System.Threading.Tasks;
using airmily.Services.Models;
using airmily.Services.ModelsExample;

namespace airmily.Services.TrackSeries
{
    public interface ITrackSeries
    {
        Task<Serie> GetSerieByIdAll(int id);
        Task<SerieInfo> GetSerieById(int id);
        Task<List<SerieFollowers>> GetStatsTopSeries();
        Task<List<SerieSearch>> GetSeriesSearch(string name);
        Task<SerieFollowers> GetStatsSerieHighlighted();
    }
}
