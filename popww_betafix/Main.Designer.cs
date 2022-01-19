namespace popww_betafix
{
    partial class Main
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
            this.label_path = new System.Windows.Forms.Label();
            this.textBox_path = new System.Windows.Forms.TextBox();
            this.btn_path = new System.Windows.Forms.Button();
            this.btn_patch = new System.Windows.Forms.Button();
            this.label_about = new System.Windows.Forms.Label();
            this.richTextBox_log = new System.Windows.Forms.RichTextBox();
            this.label_log = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label_path
            // 
            this.label_path.AutoSize = true;
            this.label_path.Location = new System.Drawing.Point(12, 16);
            this.label_path.Name = "label_path";
            this.label_path.Size = new System.Drawing.Size(143, 13);
            this.label_path.TabIndex = 0;
            this.label_path.Text = "Select SLUS_210.22 or ISO:";
            // 
            // textBox_path
            // 
            this.textBox_path.Location = new System.Drawing.Point(15, 32);
            this.textBox_path.Name = "textBox_path";
            this.textBox_path.Size = new System.Drawing.Size(124, 20);
            this.textBox_path.TabIndex = 1;
            // 
            // btn_path
            // 
            this.btn_path.Location = new System.Drawing.Point(159, 30);
            this.btn_path.Name = "btn_path";
            this.btn_path.Size = new System.Drawing.Size(75, 23);
            this.btn_path.TabIndex = 2;
            this.btn_path.Text = "Select...";
            this.btn_path.UseVisualStyleBackColor = true;
            this.btn_path.Click += new System.EventHandler(this.btn_path_Click);
            // 
            // btn_patch
            // 
            this.btn_patch.Location = new System.Drawing.Point(70, 75);
            this.btn_patch.Name = "btn_patch";
            this.btn_patch.Size = new System.Drawing.Size(100, 27);
            this.btn_patch.TabIndex = 3;
            this.btn_patch.Text = "Patch!";
            this.btn_patch.UseVisualStyleBackColor = true;
            this.btn_patch.Click += new System.EventHandler(this.btn_patch_Click);
            // 
            // label_about
            // 
            this.label_about.AutoSize = true;
            this.label_about.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label_about.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_about.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label_about.Location = new System.Drawing.Point(22, 123);
            this.label_about.Name = "label_about";
            this.label_about.Size = new System.Drawing.Size(208, 13);
            this.label_about.TabIndex = 4;
            this.label_about.Text = "Prince of Persia: Warrior Within beta patch";
            this.label_about.Click += new System.EventHandler(this.label_about_Click);
            // 
            // richTextBox_log
            // 
            this.richTextBox_log.Location = new System.Drawing.Point(13, 201);
            this.richTextBox_log.Name = "richTextBox_log";
            this.richTextBox_log.Size = new System.Drawing.Size(222, 96);
            this.richTextBox_log.TabIndex = 5;
            this.richTextBox_log.Text = "";
            // 
            // label_log
            // 
            this.label_log.AutoSize = true;
            this.label_log.Location = new System.Drawing.Point(13, 182);
            this.label_log.Name = "label_log";
            this.label_log.Size = new System.Drawing.Size(59, 13);
            this.label_log.TabIndex = 6;
            this.label_log.Text = "Debug log:";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(254, 146);
            this.Controls.Add(this.label_log);
            this.Controls.Add(this.richTextBox_log);
            this.Controls.Add(this.label_about);
            this.Controls.Add(this.btn_patch);
            this.Controls.Add(this.btn_path);
            this.Controls.Add(this.textBox_path);
            this.Controls.Add(this.label_path);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Main";
            this.Text = "Patch tool";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_path;
        private System.Windows.Forms.TextBox textBox_path;
        private System.Windows.Forms.Button btn_path;
        private System.Windows.Forms.Button btn_patch;
        private System.Windows.Forms.Label label_about;
        private System.Windows.Forms.RichTextBox richTextBox_log;
        private System.Windows.Forms.Label label_log;
    }
}

