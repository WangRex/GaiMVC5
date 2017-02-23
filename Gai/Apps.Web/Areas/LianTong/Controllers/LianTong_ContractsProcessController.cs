using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Apps.Common;
using Apps.Models;
using Microsoft.Practices.Unity;
using Apps.BLL;
using Apps.Models.Sys;
using System;
using Apps.Web.Core;
using Apps.Locale;
using Apps.BLL.Sys;
using System.Data.SqlClient;
using Apps.BLL.LianTong;
using Apps.Models.LianTong;
using Newtonsoft.Json;
using Apps.Models.Enum;

namespace Apps.Web.Areas.LianTong.Controllers
{
    public class LianTong_ContractsProcessController : BaseController
    {

        private LianTong_ProjectBLL _LianTong_Project = new LianTong_ProjectBLL();
        private LianTong_ProjectContractsBLL m_BLL = new LianTong_ProjectContractsBLL();
        private SysStructBLL StructBLL = new SysStructBLL();
        ValidationErrors errors = new ValidationErrors();
        Dictionary<string, string> dic = new Dictionary<string, string>();

        public LianTong_ContractsProcessController() {
            dic.Add("1", "等待送审");
            dic.Add("2", "等待补全");
            dic.Add("3", "等待审订");
            dic.Add("4", "等待开票");
            dic.Add("5", "等待回款");
            dic.Add("6", "已完结");
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        [SupportFilter]
        public ActionResult Index()
        {

            string RoleName = GetAccount().RoleName;
            if ("[超级管理员]".Equals(RoleName) || "[公司领导]".Equals(RoleName) || RoleName.Contains("[工程管理部]"))
            {
                ViewBag.CurrentStep = FlowLianTongContracts.送审.GetInt().ToString();
            }
            else
            {
                ViewBag.CurrentStep = FlowLianTongContracts.补全.GetInt().ToString();
            }

            return View();
        }

        [HttpPost]
        public ActionResult CreatByGrid()
        {
            string result = Request.Form[0];

            //后台拿到字符串时直接反序列化。根据需要自己处理
            //var datagrid = JsonConvert.DeserializeObject<List<datagrid>>(result);
            List<LianTong_ProjectContractsModel> datagridList = new List<LianTong_ProjectContractsModel>();
            try
            { datagridList = JsonConvert.DeserializeObject<List<LianTong_ProjectContractsModel>>(result); }
            catch (Exception)
            {
                string ErrorCol = "输入数据类型错误，请点撤销后重新输入";
                LogHandler.WriteServiceLog(GetUserId(), ErrorCol, "失败", "数据更新", "合同设置");
                return Json(JsonHandler.CreateMessage(0, Resource.SetFail), JsonRequestBehavior.AllowGet);
            }
            foreach (LianTong_ProjectContractsModel info in datagridList)
            {
                if (info.Id > 0)
                {
                    if (m_BLL.m_Rep.Update(info))
                    {
                        LogHandler.WriteServiceLog(GetUserId(), "Id:" + info.Id + ",contractNum:" + info.contractNum, "成功", "修改", "合同设置");
                    }
                    else
                    {
                        string ErrorCol = errors.Error;
                        LogHandler.WriteServiceLog(GetUserId(), "Id:" + info.Id + ",contractNum:" + info.contractNum + "," + ErrorCol, "失败", "修改", "合同设置");
                        return Json(JsonHandler.CreateMessage(0, Resource.EditFail + ":" + ErrorCol));
                    }
                }
                else
                {
                    info.department = GetAccount().DepId;
                    info.departmentName = StructBLL.m_Rep.Find(Convert.ToInt32(info.department)).Name;
                    if (m_BLL.m_Rep.Create(info))
                    {
                        LogHandler.WriteServiceLog(GetUserId(), "Id:" + info.Id + ",contractNum:" + info.contractNum, "成功", "创建", "合同设置");
                    }
                    else
                    {
                        string ErrorCol = errors.Error;
                        LogHandler.WriteServiceLog(GetUserId(), "Id:" + info.Id + ",contractNum:" + info.contractNum + "," + ErrorCol, "失败", "创建", "合同设置");
                        return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + ErrorCol), JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 异步加载数据
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="rows">每页显示的行数</param>
        /// <param name="search">查询条件</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetList(GridPager pager, string queryStr)
        {
            string DepId = GetAccount().DepId;
            string QuaryCD = StructBLL.m_Rep.Find(Convert.ToInt32(DepId)).Remark;
            string RoleName = GetAccount().RoleName;
            List<LianTong_ProjectContractsModel> list;
            if ("[超级管理员]".Equals(RoleName) || "[公司领导]".Equals(RoleName) || RoleName.Contains("[工程管理部]"))
            {
                if (string.IsNullOrEmpty(queryStr)) queryStr = FlowLianTongContracts.送审.GetInt().ToString();
                list = m_BLL.m_Rep.FindPageList(ref pager, a => a.history == queryStr).ToList();
            }
            else
            {
                if (string.IsNullOrEmpty(queryStr)) queryStr = FlowLianTongContracts.补全.GetInt().ToString();
                list = m_BLL.m_Rep.FindPageList(ref pager, a => a.department == DepId && a.history == queryStr).ToList();
            }
            foreach (LianTong_ProjectContractsModel item in list)
            {
                string strDate = item.validDate;
                if (!string.IsNullOrEmpty(strDate) && item.history == FlowLianTongContracts.送审.GetInt().ToString())
                {
                    if (Convert.ToDateTime(strDate) < DateTime.Now)
                    {
                        item.status = "送审已过期";
                    }
                }
            }
            var json = new
            {
                total = pager.totalRows,
                rows = list
            };
            ViewBag.CurrentStep = queryStr;
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        #region 审核/反审核
        [HttpPost]
        //[SupportFilter]
        public JsonResult Check(string Id)
        {
            if (!string.IsNullOrWhiteSpace(Id))
            {
                
                LianTong_ProjectContractsModel UpModel = m_BLL.m_Rep.Find(Convert.ToInt32(Id));
                int nextStep = Convert.ToInt32(UpModel.history) + 1;
                if (nextStep > Convert.ToInt32(FlowLianTongContracts.完结.GetInt()))
                {
                    return Json(JsonHandler.CreateMessage(0, Resource.CheckFail + "合同已完结"));
                }
                UpModel.history = nextStep.ToString();
                UpModel.status = dic[UpModel.history];
                if (m_BLL.m_Rep.Update(UpModel))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + Id, "成功", "审核", "合同流程");
                    return Json(JsonHandler.CreateMessage(1, Resource.CheckSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + Id + "," + ErrorCol, "失败", "审核", "合同流程");
                    return Json(JsonHandler.CreateMessage(0, Resource.CheckFail + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.CheckFail));
            }
        }

        [HttpPost]
        //[SupportFilter]
        public JsonResult UnCheck(string Id)
        {
            if (!string.IsNullOrWhiteSpace(Id))
            {
                LianTong_ProjectContractsModel UpModel = m_BLL.m_Rep.Find(Convert.ToInt32(Id));
                int nextStep = Convert.ToInt32(UpModel.history) - 1;
                if (nextStep < Convert.ToInt32(FlowLianTongContracts.送审.GetInt()))
                {
                    UpModel.history = null;
                }
                else
                {
                    UpModel.history = nextStep.ToString();
                }
                UpModel.status = "未通过";
                if (m_BLL.m_Rep.Update(UpModel))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + Id, "成功", "反审核", "信息中心");
                    return Json(JsonHandler.CreateMessage(1, Resource.UnCheckSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + Id + "," + ErrorCol, "失败", "反审核", "信息中心");
                    return Json(JsonHandler.CreateMessage(0, Resource.UnCheckFail + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.UnCheckFail));
            }
        }
        #endregion

    }
}