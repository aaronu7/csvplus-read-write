/****************************** DbCsvPlusRules ******************************\
Module Name:  DbCsvPlus
Project:      This module is used in various ETL processes.
Copyright (c) Aaron Ulrich.


DbCsvPlusRules is used to encapsulate the rules surrounding how the incoming CSV file will be parsed and errors observed.


This source is subject to the Apache License Version 2.0, January 2004
See http://www.apache.org/licenses/.
All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/

using System.Collections.Generic;

namespace MetaIS.Dbs.Csv
{
    public class DbCsvPlusRules
    {
        // Scrub Rules
        private bool discardOnToFewDataFlds = false;   // disard the datarow if it has fewer data flds then expected by the header  -- append empty data if false
        private bool discardOnToManyDataFlds = true;   // disard the datarow if it has to many data flds                            -- discard data overflow if false
        //public string purgeHeaders = "";              // ex. Legacy HR had "Column1" with no data under it
        //public bool   purgeHeaderBlanks = true;       // remove any headers with blank values

        private bool keepHeaderDuplicates = true;
        private bool keepHeaderBadnames = true;

        // Verify Rules
        private string requiredHeaders = "";           // these are the required headers... error out if any are missing
        private string optionalHeaders = "";         // these headers are optionals.... throw a warning if any new headers are found
        private string[] aRequiredHeaders = null;
        private string[] aOptionalHeaders = null;

        //public string forceStringQuotes = "";         // force these data-types on the requiredHeaders        // ****** could be used when saving ... undecided if it belongs here

        private bool ruleCheckOnlyFirstDataLine = false;  // a performance consideration to rule check ONLY the first data line (rather then ALL of them)
        private string forceDataTypes = "";              // force these data-types on the requiredHeaders
        private string[] aForceDataTypes = null;
        private string dataTypeFormats = "";           // format rules .... DateTime, RegEx on strings, etc
        private string[] aDataTypeFormats = null;

        private Dictionary<int, DbCsvHeaderRule> headerRules = null;

        #region " Properties "

        public bool CheckOnlyFirstDataLine {get { return ruleCheckOnlyFirstDataLine; } set { ruleCheckOnlyFirstDataLine = value;} }

        public bool DiscardOnToFewDataFlds {get { return discardOnToFewDataFlds; } set { discardOnToFewDataFlds = value;} }
        public bool DiscardOnToManyDataFlds {get { return discardOnToManyDataFlds; } set { discardOnToManyDataFlds = value;} }

        public bool KeepHeaderDuplicates {get { return keepHeaderDuplicates; } set { keepHeaderDuplicates = value;} }
        public bool KeepHeaderBadnames {get { return keepHeaderBadnames; } set { keepHeaderBadnames = value;} }

        public string RequiredHeaders {get { return requiredHeaders; } set { requiredHeaders = value;} }
        public string[] RequiredHeadersSet {get { return aRequiredHeaders; } set { aRequiredHeaders = value;} }

        public string OptionalHeaders {get { return optionalHeaders; } set { optionalHeaders = value;} }
        public string[] OptionalHeadersSet {get { return aOptionalHeaders; } set { aOptionalHeaders = value;} }

        public string ForceDataTypes {
            get { return this.forceDataTypes; }
            set {
                this.forceDataTypes = value;
                if (this.forceDataTypes != "") {
                    splitIntoList(this.forceDataTypes, ref this.aForceDataTypes);
                }
            }
        }
        public string[] ForceDataTypesSet {get { return aForceDataTypes; } set { aForceDataTypes = value;} }

        public string DataTypeFormats {
            get { return this.dataTypeFormats; }
            set {
                this.dataTypeFormats = value;
                if (this.dataTypeFormats != "") {
                    splitIntoList(this.dataTypeFormats, ref this.aDataTypeFormats);
                }
            }
        }
        public string[] DataTypeFormatsSet {get { return aDataTypeFormats; } set { aDataTypeFormats = value;} }

        #endregion

        public DbCsvPlusRules(bool discardOnToFewDataFlds, bool discardOnToManyDataFlds, bool ruleCheckOnlyFirstDataLine, string requiredHeaders, string additionalHeaders)
        {
            this.discardOnToFewDataFlds = discardOnToFewDataFlds;
            this.discardOnToManyDataFlds = discardOnToManyDataFlds;
            this.ruleCheckOnlyFirstDataLine = ruleCheckOnlyFirstDataLine;
            this.requiredHeaders = requiredHeaders;
            this.optionalHeaders = additionalHeaders;

            if (this.requiredHeaders != "") {
                splitIntoList(this.requiredHeaders, ref this.aRequiredHeaders);
            }
            if (this.optionalHeaders != "") {
                splitIntoList(this.optionalHeaders, ref this.aOptionalHeaders);
            }
        }




        public void AddHeaderRule(int iColPos, DbCsvHeaderRule oHeaderRule) {
            if (headerRules == null)
                headerRules = new Dictionary<int, DbCsvHeaderRule>();

            this.headerRules.Add(iColPos, oHeaderRule);
        }
        public DbCsvHeaderRule GetHeaderRule(int iColPos) {
            DbCsvHeaderRule oHeaderRule = null;
            if (this.headerRules != null && this.headerRules.ContainsKey(iColPos)) {
                oHeaderRule = this.headerRules[iColPos];
            }
            return oHeaderRule;
        }

        private void splitIntoList(string strList, ref string[] lst) {
            lst = strList.Split(',');
            if (lst.Length == 1) {
                lst = strList.Split(';');
            }
            for (int ix = 0; ix < lst.Length; ix++) {
                lst[ix] = lst[ix].Trim();
            }
        }
    }
}
