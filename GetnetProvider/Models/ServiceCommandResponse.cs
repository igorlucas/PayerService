namespace GetnetProvider.Models
{
    public class ServiceCommandResponse<R> where R : class
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public R? Result { get; set; }
        public ServiceCommandResponseError? Error { get; set; }

        public ServiceCommandResponse(R result, int statusCode, string message)
        {
            Result = result;
            StatusCode = statusCode;
            Message = message;
        }

        public ServiceCommandResponse(int statusCode, string message, IEnumerable<string> items)
        {
            StatusCode = statusCode;
            Message = message;
            Error = new ServiceCommandResponseError(items);
        }
    }

    public class ServiceCommandResponseError
    {
        public IEnumerable<string> Items { get; set; } = Array.Empty<string>();

        public ServiceCommandResponseError(IEnumerable<string> items)
        {
            Items = items;
        }
    }

}