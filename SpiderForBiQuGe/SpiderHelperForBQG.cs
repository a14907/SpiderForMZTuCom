using BaseSpiderForNovelWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace SpiderForBiQuGe
{
    public class SpiderHelperForBQG : ISpiderForBiQuGe
    {
        public SpiderHelperForBQG(string cookie)
        {
            this.cookie = cookie;
        }
        Regex reg = new Regex("&nbsp;&nbsp;&nbsp;&nbsp;");
        Regex regLink = new Regex("（.*?）");
        Regex regBr = new Regex(@"<br><br>");
        private string cookie;

        public async Task<dynamic> GetNovelName(string url)
        {
            var doc = await GetDocmentFromUrl(url);
            //获取标题
            var bt = doc.DocumentNode.SelectSingleNode("//div[@id='info']/h1[1]").FirstChild.InnerText;
            //获取作者
            var zz = doc.DocumentNode.SelectSingleNode("//div[@id='info']/p[1]").InnerText;
            zz = reg.Replace(zz, "");
            zz = zz.Replace('：', '_');
            return new { doc, fname = string.Format("{0}_{1}.txt", bt, zz) };
        }

        public IEnumerable<Tuple<string, string>> _01_GetAllZhangJieUrl(HtmlDocument doc)
        {
            List<Tuple<string, string>> us = new List<Tuple<string, string>>();
            var ds = doc.DocumentNode.SelectNodes("//div[@id='list']/dl/dd/a");
            try
            {
                //获取所有章节所在的url
                foreach (var item in ds)
                {
                    //章节名字
                    var cName = item.InnerText;
                    if (string.IsNullOrEmpty(reg.Replace(cName, "")))
                    {
                        continue;
                    }
                    //章节链接
                    var link = item.Attributes["href"].Value;
                    us.Add(new Tuple<string, string>(cName, link));
                }
                return us;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<bool> _02_GetZhangJieContent(string urlOfZahngJie, StreamWriter fs)
        {
            var doc = await GetDocmentFromUrl(urlOfZahngJie);
            //章节内容
            var c = doc.DocumentNode.SelectSingleNode("//div[@id='content']").InnerHtml;
            
            c = reg.Replace(c, "");
            c = regBr.Replace(c, "\r\n");
            c = c.Replace("<script>readx();</script>", "");
            fs.Write(c);
            fs.WriteLine();
            return true;
        }


        public async Task<HtmlDocument> GetDocmentFromUrl(string url)
        {
            string content = await GetHtmlPage(url);
            HtmlDocument d = new HtmlDocument();
            d.LoadHtml(content);
            return d;
        }

        public async Task<string> GetHtmlPage(string url)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
                string resStr = "";
                req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.71 Safari/537.36";
                req.Headers.Add("Cookie", this.cookie.Trim());
                var res = await req.GetResponseAsync();
                var resStream = res.GetResponseStream();

                using (var reader = new StreamReader(resStream, Encoding.GetEncoding("GBK")))
                {
                    resStr = await reader.ReadToEndAsync();
                }

                res.Dispose();
                resStream.Dispose();

                return resStr;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
