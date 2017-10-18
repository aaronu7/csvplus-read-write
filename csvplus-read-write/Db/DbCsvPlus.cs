/****************************** DbCsv ******************************\
Module Name:  DbCsv
Project:      This module is used in various ETL processes.
Copyright (c) Aaron Ulrich.


DbCSV offers the option of following enhancements to regular CSV file reading/writing:
- DataType retention
- Error details during csv data reads.
- Rules to handle validation of data contents (ex. expected headers)


This source is subject to the Apache License Version 2.0, January 2004
See http://www.apache.org/licenses/.
All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/

using System;
using System.Linq;
using System.Collections;
using System.Text;
using System.Data;
using System.IO;

namespace csvplus_read_write.Db
{
    /// <summary>
    /// DbCSV offers the option of following enhancements to regular CSV file reading/writing:
    /// - DataType retention
    /// - Error details during csv data reads.
    /// - Rules to handle validation of data contents (ex.expected headers)
    /// </summary>
    public static class DbCsvPlus
    {
        #region " SaveDataTable "

        #endregion


        #region " LoadDataTable "

        #region " LoadDataTableFromCsvString - From a data string "

        /// <summary>
        /// Use this function call to load the data from an in memory csv formatted string.
        /// </summary>
        /// <param name="tblName">The tablename to set in the loaded DataTable.</param>
        /// <param name="data">The CSV formatted string.</param>
        /// <param name="HasHeader">Is there a header line in the csv.</param>
        /// <param name="HasDataType">Is there a datatype line in the csv.</param>
        /// <param name="RetainNewLine">Would you like to retain new lines found in the csv data.</param>
        /// <param name="delim">A comma delimiter is the default, but any delimiter can be specified.</param>
        /// <param name="oRules">Pass null to Ignore. If used this object contains rules to verify the integrity of the csv.</param>
        /// <param name="oError">Pass null to Ignore. If used this object will be populated with details about the errors encountered while processing.</param>
        /// <returns></returns>
        static public DataTable LoadDataTableFromCsvString(string tblName, string data,
            bool HasHeader, bool HasDataType, bool RetainNewLine, char delim,
            DbCsvPlusRules oRules, DbCsvPlusError oError)
        {

            // Build our base datatable
            DataTable oDt = new DataTable(tblName);

            //string dataLast = "";
            int iLine = 1;
            int iColCount = 0;
            bool hasColumn1 = false;
            int firstDataLine = 1;
            bool isRowError = false;

            // Loop the lines of data
            string[] lines = data.Split('\n');

            foreach (string line in lines)
            {
                // prepare the dataline
                string dataLine = "";
                if (!RetainNewLine)
                    dataLine = line.Replace("(\\n)", "\n");
                else
                    dataLine = line;

                dataLine = dataLine.Trim();

                // parse out the flds
                ArrayList result = new ArrayList();
                ParseCSVFields(result, dataLine, delim);
                string[] flds = (string[])result.ToArray(typeof(string));

                // Process the line
                isRowError = false;    // This is used to detect a lastRow error

                if ((HasHeader == true) && (iLine == 1)) {
                    // Header Row
                    ProccessLineHeader(oDt, oError, oRules, flds, data, ref iColCount, ref hasColumn1);
                    firstDataLine++;

                } else if (((HasHeader == false) && (HasDataType == true) && (iLine == 1)) ||
                    ((HasHeader == true) && (HasDataType == true) && (iLine == 2))) {

                    // DataType Row
                    ProccessLineDataType(oDt, oError, flds);
                    firstDataLine++;

                } else {

                    // Data Row
                    bool IsDiscard = ProccessLine_DiscardCheck(oError, oRules, iColCount, flds);

                    if (!IsDiscard) {
                        // Process this line normally
                        isRowError = ProccessLine(oDt, oError, oRules, flds, iLine, firstDataLine);

                    } else {
                        // Discard the line IFF a discardable error condition was met
                        //      this will add the errorous row to oDtErr (instead of oDt)
                        isRowError = true;
                        if (oError != null) {
                            oError.DiscardLine(oDt, flds);
                        }
                    }
                }

                iLine++;
            }

            // if the last row threw a discard, then this will occur
            if(isRowError && oError != null) {
                oError.AddFlag_ErrorOnLastRow();
            }

            return oDt;
        }

        #endregion

        #region " LoadDataTable - Using FileReader "

        /// <summary>
        /// Use this function call to load a csv file.
        /// </summary>
        /// <param name="filePath">The path to the desired file.</param>
        /// <param name="fileEnc">The encoding to load the file with.</param>
        /// <param name="HasHeader">Is there a header line in the csv.</param>
        /// <param name="HasDataType">Is there a datatype line in the csv.</param>
        /// <param name="RetainNewLine">Would you like to retain new lines found in the csv data.</param>
        /// <param name="delim">A comma delimiter is the default, but any delimiter can be specified.</param>
        /// <param name="oRules">Pass null to Ignore. If used this object contains rules to verify the integrity of the csv.</param>
        /// <param name="oError">Pass null to Ignore. If used this object will be populated with details about the errors encountered while processing.</param>
        /// <returns></returns>
        static public DataTable LoadDataTable(string filePath, Encoding fileEnc,
            bool HasHeader, bool HasDataType, bool RetainNewLine, char delim,
            DbCsvPlusRules oRules, DbCsvPlusError oError) {

            DataTable oDt = null;
            StreamReader reader = null;
            Stream stream = null;

            //======================================================
            // Open our file with a stream reader
            if (!System.IO.File.Exists(filePath)) {
                if (oError != null) {
                    oError.AppendToLog("File not found: " + filePath);
                } else {
                    System.Console.WriteLine("File not found: " + filePath);
                }
            } else {
                try {
                    stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    if (stream == null || !stream.CanRead) {
                        if (oError != null) {
                            oError.AppendToLog("Could not read CSV: " + filePath);
                        } else {
                            System.Console.WriteLine("Could not read CSV: " + filePath);
                        }
                    }

                    if (fileEnc == null) {
                        reader = new StreamReader(stream);
                    } else {
                        reader = new StreamReader(stream, fileEnc);
                    }

                    // Build our base datatable
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
                    oDt = new DataTable(fileName);

                    // Table and FileStream are ready... lets process
                    _GetCvsDataTableBase(reader, oDt, HasHeader, HasDataType, RetainNewLine, delim, fileEnc, oRules, oError);

                } catch {
                    System.Console.WriteLine("Could not open CSV: " + filePath);
                }
            }

            // Dispose
            if (reader != null) {
                reader.Close();
                reader.Dispose();
                reader = null;
            }
            if (stream != null) {
                stream.Close();
                stream.Dispose();
                stream = null;
            }

            return oDt;
        }

        static private void _GetCvsDataTableBase(StreamReader reader, DataTable oDt,
            bool HasHeader, bool HasDataType, bool RetainNewLine, char delim, Encoding enc,
            DbCsvPlusRules oRules, DbCsvPlusError oError)
        {

            //string dataLast = "";
            int iLine = 1;
            int iColCount = 0;
            bool hasColumn1 = false;
            int firstDataLine = 1;
            bool isRowError = false;

            // Get the first line of data (and split flds)
            string data = "";
            string[] flds = GetCSVLine(reader, RetainNewLine, delim, ref data);

            if (data != null) {
                //dataLast = (string)data.Clone();

                // Loop while the flds are not NULL
                while (flds != null) {
                    isRowError = false;    // This is used to detect a lastRow error

                    if ((HasHeader == true) && (iLine == 1)) {
                        // Header Row
                        ProccessLineHeader(oDt, oError, oRules, flds, data, ref iColCount, ref hasColumn1);
                        firstDataLine++;

                    } else if (((HasHeader == false) && (HasDataType == true) && (iLine == 1)) ||
                        ((HasHeader == true) && (HasDataType == true) && (iLine == 2))) {
                        // DataType Row
                        ProccessLineDataType(oDt, oError, flds);
                        firstDataLine++;

                    } else {
                        // Data Row
                        bool IsDiscard = ProccessLine_DiscardCheck(oError, oRules, iColCount, flds);

                        if (!IsDiscard) {
                            // Process this line normally
                            bool isOk = ProccessLine(oDt, oError, oRules, flds, iLine, firstDataLine);
                            if (!isOk) {
                                isRowError = true;
                                if (oError != null) {
                                    oError.DiscardLine(oDt, flds);
                                }
                            }

                        } else {
                            // Discard the line IFF a discardable error condition was met
                            //      this will add the errorous row to oDtErr (instead of oDt)
                            isRowError = true;
                            if (oError != null) {
                                oError.DiscardLine(oDt, flds);
                            }
                        }
                    }
                    //dataLast = (string)data.Clone();

                    // Get the next set of flds to continue (terminates if NULL)
                    flds = GetCSVLine(reader, RetainNewLine, delim, ref data);
                    iLine++;
                }
            }

            // if the last row threw a discard, then this will occur
            if(isRowError && oError != null) {
                oError.AddFlag_ErrorOnLastRow();
            }
        }

        #endregion

        #region " ProccessLine_DiscardCheck "

        static private bool ProccessLine_DiscardCheck(DbCsvPlusError oError, DbCsvPlusRules oRules, Int32 iColCount, string[] flds)
        {
            bool IsDiscard = false;

            // ***** this condition is probably redundant ******
            //if(iColCount!=0 && hasColumn1 && iColCount > flds.Length) {
            // HR has a Column1 header which disappears on the data entries
            //      There-fore the header has MORE fields and one of them is Column1
            //    IsDiscard = false;

            if (iColCount != 0 && iColCount > flds.Length)
            {
                // We have fewer fields in this row then we have columns ...
                //      this occurs in some older BCEsis entries ...
                if (oRules != null)
                    IsDiscard = oRules.discardOnToFewDataFlds;
                else
                    IsDiscard = false;

                if (oError != null) {
                    oError.AddFlag_DataFldTooFew();
                    oError.AppendToLog("Fewer data fields then column headers: " + String.Join(",", flds));
                }

            } else if (iColCount != 0 && iColCount < flds.Length) {
                // We have more fields then we have columns ... no choice but to discard this entry
                //      this entry probably has a bad comma causing an extra field
                if (oRules != null)
                    IsDiscard = oRules.discardOnToManyDataFlds;
                else
                    IsDiscard = false;

                if (oError != null) {
                    oError.AddFlag_DataFldTooMany();
                    oError.AppendToLog("Too many fields, Discard Entry: " + String.Join(",", flds));
                }
            }

            return IsDiscard;
        }

        #endregion

        #region " ProccessLineHeader "

        static private void ProccessLineHeader_PrepareRulesSetColumn(DataTable oDt, DbCsvPlusRules oRules, DbCsvPlusError oError, string fld, int fldPos, bool isRequired)
        {
            // Proccess any rule logic to be applied to the data based on the headers ordering
            //   fldPos
            //      -> DataType
            //      -> Parse Rules

            // Add the data column
            oDt.Columns.Add(new DataColumn(fld));

            if (oRules != null)
            {
                // Get data type and build our rule
                string sType = "System.String";
                string sFormat = "";

                if (isRequired && oRules.aRequiredHeaders != null)
                {
                    for (int ix = 0; ix < oRules.aRequiredHeaders.Length; ix++)
                    {
                        if (oRules.aRequiredHeaders[ix].ToUpper() == fld.ToUpper())
                        {
                            if (oRules.aForceDataTypes != null)
                                sType = oRules.aForceDataTypes[ix];

                            if (oRules.aDataTypeFormats != null)
                                sFormat = oRules.aDataTypeFormats[ix];
                            break;
                        }
                    }
                }

                CoreDbCsvHeaderRule oHeaderRule = new CoreDbCsvHeaderRule(oRules, oError, fld, fldPos, sType, sFormat);
                // force a datatype on the column
                if (sType.Trim() != "") {
                    oDt.Columns[fld].DataType = Type.GetType(sType);
                }
                oRules.AddHeaderRule(fldPos, oHeaderRule);
            }
        }

        static private void ProccessLineHeader(DataTable oDt, DbCsvPlusError oError, DbCsvPlusRules oRules, string[] flds, string data, ref Int32 iColCount, ref bool hasColumn1)
        {
            // Find any missing OR additional headers
            Int32 reqFound = 0;
            Int32 optFound = 0;
            Int32 newFound = 0;

            // Process the header row
            int iFldPos = 0;
            foreach (string fld in flds)
            {

                bool keepGoing = true;
                if (oDt.Columns.Contains(fld))
                {
                    // Duplicate header column
                    if (oError != null) {
                        oError.AddFlag_HeaderDuplicate();
                        oError.AppendToLog("Duplicate header name: " + fld);
                    }
                    if (oRules != null) {
                        keepGoing = oRules.keepHeaderDuplicates;
                    }
                }

                if (ProccessLineHeader_IsBadName(fld)) {
                    // Header has unsupported symbols
                    if (oError != null) {
                        oError.AddFlag_HeaderBadName();
                        oError.AppendToLog("Header has a Bad Name and uses unsupported symbols: " + fld);
                    }
                    if (oRules != null) {
                        if (keepGoing)
                            keepGoing = oRules.keepHeaderBadnames;
                    }
                }

                if (keepGoing) {
                    if (!ProccessLineHeader_IsIdealName(fld)) {
                        if (oError != null) {
                            oError.AddFlag_HeaderPoorNameWarning();
                            oError.AppendToLog("Warning header uses non-ideal symbols: " + fld);
                        }
                    }

                    // Maintain counts - we've already filter out duplication, so this should work
                    bool isNew = true;
                    bool isRequired = false;
                    if (oRules != null && oRules.aRequiredHeaders != null) {
                        if (oRules.aRequiredHeaders.Contains(fld)) {
                            reqFound++;
                            isNew = false;
                            isRequired = true;
                        }
                    }

                    if (oRules != null && oRules.aOptionalHeaders != null) {
                        if (oRules.aOptionalHeaders.Contains(fld)) {
                            optFound++;
                            isNew = false;
                        }
                    }
                    if (isNew) {
                        newFound++;
                    }

                    // Prepare our rules for this header
                    ProccessLineHeader_PrepareRulesSetColumn(oDt, oRules, oError, fld, iFldPos, isRequired);

                    iColCount++;
                    if (fld.ToLower() == "column1" || fld.ToLower() == "") {
                        hasColumn1 = true;
                    }

                    // Add simple error coloumn (string) to catch what-ever error value may trigger an entry
                    if (oError != null && oError.oDtErr != null) {
                        oError.oDtErr.Columns.Add(new DataColumn(fld, Type.GetType("System.String")));
                    }
                }
                iFldPos++;
            }


            if (oRules != null) {
                // Check and log new headers
                if (newFound > 0) {
                    if (oError != null) {
                        oError.AddFlag_HeaderNewWarning();
                        oError.AppendToLog("New header(s) found: " + data + "   (expected: " + oRules._requiredHeaders + "," + oRules._optionalHeaders + ")");
                    }
                }

                // Check and log required header issues
                if (oRules.aRequiredHeaders != null) {
                    if (reqFound != oRules.aRequiredHeaders.Length) {
                        if (oError != null) {
                            oError.AddFlag_HeaderRequiredMissing();
                            oError.AppendToLog("Missing required headers: " + data + "   (required: " + oRules._requiredHeaders + ")");
                        }
                    }
                }

                // Check and log additonal header issues
                if (oRules.aOptionalHeaders != null) {
                    if (optFound != oRules.aOptionalHeaders.Length) {
                        if (oError != null) {
                            oError.AddFlag_HeaderOptionalMissing();
                            oError.AppendToLog("Warning missing optional headers: " + data + "   (optionals: " + oRules._optionalHeaders + ")");
                        }
                    }
                }
            }
        }

        static private bool ProccessLineHeader_IsIdealName(string fld)
        {
            bool isOK = true;

            foreach (char ch in fld)
            {
                int cVal = (int)ch;

                if ((cVal >= 48 && cVal <= 57) || (cVal >= 65 && cVal <= 90) || (cVal >= 97 && cVal <= 122))
                {
                    // OK - Numbers and letters
                }
                else if (cVal == ' ' || cVal == '_' || cVal == '-')
                {
                    // OK - Approved symbols
                }
                else
                {
                    isOK = false;
                }
            }

            return isOK;
        }

        static private bool ProccessLineHeader_IsBadName(string fld)
        {
            bool isBad = false;

            foreach (char ch in fld)
            {
                int cVal = (int)ch;

                //if(cVal == '\'' || cVal == '"' || cVal == '`' || cVal == ',' || cVal == ';') {
                //    isBad = true;
                //}
            }

            return isBad;
        }

        #endregion

        #region " ProccessLineDataType "

        static private void ProccessLineDataType(DataTable oDt, DbCsvPlusError oError, string[] flds)
        {
            int ix = 0;
            foreach (string fld in flds)
            {

                // check for any conflicts with the types set via Rules

                oDt.Columns[ix].DataType = Type.GetType(fld.Trim());

                // leave the error table with a generic string field type (created during header creation)
                //if(oError!=null && oError.oDtErr!=null) {
                //oError.oDtErr.Columns[ix].DataType = Type.GetType(fld.Trim());
                //}

                ix++;
            }
        }

        #endregion

        #region " ProcessLine "

        static private bool ProccessLine(DataTable oDt, DbCsvPlusError oError, DbCsvPlusRules oRules, string[] data, int iLine, int firstDataLine)
        {
            // Good Entry - get data row

            DataRow oRow = oDt.NewRow();
            bool isOK = true;
            int iCol = 0;
            foreach (string datum in data)
            {
                if (iCol > oDt.Columns.Count - 1) {
                    //  Column count is too high
                    //      simply "crop these" data fields.... error object has the details

                } else {
                    object dataObject = null;

                    if (oRules == null) {
                        // No rule engine, assume proper format and get value fast
                        oRow[iCol] = DbHelper.GetValueFromString(oDt.Columns[iCol], datum);

                    } else {
                        // OK, lets get a rule
                        bool setWithRule = false;
                        if (iLine == firstDataLine || !oRules.ruleCheckOnlyFirstDataLine) {

                            CoreDbCsvHeaderRule oHeaderRule = oRules.GetHeaderRule(iCol);
                            if (oHeaderRule != null) {
                                setWithRule = true;
                                isOK = oHeaderRule.ProcessData(datum, ref dataObject);
                                if (isOK) {
                                    oRow[iCol] = dataObject;
                                }
                            }
                        }

                        // No rule matched this one
                        if (!setWithRule) {
                            isOK = DbHelper.GetValueFromStringTry(oDt.Columns[iCol], datum, "", ref dataObject);
                            oRow[iCol] = dataObject;
                        }

                        if (!isOK) {
                            break;
                        }
                    }
                }
                iCol++;
            }

            if (isOK) {
                oDt.Rows.Add(oRow);
            }

            return isOK;
        }


        #endregion

        #region " GetCSVLine / ParseCSVFields "

        static private string[] GetCSVLine(StreamReader reader, bool RetainNewLine, char delim, ref string dataLine) {
            dataLine = reader.ReadLine();

            if (dataLine == null)
                return null;

            if (dataLine.Length == 0)
                return new string[0];

            // retain NewLine
            if (RetainNewLine)
                dataLine = dataLine.Replace("(\\n)", "\n");

            ArrayList result = new ArrayList();
            ParseCSVFields(result, dataLine, delim);
            return (string[])result.ToArray(typeof(string));
        }

        static private void ParseCSVFields(ArrayList result, string data, char delim) {
            // Parses the CSV fields and pushes the fields into the result arraylist
            int pos = -1;

            while (pos < data.Length)
                result.Add(ParseCSVField(data, delim, ref pos));
        }

        static private string ParseCSVField(string data, char delim, ref int startSeparatorPosition) {
            // Parses the field at the given position of the data, modified pos to match
            // the first unparsed position and returns the parsed field

            if (startSeparatorPosition == data.Length - 1) {
                startSeparatorPosition++;
                // The last field is empty
                return "";
            }

            int fromPos = startSeparatorPosition + 1;

            // Determine if this is a quoted field
            if (data[fromPos] == '"') {

                // If we're at the end of the string, let's consider this a field that only contains the quote
                if (fromPos == data.Length - 1) {
                    fromPos++;

                    // FIX: Feb 2016 -- Odd case from MyEd .... lets officially set the variable to terminate at a higher level as well
                    //   I have only noticed ONE case from MyEd requiring this .... so it is a very odd one
                    //   "\"2727000\",\"2038532\",\"101364305\",\"Oviatt\",\"Jason\",\""
                    startSeparatorPosition = fromPos;

                    return "\"";
                }

                // Otherwise, return a string of appropriate length with double quotes collapsed
                // Note that FSQ returns data.Length if no single quote was found
                int nextSingleQuote = FindSingleQuote(data, fromPos + 1);
                startSeparatorPosition = nextSingleQuote + 1;
                return data.Substring(fromPos + 1, nextSingleQuote - fromPos - 1).Replace("\"\"", "\"");
            }

            // The field ends in the next comma or EOL
            int nextComma = data.IndexOf(delim, fromPos);
            if (nextComma == -1) {
                startSeparatorPosition = data.Length;
                return data.Substring(fromPos);
            } else {
                startSeparatorPosition = nextComma;
                return data.Substring(fromPos, nextComma - fromPos);
            }
        }


        static private int FindSingleQuote(string data, int startFrom)
        {
            // Returns the index of the next single quote mark in the string 
            // (starting from startFrom)

            int i = startFrom - 1;
            while (++i < data.Length)
                if (data[i] == '"') {
                    // If this is a double quote, bypass the chars
                    if (i < data.Length - 1 && data[i + 1] == '"') {
                        i++;
                        continue;
                    } else
                        return i;
                }
            // If no quote found, return the end value of i (data.Length)
            return i;
        }


        #endregion

        #endregion

    }
}