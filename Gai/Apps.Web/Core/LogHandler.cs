﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Apps.Common;
using Apps.DAL;
using Apps.BLL;
using Apps.Models;
using Microsoft.Practices.Unity;
using Apps.Models.Sys;
using Apps.Models.WC;
using Apps.DAL.WC;
using Apps.DAL.Sys;
using Apps.BLL.Sys;

namespace Apps.Web.Core
{
    public static class LogHandler
    {
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="oper">操作人</param>
        /// <param name="mes">操作信息</param>
        /// <param name="result">结果</param>
        /// <param name="type">类型</param>
        /// <param name="module">操作模块</param>
        public static void WriteServiceLog(string oper, string mes, string result, string type, string module)
        {
            SysConfigModel siteConfig = new SysConfigBLL().loadConfig(Utils.GetXmlMapPath("Configpath"));
            //后台管理日志开启
            if (siteConfig.logstatus == 1)
            {
                ValidationErrors errors = new ValidationErrors();
                SysLog entity = new SysLog();
                entity.KEY_Id = ResultHelper.NewId;
                entity.Operator = oper;
                entity.Message = mes;
                entity.Result = result;
                entity.Type = type;
                entity.Module = module;
                entity.CreateTime = ResultHelper.NowTime.ToString("yyyy-MM-dd HH:mm:ss");
                SysLogRepository logRepository = new SysLogRepository();
                logRepository.Create(entity);
            }
            else
            {
                return;
            }
        }

        public static void WriteWeChatLog(WC_ResponseLog model)
        {
            SysConfigModel siteConfig = new SysConfigBLL().loadConfig(Utils.GetXmlMapPath("Configpath"));
            //后台管理日志开启
            if (siteConfig.logstatus == 1)
            {
                WC_ResponseLog entity = new WC_ResponseLog();
            entity.KEY_Id = ResultHelper.NewId;
            entity.OpenId = model.OpenId;
            entity.RequestType = model.RequestType;
            entity.RequestContent = model.RequestContent;
            entity.ResponseType = model.ResponseType;
            entity.ResponseContent = model.ResponseContent;
            entity.CreateBy = "";
            entity.CreateTime = ResultHelper.NowTime.ToString("yyyy-MM-dd HH:mm:ss");
            entity.ModifyBy = "";
            entity.ModifyTime = ResultHelper.NowTime.ToString("yyyy-MM-dd HH:mm:ss");
            WC_ResponseLogRepository logRepository = new WC_ResponseLogRepository();
            logRepository.Create(entity);
        }
            else
            {
                return;
            }
        }

    }
}