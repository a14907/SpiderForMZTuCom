using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace SpiderForBiQuGe
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private SpiderHelper spid = new SpiderHelper();
        private string url;
        private string folderPath;

        private async void btnStart_Click(object sender, EventArgs e)
        {
            url = tbUrl.Text.Trim();
            if (string.IsNullOrEmpty(url))
            {
                return;
            }
            FolderBrowserDialog f = new FolderBrowserDialog();
            if (f.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            folderPath = f.SelectedPath;
            await Process();
        }

        private async Task Process()
        {
            var p = await spid.GetNovelName(url);
            var filename = Path.Combine(folderPath, p.fname);//文件名
            HtmlAgilityPack.HtmlDocument doc = p.doc;
            var cs = spid._01_GetAllZhangJieUrl(doc);
            using (StreamWriter sw = new StreamWriter(filename, false, Encoding.UTF8))
            {
                int index = 0;
                foreach (var c in cs)
                {
                    try
                    {
                        index++;
                        sw.WriteLine(c.Item1);
                        await spid._02_GetZhangJieContent("http://www.zineworm.com" + c.Item2, sw);
                        //tbProgress.Text = cs.Count() + "/" + index;
                        tbProgress.Invoke(new Action(() =>
                        {
                            tbProgress.Text = cs.Count() + "/" + index;
                        }));
                    }
                    catch (Exception ex)
                    {
                        sw.Write("缺失此章节！");
                        sw.WriteLine();
                        //throw;
                    }
                }
            }
            MessageBox.Show("下载完毕！");
        }
    }


}
