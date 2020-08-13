using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoManagement
{
    public static class Global
    {
        private static int _globalUserID = 0 ;
        private static string _globalUserName;

        public static int GlobalUserID
        {
            get { return _globalUserID; }
            set { _globalUserID = value; }
        }
        public static string GlobalUserName
        {
            get { return _globalUserName; }
            set { _globalUserName = value; }
        }
    }
}
