using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csvplus_read_write.Db
{
    public class DbCsvPlusRules
    {
        // Scrub Rules
        public bool discardOnToFewDataFlds = false;   // disard the datarow if it has fewer data flds then expected by the header  -- append empty data if false
        public bool discardOnToManyDataFlds = true;   // disard the datarow if it has to many data flds                            -- discard data overflow if false
                                                      //public string purgeHeaders = "";              // ex. Legacy HR had "Column1" with no data under it
                                                      //public bool   purgeHeaderBlanks = true;       // remove any headers with blank values
        public bool keepHeaderDuplicates = true;
        public bool keepHeaderBadnames = true;

        // Verify Rules
        public string _requiredHeaders = "";           // these are the required headers... error out if any are missing
        public string _optionalHeaders = "";         // these headers are optionals.... throw a warning if any new headers are found
        public string[] aRequiredHeaders = null;
        public string[] aOptionalHeaders = null;

        //public string forceStringQuotes = "";         // force these data-types on the requiredHeaders        // ****** Used when saving

        public bool ruleCheckOnlyFirstDataLine = false;  // a performance consideration to rule check ONLY the first data line (rather then ALL of them)
        private string _forceDataTypes = "";              // force these data-types on the requiredHeaders
        public string[] aForceDataTypes = null;
        private string _dataTypeFormats = "";           // format rules .... DateTime, RegEx on strings, etc
        public string[] aDataTypeFormats = null;

        private Dictionary<int, DbCsvHeaderRule> headerRules = null;


        public DbCsvPlusRules(bool discardOnToFewDataFlds, bool discardOnToManyDataFlds, bool ruleCheckOnlyFirstDataLine, string requiredHeaders, string additionalHeaders)
        {
            this.discardOnToFewDataFlds = discardOnToFewDataFlds;
            this.discardOnToManyDataFlds = discardOnToManyDataFlds;
            this.ruleCheckOnlyFirstDataLine = ruleCheckOnlyFirstDataLine;
            this._requiredHeaders = requiredHeaders;
            this._optionalHeaders = additionalHeaders;

            if (this._requiredHeaders != "")
            {
                splitIntoList(this._requiredHeaders, ref this.aRequiredHeaders);
            }

            if (this._optionalHeaders != "")
            {
                splitIntoList(this._optionalHeaders, ref this.aOptionalHeaders);
            }
        }

        public string ForceDataTypes
        {
            get { return this._forceDataTypes; }
            set
            {
                this._forceDataTypes = value;
                if (this._forceDataTypes != "")
                {
                    splitIntoList(this._forceDataTypes, ref this.aForceDataTypes);
                }
            }
        }
        public string DataTypeFormats
        {
            get { return this._dataTypeFormats; }
            set
            {
                this._dataTypeFormats = value;
                if (this._dataTypeFormats != "")
                {
                    splitIntoList(this._dataTypeFormats, ref this.aDataTypeFormats);
                }
            }
        }

        public void AddHeaderRule(int iColPos, DbCsvHeaderRule oHeaderRule)
        {
            if (headerRules == null)
                headerRules = new Dictionary<int, DbCsvHeaderRule>();

            this.headerRules.Add(iColPos, oHeaderRule);
        }
        public DbCsvHeaderRule GetHeaderRule(int iColPos)
        {
            DbCsvHeaderRule oHeaderRule = null;
            if (this.headerRules != null && this.headerRules.ContainsKey(iColPos))
            {
                oHeaderRule = this.headerRules[iColPos];
            }
            return oHeaderRule;
        }

        private void splitIntoList(string strList, ref string[] lst)
        {
            lst = strList.Split(',');
            if (lst.Length == 1)
            {
                lst = strList.Split(';');
            }
            for (int ix = 0; ix < lst.Length; ix++)
            {
                lst[ix] = lst[ix].Trim();
            }
        }
    }
}
