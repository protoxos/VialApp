namespace VialApp.Models
{
    public class ApiResponse<T, S>
    {
        public S? Status { get; set; }
        public string Message { get; set; } = "";
        public T? Data { get; set; }

        public ApiResponse()
        {
            Status = default(S?);
        }
    }
}
