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
    public class MyClass
    {
        //DbCsvPlusRules oRulesCHQ = null;

        public static IEnumerable InputRulesCHQ {
            get {
                DbCsvPlusRules oRulesCHQ = null;

                oRulesCHQ = new DbCsvPlusRules(true, true, false, "Date,Description,Amount", "Account Number,Currency");                    
                yield return new TestCaseData(oRulesCHQ, "UnitTests/Files/chq.csv", 150);           // Normal properly formed CSV file

                oRulesCHQ = new DbCsvPlusRules(true, true, false, "Date,Description,Amount", "Account Number,Currency");
                yield return new TestCaseData(oRulesCHQ, "UnitTests/Files/chq_badRow.csv", 149);    // In this case the last entry is incomplete (a partial extract)

                DbCsvPlusRules oRulesCHQ2 = new DbCsvPlusRules(true, true, false, "Date,Description", "Account Number,Currency");
                yield return new TestCaseData(oRulesCHQ2, "UnitTests/Files/chq.csv", 150);           // Missing header
            }
        }


        [SetUp]
        public void Setup() {}

        [TearDown]
        public void TestTearDown() {}

        [Ignore("")]
        [Test]
        [TestCaseSource("InputRulesCHQ")]
        public void CsvTest(DbCsvPlusRules oRules, string fileSubPath, int rowCount) {
            fileSubPath = fileSubPath.Replace("/", @"\");
            string binFolderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string fullPath = binFolderPath + "\\" + fileSubPath;

            DbCsvPlusError oError = new DbCsvPlusError(System.IO.Path.GetFileName(fullPath));
            DataTable oDt = DbCsvPlus.LoadDataTable(fullPath, null, true, false, false, ',', oRules, oError);

            if (oDt == null) {
                Assert.Fail("Data file failed to load.");
            } else {
                Assert.AreEqual(rowCount, oDt.Rows.Count);
            }
            // Assert.IsTrue(true);
        }


        [TestCase("UnitTests/Files/chq.csv",       150, "Date,Description,Amount", "Account Number,Currency", "")]                                       // Normal properly formed CSV file
        [TestCase("UnitTests/Files/chq_badRow.csv",149, "Date,Description,Amount", "Account Number,Currency", "DataFldTooFew,ErrorOnLastRow")]           // In this case the last entry is incomplete (a partial extract)
        [TestCase("UnitTests/Files/chq.csv",       150, "Date,Description,Amount2", "Account Number,Currency", "HeaderNewWarning,HeaderRequiredMissing")]// Missing required header, new header found
        [TestCase("UnitTests/Files/chq_badcomma.csv",149, "Date,Description,Amount", "Account Number,Currency", "DataFldTooMany")]      // In this case an extra comma has been added to an attribute without quotes
        public void CsvTest2(string fileSubPath, int rowCount, string rulesRequiredHeaders, string rulesAdditionalHeaders, string flagSet)
        {
            fileSubPath = fileSubPath.Replace("/", @"\");
            string binFolderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string fullPath = binFolderPath + "\\" + fileSubPath;

            DbCsvPlusError oError = new DbCsvPlusError(System.IO.Path.GetFileName(fullPath));
            DbCsvPlusRules oRules = new DbCsvPlusRules(true, true, false, rulesRequiredHeaders, rulesAdditionalHeaders);
            DataTable oDt = DbCsvPlus.LoadDataTable(fullPath, null, true, false, false, ',', oRules, oError);


            // Make sure we match on ALL the flags
            if(oError.errorFlags == null && flagSet=="") {
                Assert.AreEqual(rowCount, oDt.Rows.Count);

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
            }

            
        }

    }
}
  