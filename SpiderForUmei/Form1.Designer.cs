namespace SpiderForUmei
{
    partial class FormMain
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
            this.lbUrl = new System.Windows.Forms.Label();
            this.tbUrl = new System.Windows.Forms.TextBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.tbMaxCount = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chbIsGuiLv = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbNumHolder = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbProgress = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lbUrl
            // 
            this.lbUrl.AutoSize = true;
            this.lbUrl.Location = new System.Drawing.Point(36, 34);
            this.lbUrl.Name = "lbUrl";
            this.lbUrl.Size = new System.Drawing.Size(29, 12);
            this.lbUrl.TabIndex = 0;
            this.lbUrl.Text = "地址";
            // 
            // tbUrl
            // 
            this.tbUrl.Location = new System.Drawing.Point(82, 31);
            this.tbUrl.Name = "tbUrl";
            this.tbUrl.Size = new System.Drawing.Size(300, 21);
            this.tbUrl.TabIndex = 1;
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(391, 29);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(75, 23);
            this.btnDownload.TabIndex = 2;
            this.btnDownload.Text = "下载";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // tbMaxCount
            // 
            this.tbMaxCount.Location = new System.Drawing.Point(82, 99);
            this.tbMaxCount.Name = "tbMaxCount";
            this.tbMaxCount.Size = new System.Drawing.Size(100, 21);
            this.tbMaxCount.TabIndex = 3;
            this.tbMaxCount.Text = "100";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 102);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "最大数";
            // 
            // chbIsGuiLv
            // 
            this.chbIsGuiLv.AutoSize = true;
            this.chbIsGuiLv.Location = new System.Drawing.Point(82, 68);
            this.chbIsGuiLv.Name = "chbIsGuiLv";
            this.chbIsGuiLv.Size = new System.Drawing.Size(72, 16);
            this.chbIsGuiLv.TabIndex = 4;
            this.chbIsGuiLv.Text = "是否规律";
            this.chbIsGuiLv.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(224, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "位数";
            // 
            // tbNumHolder
            // 
            this.tbNumHolder.Location = new System.Drawing.Point(282, 99);
            this.tbNumHolder.Name = "tbNumHolder";
            this.tbNumHolder.Size = new System.Drawing.Size(100, 21);
            this.tbNumHolder.TabIndex = 3;
            this.tbNumHolder.Text = "4";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(224, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "进度";
            // 
            // tbProgress
            // 
            this.tbProgress.Location = new System.Drawing.Point(282, 66);
            this.tbProgress.Name = "tbProgress";
            this.tbProgress.Size = new System.Drawing.Size(100, 21);
            this.tbProgress.TabIndex = 3;
            this.tbProgress.Text = "0";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 172);
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
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "优美爬取软件";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbUrl;
        private System.Windows.Forms.TextBox tbUrl;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.TextBox tbMaxCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chbIsGuiLv;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbNumHolder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbProgress;
    }
}

