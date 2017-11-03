# csvplus-read-write

[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT) 


[![Build Status](https://travis-ci.org/aaronu7/saas-plugins.svg?branch=master)](https://travis-ci.org/aaronu7/saas-plugins) [![NuGet](https://img.shields.io/nuget/v/MetaIS.SaaS.Plugins.svg)](https://www.nuget.org/packages/MetaIS.SaaS.Plugins/)

This module was designed as a component for a larger ETL system in which control over various extract sources is limited and changes/errors are a frequent occurrence; it is currently being used in a production ETL system to help verify the integrity of input data through a web-service API. 

Where this fits in the ETL (EXTRACT / scrub / blend / clean&reduce / TRANSFORM / LOAD):
- It is a pre-processing algorithm which runs when loading the "raw unverified" csv extract data.
- It is usually run when loading data into the clean&reduce algorithm, but running it immediately after an extract can be used to re-trigger the extract.
- Its purpose is to detect any unexpected or problematic data early on in the ETL process. 
- Any problematic data is removed and isolated in a separate error table which can be analysed at a later point in the ETL.

Key Features:
- DataType retention: an optional DataType line under the column header is available for Load/Save operations.
- Issue detection: too many fields, too few fields, bad datatypes and formats, unexpected/missing columns.
- Issue isolation: any datalines with detected issues are separated from the data and isolated in an error table.


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
	
	
Potential Upgrades:
- Allow table rules to be passed as JSON configuration files.
- Implement automated error handling as part of the rules.