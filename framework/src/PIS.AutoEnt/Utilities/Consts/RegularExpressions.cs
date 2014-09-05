using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt
{
    public static class RegularExprs
    {
        #region Consts

        // Data Types
        public const string Integer = @"^-?\d+$";
        public const string PositiveInteger = @"^[0-9]*[1-9][0-9]*$";
        public const string NonPositiveInteger = @"((-\d+)|(0+))$";
        public const string NegativeInteger = @"^\d+$";
        public const string NonNegativeInteger = @"^-[0-9]*[1-9][0-9]*$";

        public const string Float = @"^(-?\d+)(\.\d+)?$";
        public const string PositiveFloat = @"^(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*))$";
        public const string NonPositiveFloat = @"^((-\d+(\.\d+)?)|(0+(\.0+)?))$";
        public const string NegativeFloat = @"^(-(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*)))$";
        public const string NonNegativeFloat = @"^\d+(\.\d+)?$";

        // Characters
        public const string Characters = "[A-Za-z]+$";
        public const string Uppercase = "[A-Z]+$";
        public const string Lowercase = "[a-z]+$";
        public const string Word = @"^\w+$";    // 数字，英文字母，下划线

        // Nomal Patterns
        public const string EMail = @"^[a-zA-Z0-9_-]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$";
        public const string Url = @"^[a-zA-Z]+://(\w+(-\w+)*)(\.w+(-\w+)*))*(\?\S*)?$";
        public const string IPAddress = @"^([01]?\d\d?|2[0-4]\d|25[0-5])\.([01]?\d\d?|2[0-4]\d|25[0-5])\.([01]?\d\d?|2[0-4]\d|25[0-5])\.([01]?\d\d?|2[0-4]\d|25[0-5])$";
        public const string ChineseMobileNumber = @"^13\d{9}$/gi";
        public const string ChineseIdCard = @"\d{15}|\d{18}";


        #endregion
    }
}
