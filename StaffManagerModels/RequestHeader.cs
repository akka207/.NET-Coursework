using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaffManagerModels
{
    public enum RequestHeader
    {
        PASSWORD,
        ALL_STAFF,
        STAFF
    }

    public static class HeaderFormatizer
    {
        public static string Format(this RequestHeader header)
        {
            switch(header)
            {
                case RequestHeader.PASSWORD:
                    return "pwd";
                case RequestHeader.ALL_STAFF:
                    return "asf";
                case RequestHeader.STAFF:
                    return "stf";
            }
            return "null";
        }
    }
}
