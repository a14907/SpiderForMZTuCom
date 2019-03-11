using SpiderForMZTuCom;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpiderForUmei
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private MySpiderHelper mySp = new MySpiderHelper();

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            tbProgress.Text = "0";
            await DownLoadTuJi();
        }

        private async Task DownLoadTuJi()
        {
            string url = tbUrl.Text.Trim();
            bool isGL = chbIsGuiLv.Checked;
            string maxCount = tbMaxCount.Text;
            string numHolder = tbNumHolder.Text;

            //获取保存位置
            FolderBrowserDialog f = new FolderBrowserDialog();
            string folder = "";
            if (f.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            folder = f.SelectedPath;

            //图集名称
            var imgsName = await mySp.GetTuJiName(url);

            IEnumerable<string> imgs = null;
            if (isGL)
            {
                imgs = await mySp._3GetAllImgUrlInTuJiInGuiLv(url, int.Parse(maxCount), int.Parse(numHolder));
            }
            else
            {
                imgs = await mySp._3GetAllImgUrlInTuJi(url);
            }
            WebClient c = new WebClient();
            int index = 0;
            foreach (var imgUrl in imgs)
            {
                var dName = Path.Combine(folder, imgsName);
                if (!Directory.Exists(dName))
                {
                    Directory.CreateDirectory(dName);
                }
                var fileName = Path.Combine(dName, Path.GetFileName(imgUrl));
                try
                {
                    await c.DownloadFileTaskAsync(imgUrl, fileName);
                    tbProgress.Text = (++index).ToString();
                }
                catch (Exception ex)
                {
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }
                    continue;
                }
            }
            c.Dispose();
            MessageBox.Show("下载完成！");
        }
    }
}
