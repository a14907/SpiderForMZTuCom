using BaseSpiderForImgWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderForMZTuCom
{
    public class MySpiderHelper : SpiderHelperBase
    {
        private string mainPage;
        public MySpiderHelper(string mainPage)
        {
            this.mainPage = mainPage;
        }


        public override IEnumerable<string> GetAllImgUrlInTuJi(string tuJiUrl)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<string> GetAllListPageUrl(string mainPageUrl)
        {
            throw new NotImplementedException();
        }

        public override string GetFileSavePath(string tuJiUrl)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<string> GetTuJiFromListPageUrl(string listPageUrl)
        {
            throw new NotImplementedException();
        }
    }
}
