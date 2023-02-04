using System.Net;

namespace TaskRegister.API.Entities.Response
{
    public class CustomResponse<T>
    {
        public CustomResponse()
        {
            Message = string.Empty;
            IsSuccessful = true;
            StatusCode = (int)HttpStatusCode.OK;
        }

        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public T Content { get; set; }
        public int StatusCode { get; set; }
    }
}
