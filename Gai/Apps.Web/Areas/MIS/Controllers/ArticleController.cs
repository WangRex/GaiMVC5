﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由T4模板自动生成
//	   生成时间 2013-04-25 15:26:22 by HD
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Apps.BLL;
using Apps.Common;
using Apps.Models;
using Apps.Models.MIS;
using Apps.Web.Core;
using Apps.BLL.MIS;
using Apps.Core.PageControl;
using System;

namespace Apps.Web.Areas.MIS.Controllers
{
    public class ArticleController : BaseController
    {
        /// <summary>
        /// 业务层注入
        /// </summary>

        public MIS_ArticleBLL m_BLL = new MIS_ArticleBLL();


        public MIS_Article_CategoryBLL categoryBLL = new MIS_Article_CategoryBLL();
        ValidationErrors errors = new ValidationErrors();

        /// <summary>
        /// 主页
        /// </summary>
        /// <returns>视图</returns>
        [SupportFilter]
        public ActionResult Index(string querystr, string cid, int id = 1)
        {
            
            //文章类别
            List<MIS_Article_Category> category = new List<MIS_Article_Category>();
            List<MIS_Article_Category> category1 = categoryBLL.m_Rep.FindList(a => a.ParentId == "0").ToList();
            List<MIS_Article_Category> category2 = new List<MIS_Article_Category>();
            foreach (var model1 in category1)
            {
                category2 = categoryBLL.m_Rep.FindList(a => a.ParentId == model1.Id.ToString()).ToList();
                model1.clildren = new List<MIS_Article_Category>();
                foreach(var model2 in category2)
                {
                    model1.clildren.Add(model2);
                }
                category.Add(model1);
            }
            ViewBag.CategoryList = category;

            //数据
            GridPager pager = new GridPager()
            {
                page = id,
                rows = 10,
                sort = "CreateTime",
                order = "desc",
            };
            List<MIS_Article> orders = new List<MIS_Article>();
            orders = m_BLL.GetList(ref pager, "", "", true, "", 2);


            var list = new BaseList<MIS_Article>(id, 10, pager.totalRows, pager.totalPages, orders);//pager.totalPages,
            return View(list);

        }


        #region 详细

        public ActionResult Details(string id)
        {
            
            MIS_Article entity = m_BLL.m_Rep.Find(Convert.ToInt32(id));
            return View(entity);
        }

        #endregion

   

    }
}

