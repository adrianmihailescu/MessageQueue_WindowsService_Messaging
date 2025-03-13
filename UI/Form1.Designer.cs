namespace UI
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPrenume = new System.Windows.Forms.TextBox();
            this.txtNume = new System.Windows.Forms.TextBox();
            this.dtDataAngajarii = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSubmitForm = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtPrenume, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtNume, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.dtDataAngajarii, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(13, 13);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(343, 88);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Right;
            this.label3.Location = new System.Drawing.Point(96, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 36);
            this.label3.TabIndex = 5;
            this.label3.Text = "Data angajarii";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Right;
            this.label2.Location = new System.Drawing.Point(119, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 26);
            this.label2.TabIndex = 4;
            this.label2.Text = "Prenume";
            // 
            // txtPrenume
            // 
            this.txtPrenume.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPrenume.Location = new System.Drawing.Point(174, 29);
            this.txtPrenume.Name = "txtPrenume";
            this.txtPrenume.Size = new System.Drawing.Size(166, 20);
            this.txtPrenume.TabIndex = 1;
            // 
            // txtNume
            // 
            this.txtNume.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNume.Location = new System.Drawing.Point(174, 3);
            this.txtNume.Name = "txtNume";
            this.txtNume.Size = new System.Drawing.Size(166, 20);
            this.txtNume.TabIndex = 0;
            // 
            // dtDataAngajarii
            // 
            this.dtDataAngajarii.Location = new System.Drawing.Point(174, 55);
            this.dtDataAngajarii.Name = "dtDataAngajarii";
            this.dtDataAngajarii.Size = new System.Drawing.Size(166, 20);
            this.dtDataAngajarii.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Right;
            this.label1.Location = new System.Drawing.Point(133, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 26);
            this.label1.TabIndex = 3;
            this.label1.Text = "Nume";
            // 
            // btnSubmitForm
            // 
            this.btnSubmitForm.Location = new System.Drawing.Point(217, 107);
            this.btnSubmitForm.Name = "btnSubmitForm";
            this.btnSubmitForm.Size = new System.Drawing.Size(139, 23);
            this.btnSubmitForm.TabIndex = 1;
            this.btnSubmitForm.Text = "Trimite";
            this.btnSubmitForm.UseVisualStyleBackColor = true;
            this.btnSubmitForm.Click += new System.EventHandler(this.btnSubmitForm_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 136);
            this.Controls.Add(this.btnSubmitForm);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtPrenume;
        private System.Windows.Forms.TextBox txtNume;
        private System.Windows.Forms.DateTimePicker dtDataAngajarii;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSubmitForm;
        private System.Windows.Forms.Timer timer1;

    }
}

