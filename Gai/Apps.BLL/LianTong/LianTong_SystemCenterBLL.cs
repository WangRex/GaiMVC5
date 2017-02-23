using Apps.Common;
using Apps.DAL.LianTong;
using Apps.Models.LianTong;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Apps.BLL.LianTong
{
    public partial class LianTong_SystemCenterBLL
    {
        public LianTong_SystemCenterRepository m_Rep;
        public LianTong_SystemCenterBLL()
        {
            m_Rep = new LianTong_SystemCenterRepository();
        }

        //首页合同金额数
        public Dictionary<string, long> ContratcMoneyCnt()
        {
            List<LianTong_SystemCenterModel> reportList = m_Rep.FindList().ToList();
            Dictionary<string, long> RefList = new Dictionary<string, long>();
            foreach (LianTong_SystemCenterModel item in reportList)
            {
                long contractCost = 0;
                long equipmentCost = 0;
                long projectCost = 0;
                long Profit = 0;
                contractCost = Convert.ToInt64(item.contractCost);
                equipmentCost = Convert.ToInt64(item.equipmentCost);
                projectCost = Convert.ToInt64(item.projectCost);
                Profit = contractCost - equipmentCost - projectCost;
                RefList.Add("contractCost", contractCost);
                RefList.Add("equipmentCost", equipmentCost);
                RefList.Add("projectCost", projectCost);
                RefList.Add("Profit", Profit);
            }
            return RefList;
        }

    }
}
