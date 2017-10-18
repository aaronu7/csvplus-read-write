/****************************** DbHelper ******************************\
Module Name:  DbCsvPlus
Project:      This module is used in various ETL processes.
Copyright (c) Aaron Ulrich.


DbHelper contains various core functions to assist this process (ex. GetValueFromString)


This source is subject to the Apache License Version 2.0, January 2004
See http://www.apache.org/licenses/.
All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/

using System;
using System.Data;

namespace csvplus_read_write.Db
{
    static public class DbHelper
    {
        # region " GetValueAsString "

        static public string GetValueAsString(DataRow oRow, string sCol) {
            string sVal = "";
            if(oRow != null) {
                if(oRow.Table.Columns.Contains(sCol)) {
                    sVal = GetValueAsString(oRow[sCol]);
                }
            }
            return sVal;
        }

        static public string GetValueAsString(object value) {
            return GetValueAsString(value, "");
        }

        static public string GetValueAsString(object value, string SurroundStringDelim) {
            string val = "";

            
            if(value!=null && value != DBNull.Value) {
                string sType = value.GetType().ToString();

                if(sType == "System.String") {
                    if((string)value != "") {
                        val = ((string)value);
                        val = SurroundStringDelim + val + SurroundStringDelim;
                    }

                } else if(value.GetType().ToString() == "System.Int32") {
                    val = ((Int32)value).ToString();

                } else if(value.GetType().ToString() == "System.UInt32") {
                    val = ((UInt32)value).ToString();

                } else if(value.GetType().ToString() == "System.Int64") {
                    val = ((Int64)value).ToString();

                } else if(value.GetType().ToString() == "System.UInt64") {
                    val = ((UInt64)value).ToString();

                } else if(value.GetType().ToString() == "System.DateTime") {
                    val = ((DateTime)value).ToString();

                } else if(value.GetType().ToString() == "System.Double") {
                    Double dblVal = (Double)value;
                    if(dblVal == Double.NaN || dblVal == Double.MaxValue || dblVal == Double.MinValue)
                        val = "";
                    else
                        val = dblVal.ToString();

                } else if(value.GetType().ToString() == "System.Boolean") {
                    val = ((Boolean)value).ToString();

                } else if(value.GetType().ToString() == "System.Byte") {
                    val = ((Byte)value).ToString();
                }
            }

            return val;
        }

        # endregion

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
