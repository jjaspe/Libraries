namespace Canvas_Window_Template
{
    partial class Navigator
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnUp = new System.Windows.Forms.Button();
            this.btnLeft = new System.Windows.Forms.Button();
            this.btnRight = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnZoomOut = new System.Windows.Forms.Button();
            this.btnZoomIn = new System.Windows.Forms.Button();
            this.btnRotateDown = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.btnRotateUp = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnUp
            // 
            this.btnUp.Location = new System.Drawing.Point(50, 9);
            this.btnUp.Margin = new System.Windows.Forms.Padding(2);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(38, 24);
            this.btnUp.TabIndex = 34;
            this.btnUp.Text = "^";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnLeft
            // 
            this.btnLeft.Location = new System.Drawing.Point(8, 38);
            this.btnLeft.Margin = new System.Windows.Forms.Padding(2);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(38, 24);
            this.btnLeft.TabIndex = 33;
            this.btnLeft.Text = "<";
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // btnRight
            // 
            this.btnRight.Location = new System.Drawing.Point(87, 37);
            this.btnRight.Margin = new System.Windows.Forms.Padding(2);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(38, 24);
            this.btnRight.TabIndex = 32;
            this.btnRight.Text = ">";
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(50, 65);
            this.btnDown.Margin = new System.Windows.Forms.Padding(2);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(38, 24);
            this.btnDown.TabIndex = 31;
            this.btnDown.Text = "v";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.Location = new System.Drawing.Point(8, 66);
            this.btnZoomOut.Margin = new System.Windows.Forms.Padding(2);
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(38, 24);
            this.btnZoomOut.TabIndex = 35;
            this.btnZoomOut.Text = "-";
            this.btnZoomOut.UseVisualStyleBackColor = true;
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.Location = new System.Drawing.Point(8, 8);
            this.btnZoomIn.Margin = new System.Windows.Forms.Padding(2);
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(38, 24);
            this.btnZoomIn.TabIndex = 36;
            this.btnZoomIn.Text = "+";
            this.btnZoomIn.UseVisualStyleBackColor = true;
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // btnRotateDown
            // 
            this.btnRotateDown.Location = new System.Drawing.Point(112, 10);
            this.btnRotateDown.Margin = new System.Windows.Forms.Padding(2);
            this.btnRotateDown.Name = "btnRotateDown";
            this.btnRotateDown.Size = new System.Drawing.Size(31, 23);
            this.btnRotateDown.TabIndex = 38;
            this.btnRotateDown.Text = "vv";
            this.btnRotateDown.UseVisualStyleBackColor = true;
            this.btnRotateDown.Click += new System.EventHandler(this.btnRotateDown_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(147, 67);
            this.button3.Margin = new System.Windows.Forms.Padding(2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(35, 21);
            this.button3.TabIndex = 39;
            this.button3.Text = "<<";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(147, 9);
            this.button4.Margin = new System.Windows.Forms.Padding(2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(31, 21);
            this.button4.TabIndex = 40;
            this.button4.Text = ">>";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // btnRotateUp
            // 
            this.btnRotateUp.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRotateUp.Location = new System.Drawing.Point(112, 68);
            this.btnRotateUp.Margin = new System.Windows.Forms.Padding(2);
            this.btnRotateUp.Name = "btnRotateUp";
            this.btnRotateUp.Size = new System.Drawing.Size(31, 22);
            this.btnRotateUp.TabIndex = 37;
            this.btnRotateUp.Text = "^^";
            this.btnRotateUp.UseVisualStyleBackColor = true;
            this.btnRotateUp.Click += new System.EventHandler(this.btnRotateUp_Click);
            // 
            // Navigator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.btnRotateDown);
            this.Controls.Add(this.btnRotateUp);
            this.Controls.Add(this.btnZoomIn);
            this.Controls.Add(this.btnZoomOut);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.btnLeft);
            this.Controls.Add(this.btnRight);
            this.Controls.Add(this.btnDown);
            this.Name = "Navigator";
            this.Size = new System.Drawing.Size(190, 95);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Button btnUp;
        protected System.Windows.Forms.Button btnLeft;
        protected System.Windows.Forms.Button btnRight;
        protected System.Windows.Forms.Button btnDown;
        protected System.Windows.Forms.Button btnZoomOut;
        protected System.Windows.Forms.Button btnZoomIn;
        protected System.Windows.Forms.Button btnRotateDown;
        protected System.Windows.Forms.Button button3;
        protected System.Windows.Forms.Button button4;
        protected internal System.Windows.Forms.Button btnRotateUp;
    }
}
