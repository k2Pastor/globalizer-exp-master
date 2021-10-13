namespace Bridge
{
    partial class GraphicsGallery
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
            this.graphicPictureBox = new System.Windows.Forms.PictureBox();
            this.lineGraphicRadioButton = new System.Windows.Forms.RadioButton();
            this.columnGraphicRadioButton = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.graphicPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // graphicPictureBox
            // 
            this.graphicPictureBox.Location = new System.Drawing.Point(24, 86);
            this.graphicPictureBox.Name = "graphicPictureBox";
            this.graphicPictureBox.Size = new System.Drawing.Size(500, 400);
            this.graphicPictureBox.TabIndex = 0;
            this.graphicPictureBox.TabStop = false;
            // 
            // lineGraphicRadioButton
            // 
            this.lineGraphicRadioButton.AutoSize = true;
            this.lineGraphicRadioButton.Location = new System.Drawing.Point(698, 230);
            this.lineGraphicRadioButton.Name = "lineGraphicRadioButton";
            this.lineGraphicRadioButton.Size = new System.Drawing.Size(110, 21);
            this.lineGraphicRadioButton.TabIndex = 1;
            this.lineGraphicRadioButton.TabStop = true;
            this.lineGraphicRadioButton.Text = "Line Graphic";
            this.lineGraphicRadioButton.UseVisualStyleBackColor = true;
            this.lineGraphicRadioButton.CheckedChanged += new System.EventHandler(this.lineGraphicRadioButton_CheckedChanged);
            // 
            // columnGraphicRadioButton
            // 
            this.columnGraphicRadioButton.AutoSize = true;
            this.columnGraphicRadioButton.Location = new System.Drawing.Point(698, 271);
            this.columnGraphicRadioButton.Name = "columnGraphicRadioButton";
            this.columnGraphicRadioButton.Size = new System.Drawing.Size(194, 21);
            this.columnGraphicRadioButton.TabIndex = 2;
            this.columnGraphicRadioButton.TabStop = true;
            this.columnGraphicRadioButton.Text = "Column Clustered Graphic";
            this.columnGraphicRadioButton.UseVisualStyleBackColor = true;
            this.columnGraphicRadioButton.CheckedChanged += new System.EventHandler(this.columnGraphicRadioButton_CheckedChanged);
            // 
            // GraphicsGallery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.columnGraphicRadioButton);
            this.Controls.Add(this.lineGraphicRadioButton);
            this.Controls.Add(this.graphicPictureBox);
            this.Name = "GraphicsGallery";
            this.Text = "Graphic Gallery";
            ((System.ComponentModel.ISupportInitialize)(this.graphicPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox graphicPictureBox;
        private System.Windows.Forms.RadioButton lineGraphicRadioButton;
        private System.Windows.Forms.RadioButton columnGraphicRadioButton;
    }
}