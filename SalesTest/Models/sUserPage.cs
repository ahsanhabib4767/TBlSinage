//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SalesTest.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class sUserPage
    {
        public int PlantNo { get; set; }
        public int PageCode { get; set; }
        public string PageDescription { get; set; }
        public string ContName { get; set; }
        public string PageName { get; set; }
        public Nullable<int> MenuID { get; set; }
        public int UserID { get; set; }
        public Nullable<int> RoleID { get; set; }
        public string Status { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
    
        public virtual sMenu sMenu { get; set; }
        public virtual sRole sRole { get; set; }
    }
}
