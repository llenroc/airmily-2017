using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace airmily.Services.Exceptions
{
    public class TrackSeriesException : Exception
    {
        public bool IsConnected { get; set; }

        public HttpStatusCode StatusCode { get; set; }


        public TrackSeriesException(string message, HttpStatusCode statusCode)
            : base(message)
        {
            IsConnected = true;
            StatusCode = statusCode;
        }

        public TrackSeriesException(string message, bool isConnected, Exception inner)
            : base(message, inner)
        {
            IsConnected = isConnected;
            StatusCode = HttpStatusCode.ServiceUnavailable;
        }
    }
}
