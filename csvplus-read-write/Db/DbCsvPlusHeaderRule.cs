using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csvplus_read_write.Db
{
    public class CoreDbCsvHeaderRule
    {
        DbCsvPlusRules oRules = null;
        DbCsvPlusError oError = null;

        // lookup via fld or iPos --- iPos is probably faster
        public string columnName = "";
        public int columnPosition = 0;
        public Type dataType = Type.GetType("System.String");
        public string dataTypeFormat = "";

        public CoreDbCsvHeaderRule(DbCsvPlusRules oRules, DbCsvPlusError oError, string fld, int iPos, string dataType, string dataTypeFormat)
        {
            this.oRules = oRules;
            this.oError = oError;

            this.columnName = fld;
            this.columnPosition = iPos;
            if (dataType.Trim() != "")
            {
                this.dataType = Type.GetType(dataType);
            }
            this.dataTypeFormat = dataTypeFormat;
        }

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
