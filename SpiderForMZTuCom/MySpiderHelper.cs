using BaseSpiderForImgWeb;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Configuration;
using System.Web;
using System.Text;
using System.IO.Compression;
using System.Threading;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace SpiderForMZTuCom
{
    public class MySpiderHelper
    {
        /*
        1.获取全部的列表页的地址，参数为网站首页
        2.获取某一个列表页全部图集的名字，参数为某一个列表页的地址
        3.获取一个图集里面全部的图片地址，参数为图集地址的第一页
        4.下载图片的方法，可以由本抽象类提供默认实现
        5.获取每张图片的存放地址
        */


        public async Task<IEnumerable<string>> _1GetAllListPageUrlFromMainPage(string mainPageUrl)
        {
            //1.先获取模板
            var d = await GetHtmlDocumentFromUrl(mainPageUrl);
            var baseNode = GetDanDuNode(d.DocumentNode.SelectSingleNode("//div[@class='nav-links']"));
            var firstUrl = baseNode.SelectNodes("//a").First().GetAttributeValue("href", "").TrimEnd('/');
            var templateUrl = firstUrl.Substring(0, firstUrl.Length - 1);
            //2.获取总页数
            var lastUrl = baseNode.SelectNodes("//a")[3].InnerText;
            var totalCount = int.Parse(lastUrl);
            //3.拼接集合返回
            return ProduceUrlCollection(templateUrl, totalCount);
        }

        private IEnumerable<string> ProduceUrlCollection(string t, int count, string strInEnd = "", bool isFix2 = false)
        {
            List<string> ls = new List<string>();
            for (int i = 0; i < count; i++)
            {
                int index = i + 1;
                if (isFix2 && index >= 1 && index <= 9)
                {
                    ls.Add(t + "0" + index + strInEnd);
                }
                else
                {
                    ls.Add(t + index + strInEnd);
                }

            }
            return ls;
        }
        private HtmlNode GetDanDuNode(HtmlNode node)
        {
            return HtmlNode.CreateNode(node.OuterHtml);
        }

        public async Task<IEnumerable<Tuple<string, string>>> _2GetTuJiFromListPageUrl(string listPageUrl)
        {
            HtmlDocument d = await GetHtmlDocumentFromUrl(listPageUrl);
            var nodes = d.DocumentNode.SelectNodes("//ul[@id='pins']/li");
            List<Tuple<string, string>> ls = new List<Tuple<string, string>>();
            foreach (var item in nodes)
            {
                var a = item.ChildNodes[1].FirstChild;
                if (a == null)
                {
                    continue;
                }
                var key = a.InnerText;
                var val = a.GetAttributeValue("href", "");
                if (!string.IsNullOrEmpty(val))
                {
                    ls.Add(Tuple.Create(key, val));
                }
            }
            return ls;
        }

        private static async Task<HtmlDocument> GetHtmlDocumentFromUrl(string listPageUrl)
        {
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    var res = await GetHtmlPage(listPageUrl);
                    var d = new HtmlDocument();
                    d.LoadHtml(res);
                    return d;
                }
                catch (WebException ex)
                {
                    if (ex.Response is HttpWebResponse response && (int)response.StatusCode == 429)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(6));
                        continue;
                    }
                    throw;
                }
            }
            throw new Exception("重试次数超过3次：" + listPageUrl);
        }

        public async Task<IEnumerable<string>> _3GetAllImgUrlInTuJi(string tuJiUrl)
        {
            //1.获取模板路径
            var d = await GetHtmlDocumentFromUrl(tuJiUrl);
            var baseNode = d.DocumentNode.SelectSingleNode("//div[@class='main-image']");
            var firstUrl = baseNode.SelectNodes("//img").First().GetAttributeValue("src", "");
            //2.获取总数
            var lastUrl = baseNode.SelectSingleNode("//div[@class='pagenavi']/a[last()-1]").InnerText;
            int totalCount;
            if (!int.TryParse(lastUrl, out totalCount))
            {
                totalCount = 1;
            }
            if (totalCount == 1)
            {
                var pp = await GetHtmlDocumentFromUrl(tuJiUrl);
                var baseNodepp = pp.DocumentNode.SelectSingleNode("//div[@class='main-image']");
                var imgUrls = baseNodepp.SelectNodes("//img").Cast<HtmlNode>().Select(m => m.GetAttributeValue("src", ""));
                return imgUrls;
            }
            if (firstUrl.EndsWith("01.jpg"))
            {
                var templateUrl = firstUrl.Replace("01.jpg", "");
                //3.拼接集合返回
                return ProduceUrlCollection(templateUrl, totalCount, ".jpg", true);
            }
            else
            {
                var tujis = GetAllTuJiUrl(tuJiUrl, totalCount);
                return await GetAllImages(tujis);
            }

        }

        private async Task<IEnumerable<string>> GetAllImages(List<string> tujis)
        {
            var res = new List<string>();
            foreach (var item in tujis)
            {
                Thread.Sleep(200);
                var d = await GetHtmlDocumentFromUrl(item);
                var baseNode = d.DocumentNode.SelectSingleNode("//div[@class='main-image']");
                var firstUrl = baseNode.SelectNodes("//img").First().GetAttributeValue("src", "");
                res.Add(firstUrl);
            }
            return res;
        }

        private List<string> GetAllTuJiUrl(string tuJiUrl, int totalCount)
        {
            var res = new List<string>();
            res.Add(tuJiUrl);
            for (int i = 2; i < totalCount; i++)
            {
                res.Add(tuJiUrl + "/" + i);
            }
            return res;
        }

        public async Task<string> GetFileSaveDirectoryName(string tuJiUrl)
        {
            var d = await GetHtmlDocumentFromUrl(tuJiUrl);
            var res = HttpUtility.HtmlDecode(d.DocumentNode.SelectSingleNode("//div[@class='currentpath']").InnerText).Replace("当前位置:", "")
                .Split('»').Last().Trim();
            return res;
        }


        public static async Task<string> GetHtmlPage(string url)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            string resStr = "";
            req.UserAgent = "IE";
            req.ServerCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => { return true; };
            req.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip");
            var res = (await req.GetResponseAsync()) as HttpWebResponse;
            var resStream = res.GetResponseStream();

            if (resStream != null)
            {
                if (res.ContentEncoding.ToLower().Contains("gzip"))
                {
                    using (GZipStream gzipReader = new GZipStream(resStream, CompressionMode.Decompress))
                    {
                        using (var reader = new StreamReader(gzipReader))
                        {
                            resStr = await reader.ReadToEndAsync();
                        }
                    }
                }
                else
                {
                    using (var reader = new StreamReader(resStream))
                    {
                        resStr = await reader.ReadToEndAsync();
                    }
                }

                res.Dispose();
                resStream.Dispose();
            }

            return resStr;
        }
    }
}
