namespace SpamSmsLicencjat
{
    partial class LogisticRegression
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
            this.buttonLoad = new System.Windows.Forms.Button();
            this.buttonShowDiagrams = new System.Windows.Forms.Button();
            this.textBoxCheckSpam = new System.Windows.Forms.TextBox();
            this.buttonCheckSpam = new System.Windows.Forms.Button();
            this.labelLoading = new System.Windows.Forms.Label();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.labelLog = new System.Windows.Forms.Label();
            this.labelWriteMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(159, 106);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(174, 73);
            this.buttonLoad.TabIndex = 12;
            this.buttonLoad.Text = "Utwórz model";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // buttonShowDiagrams
            // 
            this.buttonShowDiagrams.Location = new System.Drawing.Point(777, 106);
            this.buttonShowDiagrams.Name = "buttonShowDiagrams";
            this.buttonShowDiagrams.Size = new System.Drawing.Size(203, 57);
            this.buttonShowDiagrams.TabIndex = 11;
            this.buttonShowDiagrams.Text = "Pokaż wykresy";
            this.buttonShowDiagrams.UseVisualStyleBackColor = true;
            this.buttonShowDiagrams.Click += new System.EventHandler(this.buttonShowDiagrams_Click);
            // 
            // textBoxCheckSpam
            // 
            this.textBoxCheckSpam.Location = new System.Drawing.Point(159, 252);
            this.textBoxCheckSpam.Multiline = true;
            this.textBoxCheckSpam.Name = "textBoxCheckSpam";
            this.textBoxCheckSpam.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxCheckSpam.Size = new System.Drawing.Size(386, 140);
            this.textBoxCheckSpam.TabIndex = 9;
            // 
            // buttonCheckSpam
            // 
            this.buttonCheckSpam.Location = new System.Drawing.Point(159, 432);
            this.buttonCheckSpam.Name = "buttonCheckSpam";
            this.buttonCheckSpam.Size = new System.Drawing.Size(174, 47);
            this.buttonCheckSpam.TabIndex = 8;
            this.buttonCheckSpam.Text = "Sprawdź czy spam";
            this.buttonCheckSpam.UseVisualStyleBackColor = true;
            this.buttonCheckSpam.Click += new System.EventHandler(this.buttonCheckSpam_Click);
            // 
            // labelLoading
            // 
            this.labelLoading.AutoSize = true;
            this.labelLoading.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelLoading.Location = new System.Drawing.Point(466, 136);
            this.labelLoading.Name = "labelLoading";
            this.labelLoading.Size = new System.Drawing.Size(223, 25);
            this.labelLoading.TabIndex = 7;
            this.labelLoading.Text = "Trwa ładowanie...";
            // 
            // textBoxLog
            // 
            this.textBoxLog.Location = new System.Drawing.Point(760, 270);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.Size = new System.Drawing.Size(518, 326);
            this.textBoxLog.TabIndex = 13;
            this.textBoxLog.Visible = false;
            // 
            // labelLog
            // 
            this.labelLog.AutoSize = true;
            this.labelLog.Location = new System.Drawing.Point(760, 227);
            this.labelLog.Name = "labelLog";
            this.labelLog.Size = new System.Drawing.Size(76, 13);
            this.labelLog.TabIndex = 14;
            this.labelLog.Text = "Wyniki modelu";
            this.labelLog.Visible = false;
            // 
            // labelWriteMessage
            // 
            this.labelWriteMessage.AutoSize = true;
            this.labelWriteMessage.Location = new System.Drawing.Point(159, 227);
            this.labelWriteMessage.Name = "labelWriteMessage";
            this.labelWriteMessage.Size = new System.Drawing.Size(282, 13);
            this.labelWriteMessage.TabIndex = 15;
            this.labelWriteMessage.Text = "Wpisz po angielsku wiadomość i sprawdź czy jest spamem";
            this.labelWriteMessage.Visible = false;
            // 
            // LogisticRegression
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1357, 640);
            this.Controls.Add(this.labelWriteMessage);
            this.Controls.Add(this.labelLog);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.buttonShowDiagrams);
            this.Controls.Add(this.textBoxCheckSpam);
            this.Controls.Add(this.buttonCheckSpam);
            this.Controls.Add(this.labelLoading);
            this.Name = "LogisticRegression";
            this.Text = "Regresja logistyczna";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.Button buttonShowDiagrams;
        private System.Windows.Forms.TextBox textBoxCheckSpam;
        private System.Windows.Forms.Button buttonCheckSpam;
        private System.Windows.Forms.Label labelLoading;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.Label labelLog;
        private System.Windows.Forms.Label labelWriteMessage;
    }
}