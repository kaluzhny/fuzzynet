namespace AI.Fuzzy.Samples.CruiseControlSample
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
            this.btnRun = new System.Windows.Forms.Button();
            this.gbCrispResult = new System.Windows.Forms.GroupBox();
            this.tbAccelerate = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.fbInput = new System.Windows.Forms.GroupBox();
            this.nudInputSpeedErrorDot = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nudInputSpeedError = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.gbCrispResult.SuspendLayout();
            this.fbInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudInputSpeedErrorDot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInputSpeedError)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRun
            // 
            this.btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRun.Location = new System.Drawing.Point(199, 149);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 11;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // gbCrispResult
            // 
            this.gbCrispResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbCrispResult.Controls.Add(this.tbAccelerate);
            this.gbCrispResult.Controls.Add(this.label19);
            this.gbCrispResult.Location = new System.Drawing.Point(12, 99);
            this.gbCrispResult.Name = "gbCrispResult";
            this.gbCrispResult.Size = new System.Drawing.Size(262, 43);
            this.gbCrispResult.TabIndex = 10;
            this.gbCrispResult.TabStop = false;
            this.gbCrispResult.Text = "Result";
            // 
            // tbAccelerate
            // 
            this.tbAccelerate.Location = new System.Drawing.Point(164, 15);
            this.tbAccelerate.Name = "tbAccelerate";
            this.tbAccelerate.ReadOnly = true;
            this.tbAccelerate.Size = new System.Drawing.Size(89, 20);
            this.tbAccelerate.TabIndex = 17;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(6, 18);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(75, 13);
            this.label19.TabIndex = 0;
            this.label19.Text = "Accelerate, %:";
            // 
            // fbInput
            // 
            this.fbInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fbInput.Controls.Add(this.nudInputSpeedErrorDot);
            this.fbInput.Controls.Add(this.label2);
            this.fbInput.Controls.Add(this.nudInputSpeedError);
            this.fbInput.Controls.Add(this.label1);
            this.fbInput.Location = new System.Drawing.Point(12, 11);
            this.fbInput.Name = "fbInput";
            this.fbInput.Size = new System.Drawing.Size(262, 83);
            this.fbInput.TabIndex = 9;
            this.fbInput.TabStop = false;
            this.fbInput.Text = "Input";
            // 
            // nudInputSpeedErrorDot
            // 
            this.nudInputSpeedErrorDot.DecimalPlaces = 1;
            this.nudInputSpeedErrorDot.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.nudInputSpeedErrorDot.Location = new System.Drawing.Point(164, 47);
            this.nudInputSpeedErrorDot.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudInputSpeedErrorDot.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            -2147483648});
            this.nudInputSpeedErrorDot.Name = "nudInputSpeedErrorDot";
            this.nudInputSpeedErrorDot.Size = new System.Drawing.Size(89, 20);
            this.nudInputSpeedErrorDot.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Speed Error\', km/h*h (-5 – 5):";
            // 
            // nudInputSpeedError
            // 
            this.nudInputSpeedError.DecimalPlaces = 1;
            this.nudInputSpeedError.Location = new System.Drawing.Point(164, 17);
            this.nudInputSpeedError.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudInputSpeedError.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            -2147483648});
            this.nudInputSpeedError.Name = "nudInputSpeedError";
            this.nudInputSpeedError.Size = new System.Drawing.Size(89, 20);
            this.nudInputSpeedError.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Speed Error, km/h (-20 – 20):";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(286, 182);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.gbCrispResult);
            this.Controls.Add(this.fbInput);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Cruise Control Sample (Sugeno)";
            this.gbCrispResult.ResumeLayout(false);
            this.gbCrispResult.PerformLayout();
            this.fbInput.ResumeLayout(false);
            this.fbInput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudInputSpeedErrorDot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInputSpeedError)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.GroupBox gbCrispResult;
        private System.Windows.Forms.TextBox tbAccelerate;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.GroupBox fbInput;
        private System.Windows.Forms.NumericUpDown nudInputSpeedErrorDot;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudInputSpeedError;
        private System.Windows.Forms.Label label1;
    }
}

