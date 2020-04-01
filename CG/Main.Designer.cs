using System.Windows.Forms;

namespace Playground
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));

            playground = new PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.depthLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(playground)).BeginInit();
            this.SuspendLayout();
            // 
            // playground
            // 
            playground.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                                                        | System.Windows.Forms.AnchorStyles.Left)
                                                                       | System.Windows.Forms.AnchorStyles.Right)));
            playground.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            playground.Location = new System.Drawing.Point(0, 0);
            playground.Name = "playground";
            playground.Size = new System.Drawing.Size(800, 450);
            playground.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            playground.TabIndex = 2;
            playground.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(570, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "";
            // 
            // label2
            // 
            this.depthLabel.AutoSize = true;
            this.depthLabel.Location = new System.Drawing.Point(481, 9);
            this.depthLabel.Name = "depthLabel";
            this.depthLabel.Size = new System.Drawing.Size(35, 13);
            this.depthLabel.TabIndex = 3;
            this.depthLabel.Text = "";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.depthLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(playground);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.Text = "Tetrahedron Playground";
            this.SizeChanged += new System.EventHandler(this.Main_SizeChanged);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Main_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(playground)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PictureBox playground;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label depthLabel;
    }
}

