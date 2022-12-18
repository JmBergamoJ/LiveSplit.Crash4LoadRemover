
namespace LiveSplit.Crash4LoadRemover.Controls
{
    partial class Crash4LoadRemoverSettings
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Crash4LoadRemoverSettings));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.gbAutoSplitter = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.framesToWaitForSwirl = new System.Windows.Forms.NumericUpDown();
            this.chkAutoSplitterDisableOnSkip = new System.Windows.Forms.CheckBox();
            this.lblVersion = new System.Windows.Forms.Label();
            this.chkSaveDetectionLog = new System.Windows.Forms.CheckBox();
            this.enableAutoSplitterChk = new System.Windows.Forms.CheckBox();
            this.autoSplitCategoryLbl = new System.Windows.Forms.Label();
            this.gbSplits = new System.Windows.Forms.GroupBox();
            this.panelSplits = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.gbAutoSplitter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.framesToWaitForSwirl)).BeginInit();
            this.gbSplits.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(454, 500);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.gbAutoSplitter);
            this.tabPage2.Controls.Add(this.gbSplits);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(446, 474);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Auto Splitter";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // gbAutoSplitter
            // 
            this.gbAutoSplitter.Controls.Add(this.label1);
            this.gbAutoSplitter.Controls.Add(this.framesToWaitForSwirl);
            this.gbAutoSplitter.Controls.Add(this.chkAutoSplitterDisableOnSkip);
            this.gbAutoSplitter.Controls.Add(this.lblVersion);
            this.gbAutoSplitter.Controls.Add(this.chkSaveDetectionLog);
            this.gbAutoSplitter.Controls.Add(this.enableAutoSplitterChk);
            this.gbAutoSplitter.Controls.Add(this.autoSplitCategoryLbl);
            this.gbAutoSplitter.Location = new System.Drawing.Point(6, 6);
            this.gbAutoSplitter.Name = "gbAutoSplitter";
            this.gbAutoSplitter.Size = new System.Drawing.Size(434, 135);
            this.gbAutoSplitter.TabIndex = 5;
            this.gbAutoSplitter.TabStop = false;
            this.gbAutoSplitter.Text = "Setup";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label1.Location = new System.Drawing.Point(128, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 13);
            this.label1.TabIndex = 47;
            this.label1.Text = "Max Frames to Wait for Swirl";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolTip1.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
            // 
            // framesToWaitForSwirl
            // 
            this.framesToWaitForSwirl.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.framesToWaitForSwirl.Location = new System.Drawing.Point(275, 39);
            this.framesToWaitForSwirl.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.framesToWaitForSwirl.Name = "framesToWaitForSwirl";
            this.framesToWaitForSwirl.Size = new System.Drawing.Size(42, 20);
            this.framesToWaitForSwirl.TabIndex = 46;
            this.framesToWaitForSwirl.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.framesToWaitForSwirl.ValueChanged += new System.EventHandler(this.framesToWaitForSwirl_ValueChanged);
            // 
            // chkAutoSplitterDisableOnSkip
            // 
            this.chkAutoSplitterDisableOnSkip.AutoSize = true;
            this.chkAutoSplitterDisableOnSkip.Location = new System.Drawing.Point(131, 19);
            this.chkAutoSplitterDisableOnSkip.Name = "chkAutoSplitterDisableOnSkip";
            this.chkAutoSplitterDisableOnSkip.Size = new System.Drawing.Size(239, 17);
            this.chkAutoSplitterDisableOnSkip.TabIndex = 45;
            this.chkAutoSplitterDisableOnSkip.Text = "Disable AutoSplitter on Skip until manual Split";
            this.chkAutoSplitterDisableOnSkip.UseVisualStyleBackColor = true;
            this.chkAutoSplitterDisableOnSkip.CheckedChanged += new System.EventHandler(this.chkAutoSplitterDisableOnSkip_CheckedChanged);
            // 
            // lblVersion
            // 
            this.lblVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblVersion.Location = new System.Drawing.Point(6, 106);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(60, 16);
            this.lblVersion.TabIndex = 44;
            this.lblVersion.Text = "Version";
            // 
            // chkSaveDetectionLog
            // 
            this.chkSaveDetectionLog.AutoSize = true;
            this.chkSaveDetectionLog.Checked = true;
            this.chkSaveDetectionLog.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSaveDetectionLog.Location = new System.Drawing.Point(6, 42);
            this.chkSaveDetectionLog.Name = "chkSaveDetectionLog";
            this.chkSaveDetectionLog.Size = new System.Drawing.Size(93, 17);
            this.chkSaveDetectionLog.TabIndex = 43;
            this.chkSaveDetectionLog.Text = "Detection Log";
            this.chkSaveDetectionLog.UseVisualStyleBackColor = true;
            this.chkSaveDetectionLog.Visible = false;
            // 
            // enableAutoSplitterChk
            // 
            this.enableAutoSplitterChk.AutoSize = true;
            this.enableAutoSplitterChk.Location = new System.Drawing.Point(6, 19);
            this.enableAutoSplitterChk.Name = "enableAutoSplitterChk";
            this.enableAutoSplitterChk.Size = new System.Drawing.Size(119, 17);
            this.enableAutoSplitterChk.TabIndex = 0;
            this.enableAutoSplitterChk.Text = "Enable Auto Splitter";
            this.enableAutoSplitterChk.UseVisualStyleBackColor = true;
            this.enableAutoSplitterChk.CheckedChanged += new System.EventHandler(this.enableAutoSplitterChk_CheckedChanged);
            // 
            // autoSplitCategoryLbl
            // 
            this.autoSplitCategoryLbl.AutoSize = true;
            this.autoSplitCategoryLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.autoSplitCategoryLbl.Location = new System.Drawing.Point(6, 76);
            this.autoSplitCategoryLbl.Name = "autoSplitCategoryLbl";
            this.autoSplitCategoryLbl.Size = new System.Drawing.Size(70, 16);
            this.autoSplitCategoryLbl.TabIndex = 42;
            this.autoSplitCategoryLbl.Text = "Category";
            this.autoSplitCategoryLbl.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // gbSplits
            // 
            this.gbSplits.Controls.Add(this.panelSplits);
            this.gbSplits.Controls.Add(this.label3);
            this.gbSplits.Controls.Add(this.label6);
            this.gbSplits.Location = new System.Drawing.Point(6, 147);
            this.gbSplits.Name = "gbSplits";
            this.gbSplits.Size = new System.Drawing.Size(434, 321);
            this.gbSplits.TabIndex = 4;
            this.gbSplits.TabStop = false;
            this.gbSplits.Text = "Splits";
            // 
            // panelSplits
            // 
            this.panelSplits.AutoScroll = true;
            this.panelSplits.Location = new System.Drawing.Point(6, 35);
            this.panelSplits.Name = "panelSplits";
            this.panelSplits.Size = new System.Drawing.Size(422, 280);
            this.panelSplits.TabIndex = 46;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 16);
            this.label3.TabIndex = 45;
            this.label3.Text = "Name:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(240, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(187, 16);
            this.label6.TabIndex = 44;
            this.label6.Text = "Number of Loads per Split";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Crash4LoadRemoverSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "Crash4LoadRemoverSettings";
            this.Size = new System.Drawing.Size(461, 507);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.gbAutoSplitter.ResumeLayout(false);
            this.gbAutoSplitter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.framesToWaitForSwirl)).EndInit();
            this.gbSplits.ResumeLayout(false);
            this.gbSplits.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox enableAutoSplitterChk;
        private System.Windows.Forms.GroupBox gbAutoSplitter;
        private System.Windows.Forms.Label autoSplitCategoryLbl;
        private System.Windows.Forms.GroupBox gbSplits;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panelSplits;
        private System.Windows.Forms.CheckBox chkSaveDetectionLog;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.CheckBox chkAutoSplitterDisableOnSkip;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.NumericUpDown framesToWaitForSwirl;
    }
}
