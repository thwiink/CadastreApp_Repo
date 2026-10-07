using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre_App
{
    public static class SessionManager
    {
        public static int CurrentUserID { get; set; }
        public static string CurrentUserRole { get; set; }
        public static bool IsAuthenticated { get; set; }

        public static void Login(int id, string role)
        {
            CurrentUserID = id;
            CurrentUserRole = role;
            IsAuthenticated = true;
        }

        public static void Logout()
        {
            CurrentUserID = 0;
            CurrentUserRole = null;
            IsAuthenticated = false;
        }
    }
}
