﻿using BaseSpiderForNovelWeb;
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
    public class SpiderHelper : ISpiderForBiQuGe
    {
        Regex reg = new Regex("&nbsp;&nbsp;&nbsp;&nbsp;");
        Regex regLink = new Regex("（.*?）");
        Regex regBr = new Regex(@"<br><br>[\s]{4}");

        public async Task<dynamic> GetNovelName(string url)
        {
            var doc = await GetDocmentFromUrl(url);
            //获取标题
            var bt = doc.DocumentNode.SelectSingleNode("//div[@class='Main']/h1[1]").FirstChild.InnerText;
            //获取作者
            var zz = doc.DocumentNode.SelectSingleNode("//div[@class='Main']/h1[1]/span[1]").InnerText;
            zz = reg.Replace(zz, "");
            zz = zz.Replace(':','_');
            return new { doc, fname = string.Format("{0}_{1}.txt", bt, zz) };
        }

        public IEnumerable<Tuple<string, string>> _01_GetAllZhangJieUrl(HtmlDocument doc)
        {
            List<Tuple<string, string>> us = new List<Tuple<string, string>>();
            var ds = doc.DocumentNode.SelectNodes("//div[@id='chapter']/dl/dd/a");
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
            var c = doc.DocumentNode.SelectSingleNode("//div[@id='text_area']").InnerHtml;

            c=System.Web.HttpUtility.HtmlDecode(c).TrimStart();

            c = regLink.Replace(c,"");

            c = reg.Replace(c, "");
            c = regBr.Replace(c, "\r\n");
            c = c.Replace("(美克文学http://www.meike-shoes.com)", "");
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
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            string resStr = "";
            req.UserAgent = "IE";
            var res = await req.GetResponseAsync();
            var resStream = res.GetResponseStream();

            using (var reader = new StreamReader(resStream,Encoding.GetEncoding("GBK")))
            {
                resStr = await reader.ReadToEndAsync();
            }

            res.Dispose();
            resStream.Dispose();

            return resStr;
        }
    }
}
