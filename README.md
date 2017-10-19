# csvplus-read-write

This module was designed as a component for a larger ETL system in which control over various extract sources is limited and changes/errors are constant; it is currently being used in a production ETL system to help verify the integrity of input data through a web-service API. 

Where this fits in the ETL (EXTRACT / scrub / clean&reduce / TRANSFORM / LOAD):
	
	This is a pre-processing algorithm which runs when loading the "raw unverified" csv extract data into the clean&reduce algorithm. The purpose of this is to detect any unexpected or problematic data early on in the process. Any problematic data is removed and isolated in a seperate error table which can be analysed at a later point in the ETL.

Key Features:
- DataType retention: an optional DataType line under the column header is available for Load/Save operations.
- Issue detection: too many fields, too few fields, bad datatypes and formats, unexpected/missing columns.
- Issue isolation: any datalines with a detect issue are seperated from the data and isolated in an error table.


Project Notes:
- Developed in VS2015 using the nunit package for unit testing.
- Download the "NUnit 3 Test Adapter" extension to view example tests in the Test Explorer
- Although a trivial application implements the module, examples of usage are best exemplified through the unit tests.


Simple Usage Example (has columnNames but no dataType)

	DataTable oDt = DbCsvPlus.QuickLoadDataTable(csvFilePath, true, false);
	
	
Advanced Usage Example:

	DbCsvPlusRules oRules = new DbCsvPlusRules(true, true, false, "Date,Description,Amount", "Account Number,Currency");
	oRules.ForceDataTypes = "System.DateTime, System.String, System.Double";
	oRules.DataTypeFormats = 
		"yyyy/MM/dd" + "," +
		"^[C][H][Q][#](?<P1>[0-9]{5})[-](?<P2>[0-9]{10})$" + "," + 
		"";


	DbCsvPlusError oError = new DbCsvPlusError();
	
	DataTable oDt = DbCsvPlus.LoadDataTable(tbFileName.Text, null, true, false, false, ',', oRules, oError);