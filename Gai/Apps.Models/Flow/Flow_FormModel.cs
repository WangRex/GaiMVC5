﻿using System;
using System.ComponentModel.DataAnnotations;
using Apps.Models;
using System.Collections.Generic;
namespace Apps.Models.Flow
{
    public partial class Flow_Form
    {
        [Key]
        public int Id { get; set; }
        public string KEY_Id { get; set; }
        [MaxWordsExpression(100)]
        [Display(Name = "表单名称")]
        public string Name { get; set; }

        [MaxWordsExpression(500)]
        [Display(Name = "说明")]
        public string Remark { get; set; }

        [MaxWordsExpression(2000)]
        [Display(Name = "使用部门")]
        public string UsingDep { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "分类")]
        public string TypeId { get; set; }

        public  string TypeName { get; set; }
        [Display(Name = "是否启用")]
        public string State { get; set; }

        [Display(Name = "创建时间")]
        public string CreateTime { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "AttrA")]
        public string AttrA { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "AttrB")]
        public string AttrB { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "AttrC")]
        public string AttrC { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "AttrD")]
        public string AttrD { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "AttrE")]
        public string AttrE { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "AttrF")]
        public string AttrF { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "AttrG")]
        public string AttrG { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "AttrH")]
        public string AttrH { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "AttrI")]
        public string AttrI { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "AttrJ")]
        public string AttrJ { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "AttrK")]
        public string AttrK { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "AttrL")]
        public string AttrL { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "AttrM")]
        public string AttrM { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "AttrN")]
        public string AttrN { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "AttrO")]
        public string AttrO { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "AttrP")]
        public string AttrP { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "AttrQ")]
        public string AttrQ { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "AttrR")]
        public string AttrR { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "AttrS")]
        public string AttrS { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "AttrT")]
        public string AttrT { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "AttrU")]
        public string AttrU { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "AttrV")]
        public string AttrV { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "AttrW")]
        public string AttrW { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "AttrX")]
        public string AttrX { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "AttrY")]
        public string AttrY { get; set; }

        [MaxWordsExpression(50)]
        [Display(Name = "AttrZ")]
        public string AttrZ { get; set; }

        public string HtmlForm { get; set; }

        public List<Flow_FormAttr> attrList { get; set; }
        public List<Flow_Step> stepList { get; set; }

    }
}
