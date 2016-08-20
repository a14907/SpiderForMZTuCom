using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using System.Configuration;
using System.Net;
using System.IO;

namespace SpiderForMZTuCom
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyImgDb"].ToString()))
            {
                using (SqlCommand cmd = new SqlCommand("select count(1) from FileLocNames", conn))
                {
                    conn.Open();
                    var res = cmd.ExecuteScalar();
                    MessageBox.Show(res.ToString());
                }
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyImgDb"].ToString()))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    conn.Open();
                    cmd.Connection = conn;

                    string mainUrl = "http://www.mzitu.com/";
                    MySpiderHelper m = new MySpiderHelper();
                    var _1lsPage = await m._1GetAllListPageUrlFromMainPage(mainUrl);


                    var lsPage = _1lsPage.First();

                    var _2ls = await m._2GetTuJiFromListPageUrl(lsPage);
                    int count = 0;
                    foreach (var tujiPage in _2ls)
                    {

                        var filename = HttpUtility.HtmlDecode(await m.GetFileSavePath(tujiPage));

                        cmd.CommandText = @"select count(1) from [dbo].[FileLocNames] where [FileLocation] like '%" + filename + "%'";
                        var res = int.Parse(cmd.ExecuteScalar().ToString());
                        if (res != 0)
                        {
                            continue;
                        }
                        count++;
                        var imgs = await m._3GetAllImgUrlInTuJi(tujiPage);
                        cmd.CommandText = string.Format(@"
                                                insert into 
                                                FileLocNames(FileLocation) 
                                                output inserted.FLocId
                                                values('{0}')", filename);
                        var flocId = "";
                        try
                        {
                            flocId = cmd.ExecuteScalar().ToString();
                        }
                        catch (Exception ex)
                        {
                            ShowInnerMsg(ex);
                        }

                        if (!string.IsNullOrEmpty(flocId))
                        {

                            foreach (var imgUrl in imgs)
                            {
                                cmd.CommandText = string.Format(@"
                                    insert into 
                                    MeiZiTuImgs(ImgUrl,FLocId) 
                                    values('{0}','{1}')", imgUrl, flocId);
                                try
                                {
                                    cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    ShowInnerMsg(ex);
                                }
                            }
                        }

                    }

                    MessageBox.Show("更新图集数量为：" + count);
                }

            }
        }

        private async void btnInitDownLoad_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyImgDb"].ToString()))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    conn.Open();
                    cmd.Connection = conn;

                    string mainUrl = "http://www.mzitu.com/";
                    MySpiderHelper m = new MySpiderHelper();
                    var _1lsPage = await m._1GetAllListPageUrlFromMainPage(mainUrl);
                    int indexlsPage = 0, lsPageCount = _1lsPage.Count();
                    foreach (var lsPage in _1lsPage)
                    {
                        indexlsPage++;
                        textBox1.Text = lsPageCount + "/" + indexlsPage;
                        var _2ls = await m._2GetTuJiFromListPageUrl(lsPage);
                        int indextjPage = 0, tjPageCount = _2ls.Count();
                        foreach (var tujiPage in _2ls)
                        {
                            textBox2.Text = tjPageCount + "/" + indextjPage;
                            var filename = HttpUtility.HtmlDecode(await m.GetFileSavePath(tujiPage));
                            var imgs = await m._3GetAllImgUrlInTuJi(tujiPage);
                            cmd.CommandText = string.Format(@"
                                                insert into 
                                                FileLocNames(FileLocation) 
                                                output inserted.FLocId
                                                values('{0}')", filename);
                            var flocId = "";
                            try
                            {
                                flocId = cmd.ExecuteScalar().ToString();
                            }
                            catch (Exception ex)
                            {
                                ShowInnerMsg(ex);
                            }
                            int indeximgPage = 0, imgPageCount = imgs.Count();
                            if (!string.IsNullOrEmpty(flocId))
                            {
                                textBox3.Text = imgPageCount + "/" + indeximgPage;
                                foreach (var imgUrl in imgs)
                                {
                                    cmd.CommandText = string.Format(@"
                                    insert into 
                                    MeiZiTuImgs(ImgUrl,FLocId) 
                                    values('{0}','{1}')", imgUrl, flocId);
                                    try
                                    {
                                        cmd.ExecuteNonQuery();
                                    }
                                    catch (Exception ex)
                                    {
                                        ShowInnerMsg(ex);
                                    }
                                }
                            }

                        }
                    }
                    MessageBox.Show("整体获取完成");
                }

            }
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
            string kw = lbShow.Text.Trim();
            if (string.IsNullOrEmpty(kw))
            {
                return;
            }
            FolderBrowserDialog f = new FolderBrowserDialog();
            var res = f.ShowDialog();
            if (res != DialogResult.OK)
            {
                return;
            }
            var folder = f.SelectedPath;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyImgDb"].ToString()))
            {
                int index = 0;
                using (SqlCommand cmd = new SqlCommand())
                {
                    conn.Open();
                    cmd.Connection = conn;


                    cmd.CommandText = string.Format(@"select m.ImgUrl from MeiZiTuImgs m
join  FileLocNames f on m.FLocId=f.FLocId where f.FileLocation like  '%" + kw + "'");
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    var dt = new DataTable();
                    da.Fill(dt);
                    WebClient c = new WebClient();
                    foreach (DataRow item in dt.Rows)
                    {
                        var url = item["ImgUrl"].ToString();
                        if (!Directory.Exists(folder + "\\" + kw))
                        {
                            Directory.CreateDirectory(folder + "\\" + kw);
                        }
                        await c.DownloadFileTaskAsync(url, folder + "\\" + kw + "\\" + Path.GetFileName(url));
                        index++;
                        textBox4.Text = dt.Rows.Count+"/"+index;
                    }
                }

                MessageBox.Show("下载完成！");
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string kw = tbKeyWords.Text.Trim();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyImgDb"].ToString()))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    conn.Open();
                    cmd.Connection = conn;


                    cmd.CommandText = string.Format(@"select FLocId,FileLocation from FileLocNames where FileLocation like '%" + kw + "%'");
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    var dt = new DataTable();
                    da.Fill(dt);
                    WebClient c = new WebClient();
                    foreach (DataRow item in dt.Rows)
                    {
                        var id = item["FLocId"].ToString();
                        var loc = item["FileLocation"].ToString();
                        lbShow.Items.Add(loc.Split('\\')[loc.Split('\\').Length - 1]);
                    }
                }

            }
        }
    }
}
