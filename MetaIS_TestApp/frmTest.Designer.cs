namespace MetaIS_TestApp
{
    partial class frmTest
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnBrowse = new System.Windows.Forms.Button();
            this.tbFileName = new System.Windows.Forms.TextBox();
            this.tbMsg = new System.Windows.Forms.TextBox();
            this.gridData = new System.Windows.Forms.DataGridView();
            this.gridError = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.lblDataCount = new System.Windows.Forms.Label();
            this.lblErrorCount = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbDataMsg = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridError)).BeginInit();
            this.SuspendLayout();
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(34, 130);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(177, 61);
            this.btnBrowse.TabIndex = 0;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // tbFileName
            // 
            this.tbFileName.Location = new System.Drawing.Point(217, 145);
            this.tbFileName.Name = "tbFileName";
            this.tbFileName.Size = new System.Drawing.Size(1279, 31);
            this.tbFileName.TabIndex = 1;
            // 
            // tbMsg
            // 
            this.tbMsg.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.tbMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbMsg.Location = new System.Drawing.Point(249, 23);
            this.tbMsg.Multiline = true;
            this.tbMsg.Name = "tbMsg";
            this.tbMsg.Size = new System.Drawing.Size(1247, 84);
            this.tbMsg.TabIndex = 3;
            this.tbMsg.Text = "Test cases can be found in the UnitTests folder and are viewable in the Test Expl" +
    "orer with the \"NUnit 3 Test Adapter\" extension. Use this tool to browse for test" +
    " data and observe the outputs.";
            // 
            // gridData
            // 
            this.gridData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridData.Location = new System.Drawing.Point(217, 304);
            this.gridData.Name = "gridData";
            this.gridData.RowTemplate.Height = 33;
            this.gridData.Size = new System.Drawing.Size(1307, 403);
            this.gridData.TabIndex = 5;
            // 
            // gridError
            // 
            this.gridError.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridError.Location = new System.Drawing.Point(217, 744);
            this.gridError.Name = "gridError";
            this.gridError.RowTemplate.Height = 33;
            this.gridError.Size = new System.Drawing.Size(1307, 400);
            this.gridError.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 382);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 37);
            this.label1.TabIndex = 7;
            this.label1.Text = "Data";
            // 
            // lblDataCount
            // 
            this.lblDataCount.AutoSize = true;
            this.lblDataCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDataCount.Location = new System.Drawing.Point(12, 430);
            this.lblDataCount.Name = "lblDataCount";
            this.lblDataCount.Size = new System.Drawing.Size(121, 37);
            this.lblDataCount.TabIndex = 8;
            this.lblDataCount.Text = "Count: ";
            // 
            // lblErrorCount
            // 
            this.lblErrorCount.AutoSize = true;
            this.lblErrorCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrorCount.ForeColor = System.Drawing.Color.Red;
            this.lblErrorCount.Location = new System.Drawing.Point(12, 885);
            this.lblErrorCount.Name = "lblErrorCount";
            this.lblErrorCount.Size = new System.Drawing.Size(121, 37);
            this.lblErrorCount.TabIndex = 10;
            this.lblErrorCount.Text = "Count: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(12, 837);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 37);
            this.label3.TabIndex = 9;
            this.label3.Text = "Error";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 25;
            this.listBox1.Location = new System.Drawing.Point(1655, 740);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(355, 404);
            this.listBox1.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(1603, 689);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(186, 37);
            this.label2.TabIndex = 12;
            this.label2.Text = "Error Flags:";
            // 
            // tbDataMsg
            // 
            this.tbDataMsg.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.tbDataMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbDataMsg.Location = new System.Drawing.Point(249, 199);
            this.tbDataMsg.Multiline = true;
            this.tbDataMsg.Name = "tbDataMsg";
            this.tbDataMsg.Size = new System.Drawing.Size(1247, 76);
            this.tbDataMsg.TabIndex = 13;
            // 
            // frmTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2140, 1227);
            this.Controls.Add(this.tbDataMsg);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.lblErrorCount);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblDataCount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gridError);
            this.Controls.Add(this.gridData);
            this.Controls.Add(this.tbMsg);
            this.Controls.Add(this.tbFileName);
            this.Controls.Add(this.btnBrowse);
            this.Name = "frmTest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmTest";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmTest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridError)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox tbFileName;
        private System.Windows.Forms.TextBox tbMsg;
        private System.Windows.Forms.DataGridView gridData;
        private System.Windows.Forms.DataGridView gridError;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblDataCount;
        private System.Windows.Forms.Label lblErrorCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbDataMsg;
    }
}