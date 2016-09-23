using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BaseSpiderForNovelWeb
{
    public interface ISpiderForBiQuGe
    {
        IEnumerable<Tuple<string,string>> _01_GetAllZhangJieUrl(HtmlDocument doc);
        Task<bool> _02_GetZhangJieContent(string urlOfZahngJie,  StreamWriter fs);
    }
}
