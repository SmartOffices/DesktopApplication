namespace EyeDetector
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
            this.components = new System.ComponentModel.Container();
            this.imageBoxCapturedFrame = new Emgu.CV.UI.ImageBox();
            ((System.ComponentModel.ISupportInitialize)(this.imageBoxCapturedFrame)).BeginInit();
            this.SuspendLayout();
            // 
            // imageBoxCapturedFrame
            // 
            this.imageBoxCapturedFrame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageBoxCapturedFrame.Location = new System.Drawing.Point(0, 0);
            this.imageBoxCapturedFrame.Name = "imageBoxCapturedFrame";
            this.imageBoxCapturedFrame.Size = new System.Drawing.Size(392, 316);
            this.imageBoxCapturedFrame.TabIndex = 2;
            this.imageBoxCapturedFrame.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 316);
            this.Controls.Add(this.imageBoxCapturedFrame);
            this.Name = "Form1";
            this.Text = "EmguCV  Eye  Recognition    ------->   by Luca Del Tongo";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.imageBoxCapturedFrame)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Emgu.CV.UI.ImageBox imageBoxCapturedFrame;
    }
}

