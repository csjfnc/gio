using System;

namespace Website.BLL.Entities
{
    public class Mobile
    {
        public long Id { get; set; }
        public string Imei { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsBlock { get; set; }
        public string AuthToken { get; set; }
        public DateTime IssuedOn { get; set; }
        public DateTime ExpiresOn { get; set; }
        public string Version { get; set; }
    }
}