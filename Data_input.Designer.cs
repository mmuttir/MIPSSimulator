
namespace CSASemesterProject
{
    partial class Data_input
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
            this.DataName = new System.Windows.Forms.TextBox();
            this.DataType = new System.Windows.Forms.ComboBox();
            this.DataBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.AddData = new System.Windows.Forms.Button();
            this.ClearData = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // DataName
            // 
            this.DataName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.DataName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DataName.ForeColor = System.Drawing.Color.White;
            this.DataName.Location = new System.Drawing.Point(186, 116);
            this.DataName.Name = "DataName";
            this.DataName.Size = new System.Drawing.Size(100, 20);
            this.DataName.TabIndex = 0;
            // 
            // DataType
            // 
            this.DataType.FormattingEnabled = true;
            this.DataType.Location = new System.Drawing.Point(340, 116);
            this.DataType.Name = "DataType";
            this.DataType.Size = new System.Drawing.Size(121, 21);
            this.DataType.TabIndex = 1;
            // 
            // DataBox
            // 
            this.DataBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.DataBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DataBox.ForeColor = System.Drawing.Color.White;
            this.DataBox.Location = new System.Drawing.Point(515, 116);
            this.DataBox.Name = "DataBox";
            this.DataBox.Size = new System.Drawing.Size(100, 20);
            this.DataBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(186, 97);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Data Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(337, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Datatype";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(512, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Data";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(186, 194);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(429, 159);
            this.dataGridView1.TabIndex = 6;
            // 
            // AddData
            // 
            this.AddData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.AddData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddData.ForeColor = System.Drawing.Color.White;
            this.AddData.Location = new System.Drawing.Point(340, 151);
            this.AddData.Name = "AddData";
            this.AddData.Size = new System.Drawing.Size(121, 29);
            this.AddData.TabIndex = 7;
            this.AddData.Text = "Add Data";
            this.AddData.UseVisualStyleBackColor = false;
            this.AddData.Click += new System.EventHandler(this.AddData_Click);
            // 
            // ClearData
            // 
            this.ClearData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClearData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ClearData.ForeColor = System.Drawing.Color.White;
            this.ClearData.Location = new System.Drawing.Point(340, 359);
            this.ClearData.Name = "ClearData";
            this.ClearData.Size = new System.Drawing.Size(121, 29);
            this.ClearData.TabIndex = 8;
            this.ClearData.Text = "Clear Data";
            this.ClearData.UseVisualStyleBackColor = false;
            this.ClearData.Click += new System.EventHandler(this.ClearData_Click);
            // 
            // Data_input
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ClearData);
            this.Controls.Add(this.AddData);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DataBox);
            this.Controls.Add(this.DataType);
            this.Controls.Add(this.DataName);
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "Data_input";
            this.Text = "Data Input";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox DataName;
        private System.Windows.Forms.ComboBox DataType;
        private System.Windows.Forms.TextBox DataBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button AddData;
        private System.Windows.Forms.Button ClearData;
    }
}