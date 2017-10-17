using System;
using System.Data;

namespace csvplus_read_write.Db
{
    static public class DbHelper
    {
        #region " GetValueFromString "

        // This is the slower, but safer version
        static public bool GetValueFromStringTry(DataColumn oCol, string sValue, string format, ref object outValue)
        {
            string sType = oCol.DataType.ToString();
            return GetValueFromStringTry(sType, sValue, format, ref outValue);
        }
        static public bool GetValueFromStringTry(string sType, string sValue, string format, ref object outValue)
        {
            bool isOK = false;
            outValue = DBNull.Value;

            //if(sType == "System.String")
            //    ret = "";

            if (sValue != "")
            {
                switch (sType)
                {
                    case (""):
                    case ("System.String"):
                        if (format.Trim() == "")
                        {
                            outValue = sValue;
                            isOK = true;
                        }
                        else
                        {
                            // RegEx
                            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(format);
                            System.Text.RegularExpressions.Match match = regex.Match(sValue);
                            if (match.Success)
                            {
                                outValue = sValue;
                                isOK = true;
                            }

                        }
                        break;

                    case ("System.Int32"):
                        Int32 val32 = 0;
                        isOK = Int32.TryParse(sValue, out val32);
                        outValue = val32;
                        break;

                    case ("System.Int64"):
                        Int64 val64 = 0;
                        isOK = Int64.TryParse(sValue, out val64);
                        outValue = val64;
                        break;

                    case ("System.DateTime"):
                        DateTime oDt = DateTime.MinValue;
                        if (format.Trim() == "")
                        {
                            isOK = DateTime.TryParse(sValue, out oDt);
                        }
                        else
                        {
                            // dateString = "2011-29-01 12:00 am";    
                            // format     = "yyyy-dd-MM hh:mm tt", 
                            isOK = DateTime.TryParseExact(sValue, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out oDt);
                            //isOK = DateTime.TryParse(sValue, out oDt);
                        }
                        outValue = oDt;
                        break;

                    case ("System.Double"):
                        Double valDbl = 0;
                        isOK = Double.TryParse(sValue, out valDbl);
                        outValue = valDbl;
                        break;

                    case ("System.Boolean"):
                        string val = sValue.ToLower();
                        if (val == "0" || val == "false" || val == "f")
                        {
                            outValue = false;
                            isOK = true;
                        }
                        else if (val == "1" || val == "true" || val == "t")
                        {
                            outValue = true;
                            isOK = true;
                        }
                        break;

                    case ("System.Byte"):
                        Byte valByte = 0;
                        isOK = Byte.TryParse(sValue, out valByte);
                        outValue = valByte;
                        break;

                }
            }

            return isOK;
        }

        static public object GetValueFromString(DataColumn oCol, string sValue)
        {
            string sType = oCol.DataType.ToString();
            return GetValueFromString(sType, sValue);
        }

        static public object GetValueFromString(string sType, string sValue)
        {
            object ret = DBNull.Value;

            if (sType == "System.String")
                ret = "";

            if (sValue != "")
            {
                switch (sType)
                {
                    case ("System.String"):
                        ret = sValue;
                        break;

                    case ("System.Int32"):
                        ret = Int32.Parse(sValue);
                        break;

                    case ("System.Int64"):
                        ret = Int64.Parse(sValue);
                        break;

                    case ("System.DateTime"):

                        try {
                            DateTime oDt = DateTime.MinValue;
                            if (!DateTime.TryParse(sValue, out oDt))
                            {
                                oDt = DateTime.MinValue;
                            }
                            //ret = DateTime.Parse(sValue);
                            ret = oDt;

                        } catch {
                            ret = DateTime.MinValue;
                        }
                        break;

                    case ("System.Double"):
                        ret = Double.Parse(sValue);
                        break;

                    case ("System.Boolean"):
                        string val = sValue.ToLower();
                        if (val == "0" || val == "false" || val == "f")
                            ret = false;
                        else if (val == "1" || val == "true" || val == "t")
                            ret = true;
                        break;

                    case ("System.Byte"):
                        ret = Byte.Parse(sValue);
                        break;

                }
            }

            return ret;
        }

        # endregion
    }
}
