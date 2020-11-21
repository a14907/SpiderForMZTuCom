using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using System.Configuration;
using System.Net;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace SpiderForMZTuCom
{
    public partial class MainForm : Form
    {
        private MySpiderHelper spiderHelper = new MySpiderHelper();
        private string _conn = ConfigurationManager.ConnectionStrings["MyImgDb"].ToString();
        private FolderBrowserDialog _folderBrowserDialog = new FolderBrowserDialog();
        private ServiceProvider _serviceProvider;
        private MyDbContext _dbContext;

        public MainForm()
        {
            InitializeComponent();
            var sc = new ServiceCollection();
            sc.AddDbContext<MyDbContext>(opt =>
            {
                opt.UseSqlite("Data Source=MeiZiTuDB.dll");
            });
            _serviceProvider = sc.BuildServiceProvider();
            _dbContext = _serviceProvider.GetService<MyDbContext>();
            EnsureDbExists();
        }

        protected override void OnClosed(EventArgs e)
        {
            _dbContext.Dispose();
            _serviceProvider.Dispose();
            base.OnClosed(e);
        }

        private void EnsureDbExists()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<MyDbContext>();
                db.Database.EnsureCreated();
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            lbShow.Items.Clear();

            string mainUrl = "https://www.mzitu.com/";
            MySpiderHelper m = new MySpiderHelper();
            var _1LsPage = await m._1GetAllListPageUrlFromMainPage(mainUrl);
            if (_1LsPage == null) throw new ArgumentNullException("_1LsPage");

            int count = 0;
            int checkCount = int.Parse(tbCheckCount.Text);

            for (int i = 0; i < checkCount; i++)
            {
                var lsPage = _1LsPage.Skip(i).First();
                await Task.Delay(TimeSpan.FromSeconds(2));
                var _2Ls = await m._2GetTuJiFromListPageUrl(lsPage);
                foreach (var tujiPage in _2Ls)
                {
                    string filename;
                    try
                    {
                        filename = HttpUtility.HtmlDecode(tujiPage.Item1);
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }

                    if (_dbContext.FileLocNames.Any(a => a.CollectionName.Contains(filename)))
                    {
                        continue;
                    }

                    count++;
                    lbShow.Items.Add(filename);

                    try
                    {
                        _dbContext.FileLocNames.Add(new FileLocNames { CollectionName = filename, CollectionUrl = tujiPage.Item2 });
                    }
                    catch (Exception ex)
                    {
                        ShowInnerMsg(ex);
                    }
                }
            }
            _dbContext.SaveChanges();

            MessageBox.Show(@"更新图集数量为：" + count);

        }

        private async void btnInitDownLoad_Click(object sender, EventArgs e)
        {

            string mainUrl = "https://www.mzitu.com/";

            var _1LsPage = await spiderHelper._1GetAllListPageUrlFromMainPage(mainUrl);
            if (_1LsPage != null)
            {
                int indexlsPage = 0, lsPageCount = _1LsPage.Count();
                foreach (var lsPage in _1LsPage)
                {
                    indexlsPage++;
                    textBox1.Text = lsPageCount + "/" + indexlsPage;
                    var _2Ls = await spiderHelper._2GetTuJiFromListPageUrl(lsPage);
                    int indextjPage = 0, tjPageCount = _2Ls.Count();
                    foreach (var tujiPage in _2Ls)
                    {
                        indextjPage++;
                        textBox2.Text = tjPageCount + "/" + indextjPage;
                        var directoryName = HttpUtility.HtmlDecode(tujiPage.Item1);
                        try
                        {
                            _dbContext.FileLocNames.Add(new FileLocNames { CollectionName = directoryName, CollectionUrl = tujiPage.Item2 });
                        }
                        catch (Exception ex)
                        {
                            ShowInnerMsg(ex);
                        }
                    }
                    _dbContext.SaveChanges();
                }
            }
            MessageBox.Show(@"整体获取完成");

        }

        private void ShowInnerMsg(Exception ex)
        {
            MessageBox.Show(GetInnerException(ex).Message);
        }

        private Exception GetInnerException(Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            return ex;
        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            var res = _folderBrowserDialog.ShowDialog();
            if (res != DialogResult.OK)
            {
                return;
            }
            var folder = _folderBrowserDialog.SelectedPath;

            if (lbShow.SelectedItem == null)
            {
                if (lbShow.Items.Count == 0)
                {
                    MessageBox.Show("未选择");
                    return;
                }
                foreach (var item in lbShow.Items)
                {
                    await DownLoadTuJi(item.ToString(), folder);
                }
            }
            else
            {
                string kw = lbShow.Text.Trim();
                await DownLoadTuJi(kw, folder);
            }

            MessageBox.Show(@"下载完成！");
        }

        private System.Threading.SemaphoreSlim _downloadSemaphoreSlim = new System.Threading.SemaphoreSlim(6, 6);
        private async Task DownLoadTuJi(string kw, string folder)
        {

            if (string.IsNullOrEmpty(kw))
            {
                return;
            }

            int index = 0;
            string pageUrl = string.Empty;

            var f = _dbContext.FileLocNames.FirstOrDefault(m => m.CollectionName.Contains(kw));
            if (f == null)
            {
                return;
            }
            pageUrl = f.CollectionUrl;


            var imgs = await spiderHelper._3GetAllImgUrlInTuJi(pageUrl);
            int imgPageCount = imgs.Count();

            WebClient c = new WebClient();
            char[] cs = { '\\', '/', '*', ':', '?', '<', '>', '|' , "\""};
            foreach (var item in cs)
            {
                kw = kw.Replace(item, '_');
            }
            foreach (var url in imgs)
            {
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        await _downloadSemaphoreSlim.WaitAsync();
                        if (!Directory.Exists(folder + "\\" + kw))
                        {
                            Directory.CreateDirectory(folder + "\\" + kw);
                        }
                        MakeSureHeaders(c.Headers);
                        await c.DownloadFileTaskAsync(url, folder + "\\" + kw + "\\" + Path.GetFileName(url));
                        index++;
                        textBox4.Text = imgPageCount + "/" + index;
                        break;
                    }
                    catch (WebException ex)
                    {
                        if (ex.Response is HttpWebResponse response && (int)response.StatusCode == 429)
                        {
                            await Task.Delay(TimeSpan.FromSeconds(3));
                            continue;
                        }
                    }
                    catch (Exception)
                    {
                    }
                    finally
                    {
                        _downloadSemaphoreSlim.Release();
                    }
                }
            }
            c.Dispose();


        }

        private void MakeSureHeaders(WebHeaderCollection headers)
        {
            var arr = new[] { "Referer" };
            var target = headers.Cast<string>();
            foreach (var item in arr)
            {
                if (!target.Contains(item))
                {
                    headers.Add(HttpRequestHeader.Referer, "https://www.mzitu.com");
                }
            }
            if (!target.Contains("Sec-Fetch-Site"))
            {
                headers.Add("Sec-Fetch-Site", "cross-site");
            }
            if (!target.Contains("Sec-Fetch-Mode"))
            {
                headers.Add("Sec-Fetch-Mode", "no-cors");
            }
            if (!target.Contains("Sec-Fetch-Dest"))
            {
                headers.Add("Sec-Fetch-Dest", "image");
            }
            if (!target.Contains("User-Agent"))
            {
                headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.132 Safari/537.36");
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            lbShow.Items.Clear();
            string kw = tbKeyWords.Text.Trim();

            var dt = _dbContext.FileLocNames.Where(m => m.CollectionName.Contains(kw)).ToList();

            WebClient c = new WebClient();
            foreach (var item in dt)
            {
                var id = item.Id;
                var loc = item.CollectionName;
                lbShow.Items.Add(loc);
            }
            c.Dispose();

        }
    }
}
