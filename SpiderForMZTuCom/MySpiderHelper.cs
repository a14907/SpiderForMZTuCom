using BaseSpiderForImgWeb;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web;

namespace SpiderForMZTuCom
{
    public class MySpiderHelper : SpiderHelperBase
    {
        /*
        1.获取全部的列表页的地址，参数为网站首页
        2.获取某一个列表页全部图集的名字，参数为某一个列表页的地址
        3.获取一个图集里面全部的图片地址，参数为图集地址的第一页
        4.下载图片的方法，可以由本抽象类提供默认实现
        5.获取每张图片的存放地址
        */


        public override async Task<IEnumerable<string>> _1GetAllListPageUrlFromMainPage(string mainPageUrl)
        {
            try
            {
                //1.先获取模板
                var d = await GetHtmlDocumentFromUrl(mainPageUrl);
                var baseNode = GetDanDuNode(d.DocumentNode.SelectSingleNode("//div[@class='nav-links']"));
                var firstUrl = baseNode.SelectNodes("//a").First().GetAttributeValue("href", "");
                var templateUrl = firstUrl.Substring(0, firstUrl.Length - 1);
                //2.获取总页数
                var lastUrl = baseNode.SelectNodes("//a")[2].InnerText;
                var totalCount = int.Parse(lastUrl);
                //3.拼接集合返回
                return ProduceUrlCollection(templateUrl, totalCount);
            }
            catch (Exception ex)
            {

                throw;
            }
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
        public override async Task<IEnumerable<string>> _2GetTuJiFromListPageUrl(string listPageUrl)
        {
            HtmlDocument d = await GetHtmlDocumentFromUrl(listPageUrl);
            var nodes = d.DocumentNode.SelectNodes("//ul[@id='pins']/li");
            List<string> ls = new List<string>();
            foreach (var item in nodes)
            {
                var u = item.ChildNodes[1].FirstChild.GetAttributeValue("href", "");
                if (!string.IsNullOrEmpty(u))
                {
                    ls.Add(u);
                }
            }
            return ls;
        }

        private static async Task<HtmlDocument> GetHtmlDocumentFromUrl(string listPageUrl)
        {
            WebClient c = new WebClient();
            var res = await MySpiderHelper.GetHtmlPage(listPageUrl);
            HtmlAgilityPack.HtmlDocument d = new HtmlAgilityPack.HtmlDocument();
            d.LoadHtml(res);
            return d;
        }

        public override async Task<IEnumerable<string>> _3GetAllImgUrlInTuJi(string tuJiUrl)
        {
            //1.获取模板路径
            var d = await GetHtmlDocumentFromUrl(tuJiUrl);
            var baseNode = d.DocumentNode.SelectSingleNode("//div[@class='main-image']");
            var firstUrl = baseNode.SelectNodes("//img").First().GetAttributeValue("src", "");
            var templateUrl = firstUrl.Replace("01.jpg", "");
            //2.获取总数
            var lastUrl = baseNode.SelectSingleNode("//div[@class='pagenavi']/a[last()-1]").InnerText;
            var totalCount = int.Parse(lastUrl);
            //3.拼接集合返回
            return ProduceUrlCollection(templateUrl, totalCount, ".jpg", true);
        }


        public override async Task<string> GetFileSavePath(string tuJiUrl)
        {
            var d = await GetHtmlDocumentFromUrl(tuJiUrl);
            var res = HttpUtility.HtmlDecode(d.DocumentNode.SelectSingleNode("//div[@class='currentpath']").InnerText).Replace("当前位置:", "").Replace(" » ", "\\");
            return ConfigurationManager.AppSettings["FileSavePath"] + res;
        }


        public static async Task<string> GetHtmlPage(string url)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            string resStr = "";
            req.UserAgent = "IE";
            var res = await req.GetResponseAsync();
            var resStream = res.GetResponseStream();

            using (var reader = new StreamReader(resStream))
            {
                resStr = await reader.ReadToEndAsync();
            }

            res.Dispose();
            resStream.Dispose();

            return resStr;
        }
    }
}
