using NUnit.Framework;
using System.Reflection;
using System.IO;
using System.Data;
using System.Collections;
using System.Linq;

using csvplus_read_write.Db;

namespace csvplus_read_write_test.UnitTests
{
    [TestFixture]
    public class TestCHQ
    {
        [SetUp] public void Setup() {}
        [TearDown] public void TestTearDown() {}

        public static IEnumerable InputRulesCHQ {
            get {
                DbCsvPlusRules oRulesCHQ = null;
                DbCsvPlusError oError = null;

                // No Rules or Error --- loading data without any rule or error objects (aka QuickLoad)
                yield return new TestCaseData("NULL_RULES_ERROR", null, null, "UnitTests/Files/chq_badcomma.csv", 150, 0, "");    

                // NORMAL: Normal and properly formed CSV file
                oRulesCHQ = new DbCsvPlusRules(true, true, false, "Date,Description,Amount", "Account Number,Currency");                    
                oError = new DbCsvPlusError();
                yield return new TestCaseData("NORMAL", oRulesCHQ, oError, "UnitTests/Files/chq.csv", 150, 0, "");           

                // INCOMPLETE: In this case the last entry is incomplete. This will often occur with a partial/failed extract.
                oRulesCHQ = new DbCsvPlusRules(true, true, false, "Date,Description,Amount", "Account Number,Currency");
                oError = new DbCsvPlusError();
                yield return new TestCaseData("INCOMPLETE", oRulesCHQ, oError, "UnitTests/Files/chq_badRow.csv", 149, 1, "DataFldTooFew,ErrorOnLastRow");

                // COLUMN_RENAME: This will appear as a missing column with a new column added
                oRulesCHQ = new DbCsvPlusRules(true, true, false, "Date,Description,Amount2", "Account Number,Currency");
                oError = new DbCsvPlusError();
                yield return new TestCaseData("COLUMN_RENAME", oRulesCHQ, oError, "UnitTests/Files/chq.csv", 150, 0, "HeaderNewWarning,HeaderRequiredMissing");

                // NON_QUOTED_COMMA: The output of some systems occasionally have commas in a non-quoted text string... this detects and discards those entries.
                oRulesCHQ = new DbCsvPlusRules(true, true, false, "Date,Description,Amount", "Account Number,Currency");
                oError = new DbCsvPlusError();
                yield return new TestCaseData("NON_QUOTED_COMMA", oRulesCHQ, oError, "UnitTests/Files/chq_badcomma.csv", 149, 1, "DataFldTooMany");
                
                // VERIFY_TYPES_OK: Add verification rules via data type and format.
                oRulesCHQ = new DbCsvPlusRules(true, true, false, "Date,Description,Amount", "Account Number,Currency");
                oRulesCHQ.ForceDataTypes = "System.DateTime, System.String, System.Double";
                oRulesCHQ.DataTypeFormats = 
                    "yyyy/MM/dd" + "," +
                    "^[C][H][Q][#](?<P1>[0-9]{5})[-](?<P2>[0-9]{10})$" + "," +    // this will create 3 capture groups (all-index0, P1-index1, P2-index2) it also forces a specific number length
                    "";
                oError = new DbCsvPlusError();
                yield return new TestCaseData("VERIFY_TYPES_OK", oRulesCHQ, oError, "UnitTests/Files/chq.csv", 150, 0, "");
                
                // VERIFY_TYPES_FAIL: Add verification rules via data type and format.
                oRulesCHQ = new DbCsvPlusRules(true, true, false, "Date,Description,Amount", "Account Number,Currency");
                oRulesCHQ.ForceDataTypes = "System.DateTime, System.String, System.Double";
                oRulesCHQ.DataTypeFormats = 
                    "yyyy/MM/dd" + "," +
                    "^[C][H][Q][#](?<P1>[0-9]{5})[-](?<P2>[0-9]{10})$" + "," +    // this will create 3 capture groups (all-index0, P1-index1, P2-index2) it also forces a specific number length
                    "";
                oError = new DbCsvPlusError();
                yield return new TestCaseData("VERIFY_TYPES_FAIL", oRulesCHQ, oError, "UnitTests/Files/chq_badtype.csv", 147, 3, "DataFldBadType");
                
            }
        }

        //[Ignore("")]
        [Test]
        //[ExpectedException(typeof(KeyNotFoundException))]
        [TestCaseSource("InputRulesCHQ")]
        public void CsvTest(string testName, DbCsvPlusRules oRules, DbCsvPlusError oError, string fileSubPath, int rowCount, int discardCount, string flagSet) {
            fileSubPath = fileSubPath.Replace("/", @"\");
            string binFolderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string fullPath = binFolderPath + "\\" + fileSubPath;

            //DbCsvPlusError oError = new DbCsvPlusError(System.IO.Path.GetFileName(fullPath));
            DataTable oDt = DbCsvPlus.LoadDataTable(fullPath, null, true, false, false, ',', oRules, oError);

            if (oDt == null) {
                Assert.Fail("Data file failed to load.");
            } else {
                if(oError == null) {
                    Assert.AreEqual(rowCount, oDt.Rows.Count, "Unexpected row count.");

                } else {
                    // Make sure we match on ALL the flags
                    if(oError.errorFlags.Count == 0 && flagSet=="") {
                        Assert.AreEqual(rowCount, oDt.Rows.Count, "Unexpected row count.");
                        if(oError.oDtErr != null)
                            Assert.AreEqual(discardCount, oError.oDtErr.Rows.Count, "Unexpected discard count.");

                    } else {
                        bool isOK = true;
                        string[] aFlagSet = flagSet.Split(',');
                        foreach(string key in oError.errorFlags.Keys) {
                            if(!aFlagSet.Contains(key)) {
                                isOK = false;
                                break;
                            }
                        }
                        if(isOK) {
                            foreach(string key in aFlagSet) {
                                if(!oError.errorFlags.ContainsKey(key)) {
                                    isOK = false;
                                    break;
                                }
                            }
                        }

                        string errorsActual = string.Join(",", oError.errorFlags.Keys);
                        Assert.IsTrue(isOK, "Unexpected error flag output. Expected:" + flagSet + "   Actual:" + errorsActual);
                        Assert.AreEqual(rowCount, oDt.Rows.Count, "Unexpected row count.");
                        if(oError.oDtErr != null)
                            Assert.AreEqual(discardCount, oError.oDtErr.Rows.Count, "Unexpected discard count.");
                    }
                }
            }
        }
    }
}
  