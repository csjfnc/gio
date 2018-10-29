namespace Website.MVC.WebApi.Models
{
    public class ResponseApi
    {
        public Status Status { get; set; }
        public string Message { get; set; }
        public object Results { get; set; }
    }

    public enum Status
    {
        NotFound = 0,
        Found = 1,
        OK = 2,
        NOK = 3
    }
}