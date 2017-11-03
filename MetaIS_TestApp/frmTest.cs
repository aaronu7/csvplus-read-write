using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MetaIS.Dbs.Csv;

namespace MetaIS_TestApp
{
    public partial class frmTest : Form
    {
        public frmTest()
        {
            InitializeComponent();
            this.ControlBox = true;
        }

        private void btnBrowse_Click(object sender, EventArgs e) {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = Application.StartupPath + @"\UnitTests\Files\";
            DialogResult res = dlg.ShowDialog();
            if(res==DialogResult.OK || res==DialogResult.Yes) {
                tbFileName.Text = dlg.FileName;
                LoadData(tbFileName.Text);
            }
        }

        private void LoadData(string filePath)
        {
            lblDataCount.Text = "Count: ";
            lblErrorCount.Text = "Count: ";
            listBox1.Items.Clear();

            if(System.IO.File.Exists(filePath)) {
                DbCsvPlusError oError = new DbCsvPlusError();
                DbCsvPlusRules oRules = null;

                string sampleRoot = Application.StartupPath + @"\UnitTests\Files\";
                if(!tbFileName.Text.StartsWith(sampleRoot)) {
                    oRules = new DbCsvPlusRules(true, true, false, "", "");
                    tbDataMsg.Text = "New file data, no rules loaded -see code.";
                } else {
                    // This is from our sample set, so lets load some special rules
                    oRules = new DbCsvPlusRules(true, true, false, "Date,Description,Amount", "Account Number,Currency");
                    oRules.ForceDataTypes = "System.DateTime, System.String, System.Double";
                    oRules.DataTypeFormats = 
                        "yyyy/MM/dd" + "," +
                        "^[C][H][Q][#](?<P1>[0-9]{5})[-](?<P2>[0-9]{10})$" + "," +    // this will create 3 capture groups (all-index0, P1-index1, P2-index2) it also forces a specific number length
                        "";

                    tbDataMsg.Text = "Rules loaded for default test cases.";
                }

                DataTable oDt = DbCsvPlus.LoadDataTable(tbFileName.Text, null, true, false, false, ',', oRules, oError);
                gridData.DataSource = oDt;
                gridError.DataSource = oError.ErrorDataTable;

                if(oDt!=null)
                    lblDataCount.Text = "Count: " + oDt.Rows.Count;

                if(oError.ErrorDataTable!=null)
                    lblErrorCount.Text = "Count: " + oError.ErrorDataTable.Rows.Count;

                if(oError.ErrorFlagSet!=null) {
                    foreach(string key in oError.ErrorFlagSet.Keys)
                        listBox1.Items.Add(key);

                }
            }
        }

        private void frmTest_Load(object sender, EventArgs e)
        {

        }
    }
}
