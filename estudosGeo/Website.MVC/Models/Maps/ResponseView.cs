namespace Website.MVC.Models.Maps
{
    public class ResponseView
    {
        public Status Status { get; set; }
        public object Result { get; set; }
    }

    public enum Status
    {
        NotFound = 0,
        Found = 1,
        OK = 2,
        NOK = 3
    }
}