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
            this.fernControls = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.playground)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fern_A)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fern_K0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fern_K1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fern_B)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fern_Lmin)).BeginInit();
            this.fernControls.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
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
            this.playground.Size = new System.Drawing.Size(883, 506);
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
            this.depthLabel.Location = new System.Drawing.Point(0, 0);
            this.depthLabel.Name = "depthLabel";
            this.depthLabel.Size = new System.Drawing.Size(0, 13);
            this.depthLabel.TabIndex = 3;
            // 
            // fern_A
            // 
            this.fern_A.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fern_A.Location = new System.Drawing.Point(121, 12);
            this.fern_A.Name = "fern_A";
            this.fern_A.Size = new System.Drawing.Size(113, 45);
            this.fern_A.TabIndex = 4;
            this.fern_A.Visible = false;
            this.fern_A.Scroll += new System.EventHandler(this.fern_A_Scroll);
            // 
            // fern_K0
            // 
            this.fern_K0.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fern_K0.Location = new System.Drawing.Point(478, 12);
            this.fern_K0.Name = "fern_K0";
            this.fern_K0.Size = new System.Drawing.Size(105, 45);
            this.fern_K0.TabIndex = 5;
            this.fern_K0.Visible = false;
            this.fern_K0.Scroll += new System.EventHandler(this.fern_K0_Scroll);
            // 
            // fern_K1
            // 
            this.fern_K1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fern_K1.Location = new System.Drawing.Point(589, 12);
            this.fern_K1.Name = "fern_K1";
            this.fern_K1.Size = new System.Drawing.Size(105, 45);
            this.fern_K1.TabIndex = 5;
            this.fern_K1.Visible = false;
            this.fern_K1.Scroll += new System.EventHandler(this.fern_K1_Scroll);
            // 
            // fern_B
            // 
            this.fern_B.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fern_B.Location = new System.Drawing.Point(240, 12);
            this.fern_B.Name = "fern_B";
            this.fern_B.Size = new System.Drawing.Size(113, 45);
            this.fern_B.TabIndex = 4;
            this.fern_B.Visible = false;
            this.fern_B.Scroll += new System.EventHandler(this.fern_B_Scroll);
            // 
            // fern_Lmin
            // 
            this.fern_Lmin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fern_Lmin.Location = new System.Drawing.Point(359, 12);
            this.fern_Lmin.Name = "fern_Lmin";
            this.fern_Lmin.Size = new System.Drawing.Size(113, 45);
            this.fern_Lmin.TabIndex = 4;
            this.fern_Lmin.Visible = false;
            this.fern_Lmin.Scroll += new System.EventHandler(this.fern_Lmin_Scroll);
            // 
            // fernALabel
            // 
            this.fernALabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fernALabel.AutoSize = true;
            this.fernALabel.Location = new System.Drawing.Point(129, 61);
            this.fernALabel.Name = "fernALabel";
            this.fernALabel.Size = new System.Drawing.Size(0, 13);
            this.fernALabel.TabIndex = 6;
            // 
            // fernBLabel
            // 
            this.fernBLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fernBLabel.AutoSize = true;
            this.fernBLabel.Location = new System.Drawing.Point(248, 60);
            this.fernBLabel.Name = "fernBLabel";
            this.fernBLabel.Size = new System.Drawing.Size(0, 13);
            this.fernBLabel.TabIndex = 6;
            // 
            // fernK0Label
            // 
            this.fernK0Label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fernK0Label.AutoSize = true;
            this.fernK0Label.Location = new System.Drawing.Point(485, 60);
            this.fernK0Label.Name = "fernK0Label";
            this.fernK0Label.Size = new System.Drawing.Size(0, 13);
            this.fernK0Label.TabIndex = 6;
            // 
            // fernK1Label
            // 
            this.fernK1Label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fernK1Label.AutoSize = true;
            this.fernK1Label.Location = new System.Drawing.Point(598, 60);
            this.fernK1Label.Name = "fernK1Label";
            this.fernK1Label.Size = new System.Drawing.Size(0, 13);
            this.fernK1Label.TabIndex = 6;
            // 
            // fernLminLabel
            // 
            this.fernLminLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fernLminLabel.AutoSize = true;
            this.fernLminLabel.Location = new System.Drawing.Point(366, 60);
            this.fernLminLabel.Name = "fernLminLabel";
            this.fernLminLabel.Size = new System.Drawing.Size(0, 13);
            this.fernLminLabel.TabIndex = 6;
            // 
            // fernControls
            // 
            this.fernControls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fernControls.Controls.Add(this.fern_Lmin);
            this.fernControls.Controls.Add(this.fernLminLabel);
            this.fernControls.Controls.Add(this.fern_K0);
            this.fernControls.Controls.Add(this.fernK0Label);
            this.fernControls.Controls.Add(this.fern_K1);
            this.fernControls.Controls.Add(this.fernK1Label);
            this.fernControls.Controls.Add(this.fern_A);
            this.fernControls.Controls.Add(this.fernALabel);
            this.fernControls.Controls.Add(this.fern_B);
            this.fernControls.Controls.Add(this.fernBLabel);
            this.fernControls.Location = new System.Drawing.Point(6, 6);
            this.fernControls.Name = "fernControls";
            this.fernControls.Size = new System.Drawing.Size(838, 100);
            this.fernControls.TabIndex = 7;
            this.fernControls.TabStop = false;
            this.fernControls.Text = "Fern Controls";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 512);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(858, 138);
            this.tabControl1.TabIndex = 8;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.fernControls);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(850, 112);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(192, 74);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 662);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.playground);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.depthLabel);
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
            this.fernControls.ResumeLayout(false);
            this.fernControls.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
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
        private GroupBox fernControls;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
    }
}

