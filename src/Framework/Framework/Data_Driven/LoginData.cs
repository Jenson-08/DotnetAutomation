using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Data_Driven
{
    public static class LoginData
    {
        public static object[] ValidLogins =
        {
            new object[] { "joe@smith.com", "joepassword", "Welcome" }
        };

        public static object[] InvalidLogins =
        {
            new object[] { "wrong@example.com", "badpass", "Sign in unsuccessful" }
        };
    }
}
