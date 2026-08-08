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
            txt_userquestion = new RichTextBox();
            label1 = new Label();
            label2 = new Label();
            btn_query = new Button();
            txt_question_log = new RichTextBox();
            SuspendLayout();
            // 
            // txt_result
            // 
            txt_result.BackColor = Color.LightYellow;
            txt_result.Location = new Point(156, 118);
            txt_result.Name = "txt_result";
            txt_result.ReadOnly = true;
            txt_result.Size = new Size(911, 370);
            txt_result.TabIndex = 2;
            txt_result.Text = "";
            // 
            // txt_userquestion
            // 
            txt_userquestion.Location = new Point(156, 16);
            txt_userquestion.Name = "txt_userquestion";
            txt_userquestion.Size = new Size(911, 90);
            txt_userquestion.TabIndex = 0;
            txt_userquestion.Text = "";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(12, 19);
            label1.Name = "label1";
            label1.Size = new Size(138, 20);
            label1.TabIndex = 2;
            label1.Text = "Ask Your Question";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.Location = new Point(12, 131);
            label2.Name = "label2";
            label2.Size = new Size(62, 20);
            label2.TabIndex = 3;
            label2.Text = "Answer";
            // 
            // btn_query
            // 
            btn_query.Location = new Point(1082, 19);
            btn_query.Name = "btn_query";
            btn_query.Size = new Size(144, 87);
            btn_query.TabIndex = 1;
            btn_query.Text = "Fetch";
            btn_query.UseVisualStyleBackColor = true;
            btn_query.Click += btn_query_Click;
            // 
            // txt_question_log
            // 
            txt_question_log.BackColor = Color.LightYellow;
            txt_question_log.Font = new Font("Segoe UI", 8F);
            txt_question_log.Location = new Point(1243, 12);
            txt_question_log.Name = "txt_question_log";
            txt_question_log.ReadOnly = true;
            txt_question_log.Size = new Size(453, 476);
            txt_question_log.TabIndex = 5;
            txt_question_log.Text = "";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1708, 501);
            Controls.Add(txt_question_log);
            Controls.Add(btn_query);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txt_userquestion);
            Controls.Add(txt_result);
            Name = "Form1";
            Text = "Asset Agent";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox txt_result;
        private RichTextBox txt_userquestion;
        private Label label1;
        private Label label2;
        private Button btn_query;
        private RichTextBox txt_question_log;
    }
}
