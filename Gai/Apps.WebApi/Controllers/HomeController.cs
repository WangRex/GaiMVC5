using Apps.BLL.Sys;
using Apps.Common;
using Apps.Models;
using Apps.Models.Sys;
using Microsoft.Practices.Unity;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Web.Mvc;

namespace Apps.WebApi.Controllers
{
    public class HomeController : Controller
    {
        public SysModuleBLL m_BLL ;
        public SysModuleOperateBLL operateBLL;
        ValidationErrors errors = new ValidationErrors();

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            //第一次运行时候，初始化数据库的表
            InitCurrentApiInterface();
            return View();
        }



        /// <summary>
        /// 将当前所有API接口添加到数据
        /// </summary>
        private void InitCurrentApiInterface()
        {
            m_BLL = new SysModuleBLL();
            operateBLL = new SysModuleOperateBLL();
            //插入一个约定树根数据
            SysModule rootModel = m_BLL.m_Rep.Find(x => x.EnglishName == "ApiInterfaceAuth");
            if (rootModel == null)
            {
                SysModule model = new SysModule()
                {
                    Name = "Api接口授权",
                    EnglishName = "ApiInterfaceAuth",
                    ParentId = "0",
                    Url = "",
                    Iconic = "fa fa-television",
                    Enable = "true",
                    Remark = "Api接口授权",
                    Sort ="1",
                    CreatePerson = "Admin",
                    CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    IsLast = "false"
                };
                m_BLL.m_Rep.Create(model);
            }
            //把控制器当成URL，把Aciton当成操作码插入到数据表做为权限设置，类似之前的权限系统
            //获得API管理器
            Collection<ApiDescription> apiColl = GlobalConfiguration.Configuration.Services.GetApiExplorer().ApiDescriptions;
            ILookup<HttpControllerDescriptor, ApiDescription> apiGroups = apiColl.ToLookup(api => api.ActionDescriptor.ControllerDescriptor);

            foreach (var group in apiGroups)
            {

                string controllerName = group.Key.ControllerName;
                //----------插入控制器
                rootModel = m_BLL.m_Rep.Find(a => a.Name == controllerName);
                if (rootModel == null)
                {
                    SysModule model = new SysModule()
                    {
                        Name = controllerName,
                        EnglishName = "",
                        ParentId = "0",
                        Url = controllerName,
                        Iconic = "fa fa-television",
                        Enable = "true",
                        Remark = "Api接口授权",
                        Sort = "1",
                        CreatePerson = "Admin",
                        CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        IsLast = "true"
                    };
                    m_BLL.m_Rep.Create(model);
                }
                //-----------插入Action   
                foreach (var m in group)
                {
                    string actionName = m.ActionDescriptor.ActionName;
                    SysModuleOperate model = operateBLL.m_Rep.Find(a => a.Name == controllerName + actionName);
                    if (model == null)
                    {
                        model = new SysModuleOperate();
                        model.Name = controllerName + actionName;
                        model.KeyCode = actionName;
                        model.ModuleId = controllerName;
                        model.IsValid = "true";
                        model.Sort = "0";
                        operateBLL.m_Rep.Create(model);
                    }

                }
            }
        }
    }
}
