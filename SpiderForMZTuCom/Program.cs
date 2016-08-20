using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace SpiderForMZTuCom
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            WebClient c = new WebClient();
            var res = c.DownloadString("http://www.mzitu.com");
            HtmlAgilityPack.HtmlDocument d = new HtmlAgilityPack.HtmlDocument();
            d.LoadHtml(res);
            var node = d.GetElementbyId("pins");
            List<string> ls = new List<string>();
            foreach (var item in node.ChildNodes)
            {
                var u = item.SelectNodes("//span[0]/a[0]")[0].GetAttributeValue("href", "");
                if (!string.IsNullOrEmpty(u))
                {
                    ls.Add(u);
                }
            }
            Console.WriteLine();
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainForm());
        }
    }
}
