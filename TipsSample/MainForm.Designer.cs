namespace AI.Fuzzy.Samples.TipsSample
{
    partial class MainForm
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
            this.fbInput = new System.Windows.Forms.GroupBox();
            this.nudInputFood = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nudInputService = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.gbCrispResult = new System.Windows.Forms.GroupBox();
            this.tbTips = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.btnRun = new System.Windows.Forms.Button();
            this.fbInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudInputFood)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInputService)).BeginInit();
            this.gbCrispResult.SuspendLayout();
            this.SuspendLayout();
            // 
            // fbInput
            // 
            this.fbInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fbInput.Controls.Add(this.nudInputFood);
            this.fbInput.Controls.Add(this.label2);
            this.fbInput.Controls.Add(this.nudInputService);
            this.fbInput.Controls.Add(this.label1);
            this.fbInput.Location = new System.Drawing.Point(12, 12);
            this.fbInput.Name = "fbInput";
            this.fbInput.Size = new System.Drawing.Size(435, 48);
            this.fbInput.TabIndex = 2;
            this.fbInput.TabStop = false;
            this.fbInput.Text = "Input";
            // 
            // nudInputFood
            // 
            this.nudInputFood.DecimalPlaces = 1;
            this.nudInputFood.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudInputFood.Location = new System.Drawing.Point(308, 15);
            this.nudInputFood.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudInputFood.Name = "nudInputFood";
            this.nudInputFood.Size = new System.Drawing.Size(89, 20);
            this.nudInputFood.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(231, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Food (0 – 10):";
            // 
            // nudInputService
            // 
            this.nudInputService.DecimalPlaces = 1;
            this.nudInputService.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudInputService.Location = new System.Drawing.Point(97, 15);
            this.nudInputService.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudInputService.Name = "nudInputService";
            this.nudInputService.Size = new System.Drawing.Size(89, 20);
            this.nudInputService.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Service (0 – 10):";
            // 
            // gbCrispResult
            // 
            this.gbCrispResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbCrispResult.Controls.Add(this.tbTips);
            this.gbCrispResult.Controls.Add(this.label19);
            this.gbCrispResult.Location = new System.Drawing.Point(12, 67);
            this.gbCrispResult.Name = "gbCrispResult";
            this.gbCrispResult.Size = new System.Drawing.Size(435, 43);
            this.gbCrispResult.TabIndex = 7;
            this.gbCrispResult.TabStop = false;
            this.gbCrispResult.Text = "Result";
            // 
            // tbTips
            // 
            this.tbTips.Location = new System.Drawing.Point(97, 15);
            this.tbTips.Name = "tbTips";
            this.tbTips.ReadOnly = true;
            this.tbTips.Size = new System.Drawing.Size(89, 20);
            this.tbTips.TabIndex = 17;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(6, 18);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(44, 13);
            this.label19.TabIndex = 0;
            this.label19.Text = "Tips, %:";
            // 
            // btnRun
            // 
            this.btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRun.Location = new System.Drawing.Point(372, 121);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 8;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 154);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.gbCrispResult);
            this.Controls.Add(this.fbInput);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Tips Sample (Mamdani)";
            this.fbInput.ResumeLayout(false);
            this.fbInput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudInputFood)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInputService)).EndInit();
            this.gbCrispResult.ResumeLayout(false);
            this.gbCrispResult.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox fbInput;
        private System.Windows.Forms.NumericUpDown nudInputFood;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudInputService;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbCrispResult;
        private System.Windows.Forms.TextBox tbTips;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Button btnRun;
    }
}

