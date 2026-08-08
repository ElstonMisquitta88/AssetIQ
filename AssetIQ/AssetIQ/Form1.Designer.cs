namespace AssetIQ
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txt_result = new RichTextBox();
            SuspendLayout();
            // 
            // txt_result
            // 
            txt_result.BackColor = SystemColors.ButtonHighlight;
            txt_result.Location = new Point(12, 26);
            txt_result.Name = "txt_result";
            txt_result.ReadOnly = true;
            txt_result.Size = new Size(987, 466);
            txt_result.TabIndex = 0;
            txt_result.Text = "";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1011, 556);
            Controls.Add(txt_result);
            Name = "Form1";
            Text = "Asset Agent";
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox txt_result;
    }
}
