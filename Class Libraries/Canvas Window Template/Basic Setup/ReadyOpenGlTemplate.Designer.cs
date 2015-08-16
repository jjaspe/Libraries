using Canvas_Window_Template.Basic_Drawing_Functions;
namespace Canvas_Window_Template.Basic_Drawing_Functions
{
    public partial class ReadyOpenGlTemplate
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
            this.simpleOpenGlView1 = new Canvas_Window_Template.simpleOpenGlView();
            this.navigator1 = new Canvas_Window_Template.Navigator();
            this.SuspendLayout();
            // 
            // simpleOpenGlView1
            // 
            this.simpleOpenGlView1.AccumBits = ((byte)(0));
            this.simpleOpenGlView1.AutoCheckErrors = false;
            this.simpleOpenGlView1.AutoFinish = false;
            this.simpleOpenGlView1.AutoMakeCurrent = true;
            this.simpleOpenGlView1.AutoSwapBuffers = true;
            this.simpleOpenGlView1.BackColor = System.Drawing.Color.Black;
            this.simpleOpenGlView1.ColorBits = ((byte)(32));
            this.simpleOpenGlView1.DepthBits = ((byte)(16));
            this.simpleOpenGlView1.EyeCustom = new double[] {
        0D,
        0D,
        0D};
            this.simpleOpenGlView1.Location = new System.Drawing.Point(22, 29);
            this.simpleOpenGlView1.Name = "simpleOpenGlView1";
            this.simpleOpenGlView1.PerspectiveEye = new double[] {
        300D,
        0D,
        0.0001D};
            this.simpleOpenGlView1.Size = new System.Drawing.Size(734, 534);
            this.simpleOpenGlView1.StencilBits = ((byte)(0));
            this.simpleOpenGlView1.TabIndex = 15;
            this.simpleOpenGlView1.ViewDistance = 300D;
            this.simpleOpenGlView1.ViewPhi = 0D;
            this.simpleOpenGlView1.ViewTheta = 0D;
            // 
            // navigator1
            // 
            this.navigator1.Location = new System.Drawing.Point(547, 44);
            this.navigator1.MyView = null;
            this.navigator1.MyWindowOwner = null;
            this.navigator1.Name = "navigator1";
            this.navigator1.Orientation = Canvas_Window_Template.Basic_Drawing_Functions.Common.planeOrientation.None;
            this.navigator1.Size = new System.Drawing.Size(190, 95);
            this.navigator1.TabIndex = 16;
            // 
            // ReadyOpenGlTemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1274, 735);
            this.Controls.Add(this.navigator1);
            this.Controls.Add(this.simpleOpenGlView1);
            this.Name = "ReadyOpenGlTemplate";
            this.Text = "ReadyOpenGlTemplate";
            this.ResumeLayout(false);

        }

        #endregion

        protected simpleOpenGlView simpleOpenGlView1;
        private Navigator navigator1;
    }
}