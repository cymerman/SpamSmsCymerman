namespace SpamSmsLicencjat
{
    partial class StartForm
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.perceptronButton = new System.Windows.Forms.Button();
            this.naiveBayesButton = new System.Windows.Forms.Button();
            this.logisticRegressionButton = new System.Windows.Forms.Button();
            this.mainLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // perceptronButton
            // 
            this.perceptronButton.Location = new System.Drawing.Point(53, 99);
            this.perceptronButton.Name = "perceptronButton";
            this.perceptronButton.Size = new System.Drawing.Size(200, 106);
            this.perceptronButton.TabIndex = 0;
            this.perceptronButton.Text = "Perceptron";
            this.perceptronButton.UseVisualStyleBackColor = true;
            // 
            // naiveBayesButton
            // 
            this.naiveBayesButton.Location = new System.Drawing.Point(280, 99);
            this.naiveBayesButton.Name = "naiveBayesButton";
            this.naiveBayesButton.Size = new System.Drawing.Size(182, 106);
            this.naiveBayesButton.TabIndex = 1;
            this.naiveBayesButton.Text = "Naiwny Bayes (Naive Bayes)";
            this.naiveBayesButton.UseVisualStyleBackColor = true;
            this.naiveBayesButton.Click += new System.EventHandler(this.naiveBayesButton_Click);
            // 
            // logisticRegressionButton
            // 
            this.logisticRegressionButton.Location = new System.Drawing.Point(498, 99);
            this.logisticRegressionButton.Name = "logisticRegressionButton";
            this.logisticRegressionButton.Size = new System.Drawing.Size(217, 106);
            this.logisticRegressionButton.TabIndex = 2;
            this.logisticRegressionButton.Text = "Logistyczna regresja (Logistic Regression)";
            this.logisticRegressionButton.UseVisualStyleBackColor = true;
            this.logisticRegressionButton.Click += new System.EventHandler(this.logisticRegressionButton_Click);
            // 
            // mainLabel
            // 
            this.mainLabel.AutoSize = true;
            this.mainLabel.Font = new System.Drawing.Font("Segoe Print", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.mainLabel.Location = new System.Drawing.Point(243, 37);
            this.mainLabel.Name = "mainLabel";
            this.mainLabel.Size = new System.Drawing.Size(249, 36);
            this.mainLabel.TabIndex = 3;
            this.mainLabel.Text = "Algorytmy do wyboru";
            // 
            // StartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(787, 298);
            this.Controls.Add(this.mainLabel);
            this.Controls.Add(this.logisticRegressionButton);
            this.Controls.Add(this.naiveBayesButton);
            this.Controls.Add(this.perceptronButton);
            this.Name = "StartForm";
            this.Text = "Filtrowanie spam sms";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button perceptronButton;
        private System.Windows.Forms.Button naiveBayesButton;
        private System.Windows.Forms.Button logisticRegressionButton;
        private System.Windows.Forms.Label mainLabel;
    }
}

