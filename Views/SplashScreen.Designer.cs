namespace SalesInventorySystem_WAM1
{
    partial class SplashScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashScreen));
            this.pnlLoading = new System.Windows.Forms.Panel();
            this.pnlFullLoading = new System.Windows.Forms.Panel();
            this.tmrLoad = new System.Windows.Forms.Timer(this.components);
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.lblSales = new System.Windows.Forms.Label();
            this.lblInventory = new System.Windows.Forms.Label();
            this.lblSystem = new System.Windows.Forms.Label();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlFullLoading.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlLoading
            // 
            this.pnlLoading.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.pnlLoading.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            this.pnlLoading.Location = new System.Drawing.Point(0, 0);
            this.pnlLoading.Name = "pnlLoading";
            this.pnlLoading.Size = new System.Drawing.Size(51, 18);
            this.pnlLoading.TabIndex = 0;
            // 
            // pnlFullLoading
            // 
            this.pnlFullLoading.Controls.Add(this.pnlLoading);
            this.pnlFullLoading.Location = new System.Drawing.Point(-1, 326);
            this.pnlFullLoading.Name = "pnlFullLoading";
            this.pnlFullLoading.Size = new System.Drawing.Size(601, 18);
            this.pnlFullLoading.TabIndex = 1;
            // 
            // tmrLoad
            // 
            this.tmrLoad.Enabled = true;
            this.tmrLoad.Interval = 20;
            this.tmrLoad.Tick += new System.EventHandler(this.tmrLoad_Tick);
            // 
            // pbLogo
            // 
            this.pbLogo.Image = global::SalesInventorySystem_WAM1.Properties.Resources._10827455;
            this.pbLogo.Location = new System.Drawing.Point(169, 99);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(122, 115);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbLogo.TabIndex = 2;
            this.pbLogo.TabStop = false;
            // 
            // lblSales
            // 
            this.lblSales.AutoSize = true;
            this.lblSales.Font = new System.Drawing.Font("Nirmala UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSales.Location = new System.Drawing.Point(297, 99);
            this.lblSales.Name = "lblSales";
            this.lblSales.Size = new System.Drawing.Size(82, 37);
            this.lblSales.TabIndex = 3;
            this.lblSales.Text = "Sales";
            // 
            // lblInventory
            // 
            this.lblInventory.AutoSize = true;
            this.lblInventory.Font = new System.Drawing.Font("Nirmala UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInventory.Location = new System.Drawing.Point(297, 136);
            this.lblInventory.Name = "lblInventory";
            this.lblInventory.Size = new System.Drawing.Size(143, 37);
            this.lblInventory.TabIndex = 4;
            this.lblInventory.Text = "Inventory";
            // 
            // lblSystem
            // 
            this.lblSystem.AutoSize = true;
            this.lblSystem.Font = new System.Drawing.Font("Nirmala UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSystem.Location = new System.Drawing.Point(297, 177);
            this.lblSystem.Name = "lblSystem";
            this.lblSystem.Size = new System.Drawing.Size(109, 37);
            this.lblSystem.TabIndex = 5;
            this.lblSystem.Text = "System";
            // 
            // lblCopyright
            // 
            this.lblCopyright.AutoSize = true;
            this.lblCopyright.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCopyright.Location = new System.Drawing.Point(3, 310);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(294, 13);
            this.lblCopyright.TabIndex = 6;
            this.lblCopyright.Text = "© 2024 Christopher Andrei Tayao | Karlo Robert Wagan";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(230, 250);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 7;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SplashScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.ClientSize = new System.Drawing.Size(599, 345);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblCopyright);
            this.Controls.Add(this.lblSystem);
            this.Controls.Add(this.lblInventory);
            this.Controls.Add(this.lblSales);
            this.Controls.Add(this.pbLogo);
            this.Controls.Add(this.pnlFullLoading);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SplashScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SplashScreen";
            this.pnlFullLoading.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlLoading;
        private System.Windows.Forms.Panel pnlFullLoading;
        private System.Windows.Forms.Timer tmrLoad;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Label lblSales;
        private System.Windows.Forms.Label lblInventory;
        private System.Windows.Forms.Label lblSystem;
        public System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.Label label1;
    }
}