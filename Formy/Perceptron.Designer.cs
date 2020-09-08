namespace SpamSmsLicencjat.Formy
{
    partial class Perceptron
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
            this.labelWriteMessage = new System.Windows.Forms.Label();
            this.labelLog = new System.Windows.Forms.Label();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.buttonShowDiagrams = new System.Windows.Forms.Button();
            this.textBoxCheckSpam = new System.Windows.Forms.TextBox();
            this.buttonCheckSpam = new System.Windows.Forms.Button();
            this.labelLoading = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelWriteMessage
            // 
            this.labelWriteMessage.AutoSize = true;
            this.labelWriteMessage.Location = new System.Drawing.Point(64, 177);
            this.labelWriteMessage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelWriteMessage.Name = "labelWriteMessage";
            this.labelWriteMessage.Size = new System.Drawing.Size(314, 15);
            this.labelWriteMessage.TabIndex = 15;
            this.labelWriteMessage.Text = "Wpisz po angielsku wiadomość i sprawdź czy jest spamem";
            this.labelWriteMessage.Visible = false;
            // 
            // labelLog
            // 
            this.labelLog.AutoSize = true;
            this.labelLog.Location = new System.Drawing.Point(765, 177);
            this.labelLog.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLog.Name = "labelLog";
            this.labelLog.Size = new System.Drawing.Size(87, 15);
            this.labelLog.TabIndex = 14;
            this.labelLog.Text = "Wyniki modelu";
            this.labelLog.Visible = false;
            // 
            // textBoxLog
            // 
            this.textBoxLog.Location = new System.Drawing.Point(765, 227);
            this.textBoxLog.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.Size = new System.Drawing.Size(604, 376);
            this.textBoxLog.TabIndex = 13;
            this.textBoxLog.Visible = false;
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(64, 37);
            this.buttonLoad.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(203, 84);
            this.buttonLoad.TabIndex = 12;
            this.buttonLoad.Text = "Utwórz model";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // buttonShowDiagrams
            // 
            this.buttonShowDiagrams.Location = new System.Drawing.Point(784, 37);
            this.buttonShowDiagrams.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonShowDiagrams.Name = "buttonShowDiagrams";
            this.buttonShowDiagrams.Size = new System.Drawing.Size(237, 66);
            this.buttonShowDiagrams.TabIndex = 11;
            this.buttonShowDiagrams.Text = "Pokaż wykresy";
            this.buttonShowDiagrams.UseVisualStyleBackColor = true;
            this.buttonShowDiagrams.Click += new System.EventHandler(this.buttonShowDiagrams_Click);
            // 
            // textBoxCheckSpam
            // 
            this.textBoxCheckSpam.Location = new System.Drawing.Point(64, 206);
            this.textBoxCheckSpam.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBoxCheckSpam.Multiline = true;
            this.textBoxCheckSpam.Name = "textBoxCheckSpam";
            this.textBoxCheckSpam.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxCheckSpam.Size = new System.Drawing.Size(450, 161);
            this.textBoxCheckSpam.TabIndex = 9;
            // 
            // buttonCheckSpam
            // 
            this.buttonCheckSpam.Location = new System.Drawing.Point(64, 413);
            this.buttonCheckSpam.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonCheckSpam.Name = "buttonCheckSpam";
            this.buttonCheckSpam.Size = new System.Drawing.Size(203, 54);
            this.buttonCheckSpam.TabIndex = 8;
            this.buttonCheckSpam.Text = "Sprawdź czy spam";
            this.buttonCheckSpam.UseVisualStyleBackColor = true;
            this.buttonCheckSpam.Click += new System.EventHandler(this.buttonCheckSpam_Click);
            // 
            // labelLoading
            // 
            this.labelLoading.AutoSize = true;
            this.labelLoading.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelLoading.Location = new System.Drawing.Point(422, 72);
            this.labelLoading.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLoading.Name = "labelLoading";
            this.labelLoading.Size = new System.Drawing.Size(223, 25);
            this.labelLoading.TabIndex = 7;
            this.labelLoading.Text = "Trwa ładowanie...";
            // 
            // Perceptron
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1380, 611);
            this.Controls.Add(this.labelLoading);
            this.Controls.Add(this.buttonCheckSpam);
            this.Controls.Add(this.textBoxCheckSpam);
            this.Controls.Add(this.buttonShowDiagrams);
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.labelLog);
            this.Controls.Add(this.labelWriteMessage);
            this.Name = "Perceptron";
            this.Text = "Perceptron";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelWriteMessage;
        private System.Windows.Forms.Label labelLog;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.Button buttonShowDiagrams;
        private System.Windows.Forms.TextBox textBoxCheckSpam;
        private System.Windows.Forms.Button buttonCheckSpam;
        private System.Windows.Forms.Label labelLoading;
    }
}