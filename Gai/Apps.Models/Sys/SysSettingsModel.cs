﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由T4模板自动生成
//	   生成时间 2012-12-04 20:40:10 by App
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;

namespace Apps.Models.Sys
{

    public partial class SysSettings
    {
        [Key]
        public int Id { get; set; }
        public string KEY_Id { get; set; }
        [Display(Name = "名称")]
        public string Name { get; set; }
        [Display(Name = "类型")]
        public string Type { get; set; }
        [Display(Name = "参数")]
        public string Parameter { get; set; }
        [Display(Name = "说明")]
        public string ReMark { get; set; }


    }
}

