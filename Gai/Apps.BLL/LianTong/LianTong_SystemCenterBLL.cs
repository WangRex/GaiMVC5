using Apps.Common;
using Apps.DAL.LianTong;
using Apps.DAL.Spl;
using Apps.Models;
using Apps.Models.Spl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.BLL.LianTong
{
    public partial class LianTong_SystemCenterBLL
    {
        public LianTong_SystemCenterRepository m_Rep;
        public LianTong_SystemCenterBLL()
        {
            m_Rep = new LianTong_SystemCenterRepository();
        }

    }
}
