/****************************** DbCsvPlusHeaderRule ******************************\
Module Name:  DbCsvPlus
Project:      This module is used in various ETL processes.
Copyright (c) Aaron Ulrich.


DbCsvPlusHeaderRule is used to help process column data based on expected details (DataType & Format)


This source is subject to the Apache License Version 2.0, January 2004
See http://www.apache.org/licenses/.
All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/

using System;

namespace MetaIS.Dbs.Csv
{
    public class DbCsvHeaderRule
    {
        private DbCsvPlusRules oRules = null;
        private DbCsvPlusError oError = null;

        // lookup via fld or iPos --- iPos is probably faster
        private string columnName = "";
        private int columnPosition = 0;
        private Type dataType = Type.GetType("System.String");
        private string dataTypeFormat = "";

        public DbCsvHeaderRule(DbCsvPlusRules oRules, DbCsvPlusError oError, string fld, int iPos, string dataType, string dataTypeFormat)
        {
            this.oRules = oRules;
            this.oError = oError;

            this.columnName = fld;
            this.columnPosition = iPos;
            if (dataType.Trim() != "") {
                this.dataType = Type.GetType(dataType);
            }
            this.dataTypeFormat = dataTypeFormat;
        }

        #region " Properties "

        public String ColumnName {get { return columnName; } set { columnName = value;} }
        public int ColumnPosition {get { return columnPosition; } set { columnPosition = value;} }
        public Type DataType {get { return dataType; } set { dataType = value;} }
        public String DataTypeFormat {get { return dataTypeFormat; } set { dataTypeFormat = value;} }

        #endregion

        public bool ProcessData(string dataValue, ref object dataObject)
        {
            // try cast to data type
            bool isOK = DbHelper.GetValueFromStringTry(dataType.ToString(), dataValue, this.dataTypeFormat, ref dataObject);
            if (!isOK) {
                // parse failed
                if (oError != null) {
                    oError.AddFlag_DataFldBadType();
                    oError.AppendToLog("DataType cannot cast the line value: " +
                        this.columnName + "[" + this.dataType.ToString() + "]" + " failed to cast " + dataValue + " into the format: " + this.dataTypeFormat);
                }
            }

            return isOK;
        }
    }
}
