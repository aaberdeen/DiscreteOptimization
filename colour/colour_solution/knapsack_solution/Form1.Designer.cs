namespace knapsack_solution
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
            this.Solve_button = new System.Windows.Forms.Button();
            this.IP_textBox = new System.Windows.Forms.RichTextBox();
            this.OP_textBox = new System.Windows.Forms.RichTextBox();
            this.DebugTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // Solve_button
            // 
            this.Solve_button.Location = new System.Drawing.Point(549, 12);
            this.Solve_button.Name = "Solve_button";
            this.Solve_button.Size = new System.Drawing.Size(65, 427);
            this.Solve_button.TabIndex = 2;
            this.Solve_button.Text = "Solve";
            this.Solve_button.UseVisualStyleBackColor = true;
            this.Solve_button.Click += new System.EventHandler(this.Solve_button_Click);
            // 
            // IP_textBox
            // 
            this.IP_textBox.EnableAutoDragDrop = true;
            this.IP_textBox.Location = new System.Drawing.Point(12, 12);
            this.IP_textBox.Name = "IP_textBox";
            this.IP_textBox.Size = new System.Drawing.Size(531, 681);
            this.IP_textBox.TabIndex = 3;
            this.IP_textBox.Text = "";
            // 
            // OP_textBox
            // 
            this.OP_textBox.Location = new System.Drawing.Point(620, 12);
            this.OP_textBox.Name = "OP_textBox";
            this.OP_textBox.Size = new System.Drawing.Size(456, 427);
            this.OP_textBox.TabIndex = 4;
            this.OP_textBox.Text = "";
            // 
            // DebugTextBox
            // 
            this.DebugTextBox.Location = new System.Drawing.Point(549, 445);
            this.DebugTextBox.Name = "DebugTextBox";
            this.DebugTextBox.Size = new System.Drawing.Size(527, 248);
            this.DebugTextBox.TabIndex = 5;
            this.DebugTextBox.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1095, 715);
            this.Controls.Add(this.DebugTextBox);
            this.Controls.Add(this.OP_textBox);
            this.Controls.Add(this.IP_textBox);
            this.Controls.Add(this.Solve_button);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Solve_button;
        private System.Windows.Forms.RichTextBox IP_textBox;
        private System.Windows.Forms.RichTextBox OP_textBox;
        private System.Windows.Forms.RichTextBox DebugTextBox;
    }
}

