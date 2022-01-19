namespace popww_betafix
{
    partial class About
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
            this.label_title = new System.Windows.Forms.Label();
            this.label_credits = new System.Windows.Forms.Label();
            this.btn_close = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_title
            // 
            this.label_title.AutoSize = true;
            this.label_title.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_title.Location = new System.Drawing.Point(59, 13);
            this.label_title.Name = "label_title";
            this.label_title.Size = new System.Drawing.Size(128, 16);
            this.label_title.TabIndex = 0;
            this.label_title.Text = "WW Beta patcher";
            // 
            // label_credits
            // 
            this.label_credits.AutoSize = true;
            this.label_credits.Location = new System.Drawing.Point(56, 63);
            this.label_credits.Name = "label_credits";
            this.label_credits.Size = new System.Drawing.Size(134, 182);
            this.label_credits.TabIndex = 1;
            this.label_credits.Text = "Created by BlackDaemon.\r\n\r\nThanks to:\r\n\r\nharryoke\r\nCheatEngine devs\r\nx64dbg devs\r" +
    "\nPCSX2 devs\r\nPPSSPP devs\r\nghidra devs\r\nghidra-emotionengine devs\r\nghidra-allegre" +
    "x devs\r\nRikuKH3\r\nYoti";
            this.label_credits.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_close
            // 
            this.btn_close.Location = new System.Drawing.Point(84, 282);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(75, 23);
            this.btn_close.TabIndex = 2;
            this.btn_close.Text = "Close";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(244, 322);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.label_credits);
            this.Controls.Add(this.label_title);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "About";
            this.Text = "About";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_title;
        private System.Windows.Forms.Label label_credits;
        private System.Windows.Forms.Button btn_close;
    }
}