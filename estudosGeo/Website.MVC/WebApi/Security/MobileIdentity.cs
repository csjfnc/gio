using System.Collections.Generic;
using System.Security.Principal;

namespace WebApi.Security
{
    public class MobileIdentity : GenericIdentity
    {
        public MobileIdentity(string login, string password, string imei) : base(login, "MobileBasic")
        {
            Password = password;
            Login = login;
            Imei = imei;
        }

        public string IdUsuario { get; set; }
        public string Password { get; set; }
        public string Login { get; set; }
        public string Imei { get; set; }
        public string Version { get; set; }

        public ICollection<string> Modules { get; set; }
    }
}