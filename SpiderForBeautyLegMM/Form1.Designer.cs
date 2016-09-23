namespace SpiderForBeautyLegMM
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.chbIsGuiLv = new System.Windows.Forms.CheckBox();
            this.tbProgress = new System.Windows.Forms.TextBox();
            this.tbNumHolder = new System.Windows.Forms.TextBox();
            this.tbMaxCount = new System.Windows.Forms.TextBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbUrl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbUrl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chbIsGuiLv
            // 
            this.chbIsGuiLv.AutoSize = true;
            this.chbIsGuiLv.Checked = true;
            this.chbIsGuiLv.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbIsGuiLv.Location = new System.Drawing.Point(81, 68);
            this.chbIsGuiLv.Name = "chbIsGuiLv";
            this.chbIsGuiLv.Size = new System.Drawing.Size(72, 16);
            this.chbIsGuiLv.TabIndex = 14;
            this.chbIsGuiLv.Text = "是否规律";
            this.chbIsGuiLv.UseVisualStyleBackColor = true;
            // 
            // tbProgress
            // 
            this.tbProgress.Location = new System.Drawing.Point(281, 66);
            this.tbProgress.Name = "tbProgress";
            this.tbProgress.Size = new System.Drawing.Size(100, 21);
            this.tbProgress.TabIndex = 11;
            this.tbProgress.Text = "0";
            // 
            // tbNumHolder
            // 
            this.tbNumHolder.Location = new System.Drawing.Point(281, 99);
            this.tbNumHolder.Name = "tbNumHolder";
            this.tbNumHolder.Size = new System.Drawing.Size(100, 21);
            this.tbNumHolder.TabIndex = 12;
            this.tbNumHolder.Text = "4";
            // 
            // tbMaxCount
            // 
            this.tbMaxCount.Location = new System.Drawing.Point(81, 99);
            this.tbMaxCount.Name = "tbMaxCount";
            this.tbMaxCount.Size = new System.Drawing.Size(100, 21);
            this.tbMaxCount.TabIndex = 13;
            this.tbMaxCount.Text = "100";
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(390, 29);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(75, 23);
            this.btnDownload.TabIndex = 10;
            this.btnDownload.Text = "下载";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(223, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "进度";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(223, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "位数";
            // 
            // tbUrl
            // 
            this.tbUrl.Location = new System.Drawing.Point(81, 31);
            this.tbUrl.Name = "tbUrl";
            this.tbUrl.Size = new System.Drawing.Size(300, 21);
            this.tbUrl.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 102);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "最大数";
            // 
            // lbUrl
            // 
            this.lbUrl.AutoSize = true;
            this.lbUrl.Location = new System.Drawing.Point(35, 34);
            this.lbUrl.Name = "lbUrl";
            this.lbUrl.Size = new System.Drawing.Size(29, 12);
            this.lbUrl.TabIndex = 8;
            this.lbUrl.Text = "地址";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 166);
            this.Controls.Add(this.chbIsGuiLv);
            this.Controls.Add(this.tbProgress);
            this.Controls.Add(this.tbNumHolder);
            this.Controls.Add(this.tbMaxCount);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbUrl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbUrl);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chbIsGuiLv;
        private System.Windows.Forms.TextBox tbProgress;
        private System.Windows.Forms.TextBox tbNumHolder;
        private System.Windows.Forms.TextBox tbMaxCount;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbUrl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbUrl;
    }
}

