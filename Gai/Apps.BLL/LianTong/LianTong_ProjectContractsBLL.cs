﻿using Apps.Common;
using Apps.DAL.LianTong;
using Apps.DAL.Spl;
using Apps.Models;
using Apps.Models.Enum;
using Apps.Models.LianTong;
using Apps.Models.Spl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.BLL.LianTong
{
    public partial class LianTong_ProjectContractsBLL
    {
        public LianTong_ProjectContractsRepository m_Rep;
        public LianTong_ProjectRepository LianTong_Project;
        public LianTong_ProjectContractsBLL()
        {
            m_Rep = new LianTong_ProjectContractsRepository();
            LianTong_Project = new LianTong_ProjectRepository();
        }

        public Boolean BindingProject(string ContractsId, string projectNums)
        {
            string[] arr = projectNums.Split(',');
            var LianTong_ProjectContracts = m_Rep.Find(Convert.ToInt32(ContractsId));
            if (LianTong_ProjectContracts == null)
            {
                return false;
            }
            LianTong_ProjectContracts.projectNum = string.Empty;
            foreach (string projectNum in arr)
            {
                if (!string.IsNullOrWhiteSpace(projectNum))
                {
                    var project = LianTong_Project.Find(Convert.ToInt32(projectNum));
                    if (project != null)
                    {
                        LianTong_ProjectContracts.projectNum = LianTong_ProjectContracts.projectNum + "," + project.projectNum;
                        LianTong_ProjectContracts.status = "等待送审";
                        m_Rep.Update(LianTong_ProjectContracts);
                        project.contractNum = LianTong_ProjectContracts.contractNum;
                        LianTong_Project.Update(project);

                    }
                    else
                    {
                        return false;
                    }
                }
            }
            LianTong_ProjectContracts.history = FlowLianTongContracts.送审.GetInt().ToString();
            return true;
        }

        //首页合同数
        public LianTong_ContractsCntViewModels ContractsCnt(string department)
        {
            //获取实体列表
            LianTong_ContractsCntViewModels refModels = new LianTong_ContractsCntViewModels();
            IQueryable<LianTong_ProjectContractsModel> _LianTong_Contracts = m_Rep.FindList();
            if (!string.IsNullOrEmpty(department)) _LianTong_Contracts = _LianTong_Contracts.Where(cm => cm.department == department);

            refModels.NewContracts = _LianTong_Contracts.Where(cm => cm.history == null).Count();
            refModels.ConnectWait = _LianTong_Contracts.Where(cm => cm.history == null).Count();
            refModels.ApproveWait = _LianTong_Contracts.Where(cm => cm.history == "1").Count();
            refModels.FullWait = _LianTong_Contracts.Where(cm => cm.history == "2").Count();
            refModels.CheckWait = _LianTong_Contracts.Where(cm => cm.history == "3").Count();
            refModels.InvoiceWait = _LianTong_Contracts.Where(cm => cm.history == "4").Count();
            refModels.PaymentsWait = _LianTong_Contracts.Where(cm => cm.history == "5").Count();
            refModels.OverContracts = _LianTong_Contracts.Where(cm => cm.history == "6").Count();
            return refModels;
        }

        //首页合同数
        public Dictionary<string, long> outlineCostReport()
        {
            long TotalC = 0;
            List<LianTong_ProjectContractsModel> reportList = m_Rep.FindList().Where(cm => cm.outlineCost != null && cm.department != null).ToList();
            Dictionary<string, long> RefList = new Dictionary<string, long>();
            foreach (LianTong_ProjectContractsModel item in reportList)
            {
                long cost = 0;
                if (!string.IsNullOrEmpty(item.outlineCost))
                {
                    cost = Convert.ToInt64(item.outlineCost);
                }
                if (RefList.ContainsKey(item.departmentName))
                {
                    RefList[item.departmentName] = RefList[item.departmentName] + cost;
                }
                else
                {
                    RefList.Add(item.departmentName, cost);
                }
                TotalC = TotalC + cost;
            }
            RefList.Add("TotalC", TotalC);
            return RefList;
        }
    }
}
