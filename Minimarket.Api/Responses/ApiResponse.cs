using Minimarket.Core.CustomEntities;
using System.Net;

namespace Minimarket.Api.Responses
{
    public class ApiResponse<T>
    {
        public Message[] Messages { get; set; }
        public T Data { get; set; }
        public Pagination Pagination { get; set; }
        //public HttpStatusCode statusCode { get; set; }
        public ApiResponse(T data)
        {
            Data = data;
        }
    }
}
