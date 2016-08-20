using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BaseSpiderForImgWeb
{
    public abstract class SpiderHelperBase : ISpiderBasic
    {

        /*
        1.获取全部的列表页的地址，参数为网站首页
        2.获取某一个列表页全部图集的名字，参数为某一个列表页的地址
        3.获取一个图集里面全部的图片地址，参数为图集地址的第一页
        4.下载图片的方法，可以由本抽象类提供默认实现
        5.获取每张图片的存放地址
        */
        public virtual async Task<bool> DownLoadImgAsync(string imgUrl, string fileName)
        {
            try
            {
                WebClient c = new WebClient();
                await c.DownloadFileTaskAsync(imgUrl, fileName);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public virtual bool DownLoadImg(string imgUrl, string fileName)
        {
            try
            {
                WebClient c = new WebClient();
                c.DownloadFile(imgUrl, fileName);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public abstract IEnumerable<string> GetAllImgUrlInTuJi(string tuJiUrl);
        public abstract IEnumerable<string> GetAllListPageUrl(string mainPageUrl);
        public abstract string GetFileSavePath(string tuJiUrl);
        public abstract IEnumerable<string> GetTuJiFromListPageUrl(string listPageUrl);
    }
}
