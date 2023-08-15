using System.Net;
using System.Text.Json.Serialization;

namespace Elasticsearch.API.DTOs
{
    public record ResponseDTO<T>
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Data { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
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
