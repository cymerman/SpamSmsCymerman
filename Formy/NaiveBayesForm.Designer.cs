namespace SpamSmsLicencjat
{
    partial class NaiveBayesForm
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
            this.labelLoading = new System.Windows.Forms.Label();
            this.buttonCheckSpam = new System.Windows.Forms.Button();
            this.textBoxCheckSpam = new System.Windows.Forms.TextBox();
            this.isSpamLabel = new System.Windows.Forms.Label();
            this.buttonShowDiagrams = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelLoading
            // 
            this.labelLoading.AutoSize = true;
            this.labelLoading.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelLoading.Location = new System.Drawing.Point(352, 80);
            this.labelLoading.Name = "labelLoading";
            this.labelLoading.Size = new System.Drawing.Size(223, 25);
            this.labelLoading.TabIndex = 0;
            this.labelLoading.Text = "Trwa ładowanie...";
            // 
            // buttonCheckSpam
            // 
            this.buttonCheckSpam.Location = new System.Drawing.Point(59, 327);
            this.buttonCheckSpam.Name = "buttonCheckSpam";
            this.buttonCheckSpam.Size = new System.Drawing.Size(174, 47);
            this.buttonCheckSpam.TabIndex = 1;
            this.buttonCheckSpam.Text = "Sprawdź czy spam";
            this.buttonCheckSpam.UseVisualStyleBackColor = true;
            this.buttonCheckSpam.Click += new System.EventHandler(this.buttonCheckSpam_Click);
            // 
            // textBoxCheckSpam
            // 
            this.textBoxCheckSpam.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCheckSpam.Location = new System.Drawing.Point(59, 183);
            this.textBoxCheckSpam.Multiline = true;
            this.textBoxCheckSpam.Name = "textBoxCheckSpam";
            this.textBoxCheckSpam.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxCheckSpam.Size = new System.Drawing.Size(544, 138);
            this.textBoxCheckSpam.TabIndex = 2;
            // 
            // isSpamLabel
            // 
            this.isSpamLabel.AutoSize = true;
            this.isSpamLabel.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.isSpamLabel.ForeColor = System.Drawing.Color.Red;
            this.isSpamLabel.Location = new System.Drawing.Point(298, 361);
            this.isSpamLabel.Name = "isSpamLabel";
            this.isSpamLabel.Size = new System.Drawing.Size(0, 18);
            this.isSpamLabel.TabIndex = 4;
            // 
            // buttonShowDiagrams
            // 
            this.buttonShowDiagrams.Location = new System.Drawing.Point(727, 80);
            this.buttonShowDiagrams.Name = "buttonShowDiagrams";
            this.buttonShowDiagrams.Size = new System.Drawing.Size(203, 57);
            this.buttonShowDiagrams.TabIndex = 5;
            this.buttonShowDiagrams.Text = "Pokaż wykresy";
            this.buttonShowDiagrams.UseVisualStyleBackColor = true;
            this.buttonShowDiagrams.Click += new System.EventHandler(this.buttonShowDiagrams_Click);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(45, 50);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(174, 73);
            this.buttonLoad.TabIndex = 6;
            this.buttonLoad.Text = "Utwórz model";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // NaiveBayesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(952, 423);
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.buttonShowDiagrams);
            this.Controls.Add(this.isSpamLabel);
            this.Controls.Add(this.textBoxCheckSpam);
            this.Controls.Add(this.buttonCheckSpam);
            this.Controls.Add(this.labelLoading);
            this.Name = "NaiveBayesForm";
            this.Text = "Algorytm - Naiwny Bayes";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelLoading;
        private System.Windows.Forms.Button buttonCheckSpam;
        private System.Windows.Forms.TextBox textBoxCheckSpam;
        private System.Windows.Forms.Label isSpamLabel;
        private System.Windows.Forms.Button buttonShowDiagrams;
        private System.Windows.Forms.Button buttonLoad;
    }
}