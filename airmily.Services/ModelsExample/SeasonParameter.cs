namespace airmily.Services.ModelsExample
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
