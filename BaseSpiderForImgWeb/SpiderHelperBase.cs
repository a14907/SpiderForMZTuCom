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
                using (WebClient c = new WebClient())
                {
                    await c.DownloadFileTaskAsync(imgUrl, fileName);
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public virtual async Task<bool> DownLoadImg(string imgUrl, string fileName)
        {
            try
            {
                using (WebClient c = new WebClient())
                {
                    await c.DownloadFileTaskAsync(imgUrl, fileName);
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public abstract Task<IEnumerable<string>> _3GetAllImgUrlInTuJi(string tuJiUrl);
        public abstract Task<IEnumerable<string>> _1GetAllListPageUrlFromMainPage(string mainPageUrl);
        public abstract Task<string> GetFileSaveDirectoryName(string tuJiUrl);
        public abstract Task<IEnumerable<string>> _2GetTuJiFromListPageUrl(string listPageUrl);
    }
}
