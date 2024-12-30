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
        STAFF,
        UPD_STAFF,
        ALL_STAFF,
        CH_ROLE,
        REG_PERSON,
        ADD_EVENT,
        EDT_PERSON,
        CH_PASSWORD,
        CH_PASSWORD_SUBM
    }

    public static class HeaderFormatizer
    {
        public static string Format(this RequestHeader header)
        {
            switch (header)
            {
                case RequestHeader.PASSWORD:
                    return "pwd";
                case RequestHeader.STAFF:
                    return "stf";
                case RequestHeader.UPD_STAFF:
                    return "uds";
                case RequestHeader.ALL_STAFF:
                    return "atf";
                case RequestHeader.CH_ROLE:
                    return "chr";
                case RequestHeader.REG_PERSON:
                    return "rgp";
                case RequestHeader.ADD_EVENT:
                    return "ade";
                case RequestHeader.EDT_PERSON:
                    return "edp";
                case RequestHeader.CH_PASSWORD:
                    return "chp";
                case RequestHeader.CH_PASSWORD_SUBM:
                    return "cps";
            }
            return "null";
        }
    }
}
