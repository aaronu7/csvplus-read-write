# csvplus-read-write

This module was designed as a component for a larger ETL system in which control over various extract sources is limited and changes/errors are constant; it is currently being used in a production ETL system to help verify the integrity of input data through a web-service API. 


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