using System;
using System.Data;

namespace csvplus_read_write.Db
{
    public class DbCsvPlusError
    {
        // This object tracks issues in the CSV file
        public DataTable oDtErr = null;                     // oDtErr stores the rows that have issues
        public string logMsg = "";                          // msg log to append issues messages to

        public bool error_HeaderRequiredMissing = false;
        public bool error_HeaderOptionalMissing = false;   // optional header removed
        public bool error_HeaderNewWarning = false;        // new header found
        public bool error_HeaderBadName = false;
        public bool error_HeaderPoorNameWarning = false;
        public bool error_HeaderDuplicate = false;

        public bool error_DataFldTooFew = false;
        public bool error_DataFldTooMany = false;
        public bool error_DataFldOnLastRow = false; // did this error occur on the last row of the dataset
        public bool error_DataFldBadType = false;
        //public bool error_DataFldBadRule = false;

        //public bool error_DataType_Unknown = false;

        public DbCsvPlusError(string csvTableName)
        {
            oDtErr = new DataTable(csvTableName + "_ERROR");
        }

        #region " Helpers "

        public void AppendToLog(string msg)
        {
            if (this.logMsg == "")
            {
                this.logMsg = msg;
            }
            else
            {
                this.logMsg = this.logMsg + Environment.NewLine + msg;
            }
        }

        #region " GetValueFromString "

        static public object GetValueFromString(DataColumn oCol, string sValue)
        {
            object ret = DBNull.Value;
            string sType = oCol.DataType.ToString();
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

                        try
                        {
                            DateTime oDt = DateTime.MinValue;
                            if (!DateTime.TryParse(sValue, out oDt))
                            {
                                oDt = DateTime.MinValue;
                            }
                            //ret = DateTime.Parse(sValue);
                            ret = oDt;

                        }
                        catch
                        {
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

        #endregion

        #endregion

        public void DiscardLine(DataTable oDt, string[] flds)
        {
            // Discard the line, if an discardable error condition was met

            if (this.oDtErr != null)
            {
                DataRow oRow = this.oDtErr.NewRow();
                int iCol = 0;
                foreach (string fld in flds)
                {
                    if (iCol > oDt.Columns.Count - 1)
                    {

                        // Column count too high ... lets just add a generic new one
                        if (!this.oDtErr.Columns.Contains("Column" + iCol.ToString()))
                        {
                            this.oDtErr.Columns.Add(new DataColumn("Column" + iCol.ToString()));
                        }

                        oRow[iCol] = GetValueFromString(this.oDtErr.Columns[iCol], fld);
                        iCol++;

                    }
                    else
                    {
                        oRow[iCol] = GetValueFromString(this.oDtErr.Columns[iCol], fld);
                        iCol++;
                    }
                }

                this.oDtErr.Rows.Add(oRow);
            }
        }
    }

}
