namespace API.Models
{
    public abstract class GenericApiResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        protected GenericApiResponse(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }

    public class GenericApiResponseEntity<T> : GenericApiResponse where T : class
    {
        public T? Resource { get; set; }

        public GenericApiResponseEntity(int statusCode, string message, T? resource) : base(statusCode, message)
        {
            Resource = resource;
        }
    }

    public class GenericApiResponseEntityList<T> : GenericApiResponse where T : class
    {
        public IEnumerable<T> Resources { get; set; }

        public GenericApiResponseEntityList(int statusCode, string message, IEnumerable<T> resources) : base(statusCode, message)
        {
            Resources = resources;
        }
    }
}