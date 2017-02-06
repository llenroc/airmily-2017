using System;
using System.Net.Http;
using System.Threading.Tasks;
using airmily.Services.Exceptions;

namespace airmily.Services.TrackSeries
{
    public class TrackSeriesBase
    {
        protected string BaseUrl;

        protected HttpClient GetClient()
        {
            return GetClient(BaseUrl);
        }

        protected virtual HttpClient GetClient(string baseUrl)
        {
            var client = new HttpClient {BaseAddress = new Uri(baseUrl)};

            return client;
        }

        protected async Task Get(string url)
        {
            using (var client = GetClient())
            {
                try
                {
                    var response = await client.GetAsync(url);
                    if (!response.IsSuccessStatusCode)
                    {
                        var error = await response.Content.ReadAsAsync<TrackSeriesError>();
                        throw new TrackSeriesException(error.Message, response.StatusCode);
                    }
                }
                catch (HttpRequestException ex)
                {
                    throw new TrackSeriesException("", false, ex);
                }
            }
        }

        protected async Task<T> Get<T>(string url)
        {
            using (var client = GetClient())
            {
                try
                {
                    var response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                        return await response.Content.ReadAsAsync<T>();

                    var error = await response.Content.ReadAsAsync<TrackSeriesError>();
                    var message = error != null ? error.Message : "";
                    throw new TrackSeriesException(message, response.StatusCode);
                }
                catch (HttpRequestException ex)
                {
                    throw new TrackSeriesException("", false, ex);
                }
                catch (UnsupportedMediaTypeException ex)
                {
                    throw new TrackSeriesException("", false, ex);
                }
            }
        }
    }
}
