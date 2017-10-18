/****************************** DbCsvPlusError ******************************\
Module Name:  DbCsvPlus
Project:      This module is used in various ETL processes.
Copyright (c) Aaron Ulrich.


DbCsvPlusError is used to record and store error information when reading a CSV file.
- Contains an Error datatable to store exact record state of problem entries.
- Contains a lookup of error flags (and counts) to give an overview of the problems.


This source is subject to the Apache License Version 2.0, January 2004
See http://www.apache.org/licenses/.
All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/

using System;
using System.Data;
using System.Collections.Generic;

namespace csvplus_read_write.Db
{
    public class DbCsvPlusError
    {
        // This object tracks issues in the CSV file
        public DataTable oDtErr = null;                     // oDtErr stores the rows that have issues
        public string logMsg = "";                          // msg log to append issues messages to
        public Dictionary<String, Int32> errorFlags = null; // flag keys and occurance count

        public DbCsvPlusError() {
            errorFlags = new Dictionary<string, int>();
        }

        #region " Add flag " 

        //public bool error_DataFldBadRule = false;
        //public bool error_DataType_Unknown = false;


        protected void AddFlag(string flagName) {
            if(errorFlags==null)
                errorFlags = new Dictionary<string, int>();

            if(errorFlags.ContainsKey(flagName)) {
                errorFlags[flagName] += 1;
            } else {
                errorFlags.Add(flagName, 1);
            }
        }

        public void AddFlag_HeaderRequiredMissing() {AddFlag("HeaderRequiredMissing");}

        public void AddFlag_HeaderOptionalMissing() {AddFlag("HeaderOptionalMissing");}

        public void AddFlag_HeaderNewWarning() {AddFlag("HeaderNewWarning");}

        public void AddFlag_HeaderBadName() {AddFlag("HeaderBadName");}

        public void AddFlag_HeaderPoorNameWarning() {AddFlag("HeaderPoorNameWarning");}

        public void AddFlag_HeaderDuplicate() {AddFlag("HeaderDuplicate");}

        public void AddFlag_DataFldTooFew() {AddFlag("DataFldTooFew");}

        public void AddFlag_DataFldTooMany() {AddFlag("DataFldTooMany");}

        public void AddFlag_ErrorOnLastRow() {AddFlag("ErrorOnLastRow");}

        public void AddFlag_DataFldBadType() {AddFlag("DataFldBadType");}

        #endregion

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

        /// <summary>
        /// Discard this data row into the error table. This will update the error table with any required data columns.
        /// </summary>
        /// <param name="oDt">The target datatable which is being populated from the CSV file.</param>
        /// <param name="flds">The array of field values which are causing this discard to be triggered.</param>
        public void DiscardLine(DataTable oDt, string[] flds)
        {
            // Create the Error table
            if(this.oDtErr==null) {
                this.oDtErr = new DataTable(oDt.TableName + "_ERROR");
                foreach(DataColumn oCol in oDt.Columns) {
                    this.oDtErr.Columns.Add(oCol.ColumnName, Type.GetType("System.String"));   // always force error columns into a string type
                }
            }

            DataRow oRow = this.oDtErr.NewRow();
            int iCol = 0;
            foreach (string fld in flds) {

                if (iCol > oDt.Columns.Count - 1) {
                    // Column count too high ... lets just add a generic new one
                    if (!this.oDtErr.Columns.Contains("Column" + iCol.ToString())) {
                        this.oDtErr.Columns.Add(new DataColumn("Column" + iCol.ToString()));
                    }

                    oRow[iCol] = GetValueFromString(this.oDtErr.Columns[iCol], fld);
                    iCol++;

                } else {
                    oRow[iCol] = GetValueFromString(this.oDtErr.Columns[iCol], fld);
                    iCol++;
                }
            }

            this.oDtErr.Rows.Add(oRow);
        }
    }

}
