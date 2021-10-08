using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagement
{
    public class Account
    {
        private string username;
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
            }
        }

        private string password;
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
            }
        }

        public Account()
        {
            username = password = "";
        }
        public Account(string _id, string _pw)
        {
            this.username = _id;
            this.password = _pw;
        }
    }
}
