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
using System.Text.RegularExpressions;

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
                var baseNode = GetDanDuNode(d.DocumentNode.SelectSingleNode("//div[@class='NewPages']"));
                var firstUrl = baseNode.SelectNodes("//a")[2].GetAttributeValue("href", "");
                var templateUrl = firstUrl.Substring(0, firstUrl.Length - 5);
                //2.获取总页数
                var lastUrl = baseNode.SelectNodes("//a").Last().GetAttributeValue("href", "");
                int i1 = lastUrl.IndexOf("-") + 1;
                int i2 = lastUrl.IndexOf(".htm");
                var totalCount = int.Parse(lastUrl.Substring(i1, i2 - i1));
                //3.拼接集合返回
                return ProduceUrlCollection(templateUrl, totalCount, ".htm", i2 - i1);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<string> GetTuJiName(string url)
        {
            var d = await GetHtmlDocumentFromUrl(url);
            return HttpUtility.HtmlDecode( d.DocumentNode.SelectNodes("//div[@class='ArticleTitle']/strong").First().InnerText);
        }

        private IEnumerable<string> ProduceUrlCollection(string t, int count, string strInEnd = "", int buNum = 2)
        {
            List<string> ls = new List<string>();
            for (int i = 0; i <= count; i++)
            {
                int index = i;
                if (string.IsNullOrEmpty(strInEnd))
                {
                    ls.Add(t + index + strInEnd);
                }
                else
                {
                    var zeroNum = buNum - index.ToString().Length;
                    var temp = new int[zeroNum].Aggregate("", (ta, item) => ta + item);
                    ls.Add(t + temp + index + strInEnd);
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
            var nodes = d.DocumentNode.SelectNodes("//div[@class='TypeList']/li");
            List<string> ls = new List<string>();
            foreach (var item in nodes)
            {
                var u = item.ChildNodes[0].GetAttributeValue("href", "");
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
            HtmlDocument d = new HtmlDocument();
            d.LoadHtml(res);
            return d;
        }

        public override async Task<IEnumerable<string>> _3GetAllImgUrlInTuJi(string tuJiUrl)
        {
            //1.获取包含图片的每一个地址
            var doc = await GetHtmlDocumentFromUrl(tuJiUrl);
            int count = 0;
            try
            {
                count = int.Parse(doc.DocumentNode.SelectNodes("//div[@class='NewPages']/ul/li[1]/a[1]").First().InnerText.Replace("共", "").Replace("页: ", ""));
            }
            catch (Exception)
            {
                count = 0;
            }
            List<string> us = GetAllPageHaveImg(tuJiUrl, count);
            //2.循环获取每一个页面的图片，拼接成集合
            List<string> imgs = new List<string>();
            foreach (var u in us)
            {
                var pageDoc = await GetHtmlDocumentFromUrl(u);
                var imgUrl = pageDoc.DocumentNode.SelectNodes("//div[@class='ImageBody']/p/img").ToList().Select(m => m.GetAttributeValue("src", ""));//.GetAttributeValue("src","");
                imgs.AddRange(imgUrl);
            }
            //3.返回
            return imgs;
        }

        private List<string> GetAllPageHaveImg(string tuJiUrl, int count)
        {
            List<string> ls = new List<string>();
            string template = tuJiUrl.Replace(".htm", "_{0}.htm");
            for (int i = 1; i < count; i++)
            {
                ls.Add(string.Format(template, i + 1));
            }
            ls.Add(tuJiUrl);
            return ls;
        }

        public async Task<IEnumerable<string>> _3GetAllImgUrlInTuJiInGuiLv(string tuJiUrl, int count, int holder)
        {
            try
            {
                //1.获取模板路径
                var d = await GetHtmlDocumentFromUrl(tuJiUrl);
                var baseNode = d.DocumentNode.SelectSingleNode("//div[@class='ImageBody']");
                var firstUrl = baseNode.SelectNodes("//img").First().GetAttributeValue("src", "");
                var templateUrl = new Regex("[0-9]+[.]jpg").Replace(firstUrl,"");
                //2.获取总数
                var totalCount = count;
                //3.拼接集合返回
                return ProduceUrlCollection(templateUrl, totalCount, ".jpg", holder);
            }
            catch (Exception ex)
            {
                var sssss = ex.Message;
                throw;
            }

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
