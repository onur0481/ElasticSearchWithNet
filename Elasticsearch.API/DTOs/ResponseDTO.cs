using System.Net;

namespace Elasticsearch.API.DTOs
{
    public record ResponseDTO<T>
    {
        public T? Data { get; set; }

        public List<string>? Errors { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public static ResponseDTO<T> Success(T data, HttpStatusCode statusCode)
        {
            return new ResponseDTO<T> { Data = data, StatusCode = statusCode };
        }

        public static ResponseDTO<T> Fail(List<string> errors, HttpStatusCode statusCode)
        {
            return new ResponseDTO<T> { StatusCode = statusCode, Errors = errors };
        }

        public static ResponseDTO<T> Fail(string error, HttpStatusCode statusCode)
        {
            return new ResponseDTO<T> { Errors = new List<string> { error }, StatusCode = statusCode };
        }
    }
}
