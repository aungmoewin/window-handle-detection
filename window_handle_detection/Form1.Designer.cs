namespace window_handle_detection
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
            if (disposing)
            {
                CleanupHook();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            btnDetectDialogs = new Button();
            btnCloseDialogs = new Button();
            lblWindowHandle = new Label();
            SuspendLayout();
            // 
            // btnDetectDialogs
            // 
            btnDetectDialogs.Location = new Point(350, 200);
            btnDetectDialogs.Name = "btnDetectDialogs";
            btnDetectDialogs.Size = new Size(100, 50);
            btnDetectDialogs.TabIndex = 0;
            btnDetectDialogs.Text = "Detect Dialogs";
            btnDetectDialogs.UseVisualStyleBackColor = true;
            btnDetectDialogs.Click += BtnDetectDialogs_Click;
            // 
            // btnCloseDialogs
            // 
            btnCloseDialogs.Location = new Point(470, 200);
            btnCloseDialogs.Name = "btnCloseDialogs";
            btnCloseDialogs.Size = new Size(100, 50);
            btnCloseDialogs.TabIndex = 2;
            btnCloseDialogs.Text = "Close Dialogs";
            btnCloseDialogs.UseVisualStyleBackColor = true;
            btnCloseDialogs.Click += BtnCloseDialogs_Click;
            // 
            // lblWindowHandle
            // 
            lblWindowHandle.AutoSize = true;
            lblWindowHandle.Location = new Point(50, 50);
            lblWindowHandle.Name = "lblWindowHandle";
            lblWindowHandle.Size = new Size(150, 15);
            lblWindowHandle.TabIndex = 1;
            lblWindowHandle.Text = "Window Handle: None";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F,15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800,450);
            Controls.Add(lblWindowHandle);
            Controls.Add(btnCloseDialogs);
            Controls.Add(btnDetectDialogs);
            Name = "Form1";
            Text = "Dialog Detection";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button btnDetectDialogs;
        private Button btnCloseDialogs;
        private Label lblWindowHandle;
    }
}
