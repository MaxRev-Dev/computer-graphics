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
            this.playground = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.depthLabel = new System.Windows.Forms.Label();
            this.fern_A = new System.Windows.Forms.TrackBar();
            this.fern_K0 = new System.Windows.Forms.TrackBar();
            this.fern_K1 = new System.Windows.Forms.TrackBar();
            this.fern_B = new System.Windows.Forms.TrackBar();
            this.fern_Lmin = new System.Windows.Forms.TrackBar();
            this.fernALabel = new System.Windows.Forms.Label();
            this.fernBLabel = new System.Windows.Forms.Label();
            this.fernK0Label = new System.Windows.Forms.Label();
            this.fernK1Label = new System.Windows.Forms.Label();
            this.fernLminLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.playground)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fern_A)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fern_K0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fern_K1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fern_B)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fern_Lmin)).BeginInit();
            this.SuspendLayout();
            // 
            // playground
            // 
            this.playground.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.playground.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.playground.Location = new System.Drawing.Point(0, 0);
            this.playground.Name = "playground";
            this.playground.Size = new System.Drawing.Size(800, 450);
            this.playground.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.playground.TabIndex = 2;
            this.playground.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(570, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 3;
            // 
            // depthLabel
            // 
            this.depthLabel.AutoSize = true;
            this.depthLabel.Location = new System.Drawing.Point(481, 9);
            this.depthLabel.Name = "depthLabel";
            this.depthLabel.Size = new System.Drawing.Size(0, 13);
            this.depthLabel.TabIndex = 3;
            // 
            // fern_A
            // 
            this.fern_A.Location = new System.Drawing.Point(26, 380);
            this.fern_A.Name = "fern_A";
            this.fern_A.Size = new System.Drawing.Size(116, 45);
            this.fern_A.TabIndex = 4;
            this.fern_A.Scroll += new System.EventHandler(this.fern_A_Scroll);
            // 
            // fern_K0
            // 
            this.fern_K0.Location = new System.Drawing.Point(270, 380);
            this.fern_K0.Name = "fern_K0";
            this.fern_K0.Size = new System.Drawing.Size(108, 45);
            this.fern_K0.TabIndex = 5;
            this.fern_K0.Scroll += new System.EventHandler(this.fern_K0_Scroll);
            // 
            // fern_K1
            // 
            this.fern_K1.Location = new System.Drawing.Point(384, 380);
            this.fern_K1.Name = "fern_K1";
            this.fern_K1.Size = new System.Drawing.Size(108, 45);
            this.fern_K1.TabIndex = 5;
            this.fern_K1.Scroll += new System.EventHandler(this.fern_K1_Scroll);
            // 
            // fern_B
            // 
            this.fern_B.Location = new System.Drawing.Point(148, 380);
            this.fern_B.Name = "fern_B";
            this.fern_B.Size = new System.Drawing.Size(116, 45);
            this.fern_B.TabIndex = 4;
            this.fern_B.Scroll += new System.EventHandler(this.fern_B_Scroll);
            // 
            // fern_Lmin
            // 
            this.fern_Lmin.Location = new System.Drawing.Point(498, 380);
            this.fern_Lmin.Name = "fern_Lmin";
            this.fern_Lmin.Size = new System.Drawing.Size(116, 45);
            this.fern_Lmin.TabIndex = 4;
            this.fern_Lmin.Scroll += new System.EventHandler(this.fern_Lmin_Scroll);
            // 
            // fernALabel
            // 
            this.fernALabel.AutoSize = true;
            this.fernALabel.Location = new System.Drawing.Point(23, 412);
            this.fernALabel.Name = "fernALabel";
            this.fernALabel.Size = new System.Drawing.Size(0, 13);
            this.fernALabel.TabIndex = 6;
            // 
            // fernBLabel
            // 
            this.fernBLabel.AutoSize = true;
            this.fernBLabel.Location = new System.Drawing.Point(148, 412);
            this.fernBLabel.Name = "fernBLabel";
            this.fernBLabel.Size = new System.Drawing.Size(0, 13);
            this.fernBLabel.TabIndex = 6;
            // 
            // fernK0Label
            // 
            this.fernK0Label.AutoSize = true;
            this.fernK0Label.Location = new System.Drawing.Point(270, 412);
            this.fernK0Label.Name = "fernK0Label";
            this.fernK0Label.Size = new System.Drawing.Size(0, 13);
            this.fernK0Label.TabIndex = 6;
            // 
            // fernK1Label
            // 
            this.fernK1Label.AutoSize = true;
            this.fernK1Label.Location = new System.Drawing.Point(384, 412);
            this.fernK1Label.Name = "fernK1Label";
            this.fernK1Label.Size = new System.Drawing.Size(0, 13);
            this.fernK1Label.TabIndex = 6;
            // 
            // fernLminLabel
            // 
            this.fernLminLabel.AutoSize = true;
            this.fernLminLabel.Location = new System.Drawing.Point(498, 412);
            this.fernLminLabel.Name = "fernLminLabel";
            this.fernLminLabel.Size = new System.Drawing.Size(0, 13);
            this.fernLminLabel.TabIndex = 6;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.fernLminLabel);
            this.Controls.Add(this.fernK1Label);
            this.Controls.Add(this.fernK0Label);
            this.Controls.Add(this.fernBLabel);
            this.Controls.Add(this.fernALabel);
            this.Controls.Add(this.fern_K1);
            this.Controls.Add(this.fern_K0);
            this.Controls.Add(this.fern_Lmin);
            this.Controls.Add(this.fern_B);
            this.Controls.Add(this.fern_A);
            this.Controls.Add(this.depthLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.playground);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "Main";
            this.Text = "Tetrahedron Playground";
            this.SizeChanged += new System.EventHandler(this.Main_SizeChanged);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Main_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.playground)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fern_A)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fern_K0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fern_K1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fern_B)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fern_Lmin)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PictureBox playground;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label depthLabel;

        private TrackBar fern_A;
        private TrackBar fern_K0;
        private TrackBar fern_K1;
        private TrackBar fern_B;
        private TrackBar fern_Lmin;
        private Label fernALabel;
        private Label fernBLabel;
        private Label fernK0Label;
        private Label fernK1Label;
        private Label fernLminLabel;
    }
}

