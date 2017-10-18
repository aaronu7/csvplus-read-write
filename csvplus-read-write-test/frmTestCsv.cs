using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using csvplus_read_write.Db;

namespace csvplus_read_write_test
{
    public partial class frmTestCsv : Form
    {
        public frmTestCsv()
        {
            InitializeComponent();
        }

        //string dataPathCSV = @"E:\OneDrive - School District 27\DW\test.csv";
        string dataPathCSV = @"C:\Users\aaron\OneDrive\Work\Code_GitHub\csvplus-read-write\csvplus-read-write-test\bin\Debug\UnitTests\Files\chq_badRow.csv";

        //string dataPathLST = @"E:\OneDrive - School District 27\DW\test.lst";
        //string dataPathLST = @"C:\One Drive\OneDrive - School District 27\DW\test.lst";


        #region " CSV Test "

        private void btnLoadNullObjects_Click(object sender, EventArgs e)
        {
            DataTable oDtCsv = DbCsvPlus.LoadDataTable(dataPathCSV, null, true, false, false, ',', null, null);
            dataGridView1.DataSource = oDtCsv;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            DbCsvPlusError oError = new DbCsvPlusError(System.IO.Path.GetFileName(dataPathCSV));
            DbCsvPlusRules oRules = new DbCsvPlusRules(true, true, false, "Date,Description,Amount", "Account Number,Currency");
            oRules.ForceDataTypes = "System.DateTime, System.String, System.Double";
            oRules.DataTypeFormats = 
                "yyyy/MM/dd" + "," +
                "^[C][H][Q][#](?<P1>[0-9]{5})[-](?<P2>[0-9]{10})$" + "," +    // this will create 3 capture groups (all-index0, P1-index1, P2-index2) it also forces a specific number length
                //"^[C][H][Q][#](?<P1>[0-9]+)[-](?<P2>[0-9]+)$" + "," +       // this will create 3 capture groups (all-index0, P1-index1, P2-index2)
                //"^[C][H][Q][#][0-9]+[-][0-9]+$" + "," +
                "";
            DataTable oDtCsv = DbCsvPlus.LoadDataTable(dataPathCSV, null, true, false, false, ',', oRules, oError);




            // Display resaults
            dataGridView1.DataSource = oDtCsv;
            dataGridView2.DataSource = oError.oDtErr;
            tbLog.Text = oError.logMsg;
            if(oError.logMsg.Trim() != "") {
                tabControl1.SelectedTab = tabPage3;
            }
            /*
            UpdateLabel(label4, oError.error_HeaderRequiredMissing, "error_HeaderRequiredMissing");
            UpdateLabel(label5, oError.error_HeaderOptionalMissing, "error_HeaderOptionalMissing");
            UpdateLabel(label6, oError.error_HeaderNewWarning, "error_HeaderNewWarning");
            UpdateLabel(label7, oError.error_HeaderDuplicate, "error_HeaderDuplicate");
            UpdateLabel(label8, oError.error_HeaderBadName, "error_HeaderBadName");
            UpdateLabel(label9, oError.error_HeaderPoorNameWarning, "error_HeaderPoorNameWarning");

            UpdateLabel(label1, oError.error_DataFldTooFew, "error_DataFldTooFew");
            UpdateLabel(label2, oError.error_DataFldTooMany, "error_DataFldTooMany");
            UpdateLabel(label3, oError.error_DataFldOnLastRow, "error_DataFldOnLastRow");
            UpdateLabel(label10,oError.error_DataFldBadType, "error_DataFldBadType");
            */
        }

        #endregion

        protected void UpdateLabel(Label lbl, bool res, string text) {
            lbl.Text = text;
            lbl.ForeColor = Color.WhiteSmoke;

            if(res)
                lbl.BackColor = Color.Red;
            else 
                lbl.BackColor = Color.Green;
                
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


    }
}
