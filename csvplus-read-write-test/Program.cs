using System;
using System.Reflection;
using System.IO;
using System.Data;

using csvplus_read_write.Db;

namespace csvplus_read_write_test
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {            
            /*
            // VERIFY_TYPES_FAIL: Add verification rules via data type and format.
            DbCsvPlusRules oRules = new DbCsvPlusRules(true, true, false, "Date,Description,Amount", "Account Number,Currency");
            oRules.ForceDataTypes = "System.DateTime, System.String, System.Double";
            oRules.DataTypeFormats = 
                "yyyy/MM/dd" + "," +
                "^[C][H][Q][#](?<P1>[0-9]{5})[-](?<P2>[0-9]{10})$" + "," +    // this will create 3 capture groups (all-index0, P1-index1, P2-index2) it also forces a specific number length
                "";

            string fileSubPath = "UnitTests/Files/chq_badtype.csv";
            fileSubPath = fileSubPath.Replace("/", @"\");
            string binFolderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string fullPath = binFolderPath + "\\" + fileSubPath;

            DbCsvPlusError oError = new DbCsvPlusError();
            DataTable oDt = DbCsvPlus.LoadDataTable(fullPath, null, true, false, false, ',', oRules, oError);
            */
        }
    }
}
