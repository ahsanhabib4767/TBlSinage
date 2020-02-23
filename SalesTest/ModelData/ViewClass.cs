using SalesTest.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SalesTest.ModelData
{
    public class ViewClass
    {
    }
    public class ConItem
    {
        public int Id { get; set; }
        public int ItemNo { get; set; }
        public int ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public Nullable<int> ItemType { get; set; }
        public Nullable<int> ItemTypeCode { get; set; }
        public Nullable<int> ItemUnitCode { get; set; }
        public int PlantCode { get; set; }
        public Nullable<int> ItemMachineCode { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public string TaxFlag { get; set; }
        public int ConvertValue { get; set; }
        public string UseFor { get; set; }
        public string Show { get; set; }
  

    }
    public  class RequestVM
    {
        public int id { get; set; }
        public Nullable<int> ReqID { get; set; }
        public string RegionName { get; set; }
        public string AreaName { get; set; }
        public string CeAreaName { get; set; }
        public string DbName { get; set; }
        public string cluster { get; set; }
        public string PSRName { get; set; }
        public string SubRoute { get; set; }
        public Nullable<int> OutletCode { get; set; }
        public string OutLetName { get; set; }
        public string Address { get; set; }
        public string OwnerName { get; set; }
        public string ContactNum { get; set; }
        public string ContactNumTwo { get; set; }
        public Nullable<bool> HaveVisicooler { get; set; }
        public string ChannelName { get; set; }
        public string OutletCatagoryName { get; set; }
        public string PicUrl { get; set; }
        [Required(ErrorMessage = "please select signage type")]
        public Nullable<int> sinagetype { get; set; }
        public string sinage { get; set; }
        public string currentsignage { get; set; }

        public Nullable<int> otherbrand { get; set; }
        public string otherbrandd { get; set; }
        public string signageheight { get; set; }
        public string signagewidth { get; set; }
        public string remarks { get; set; }
        public string AppFlag1 { get; set; }
        public string AppFlag2 { get; set; }
        public string AppFlag3 { get; set; }
        public string AppFlag4 { get; set; }
        public string AppFlag5 { get; set; }
        public string AppFlag6 { get; set; }
        public string SE { get; set; }
        public string ASM { get; set; }
        public string RSM { get; set; }
        public string HOS { get; set; }
        public string BM { get; set; }
        public HttpPostedFileBase UploadImage{ get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string SalesVolume { get; set; }
        //  public string CreateBy { get; set; }
        // public Nullable<System.DateTime> CreateDate { get; set; }

    }
    public class RequestVisicoolerVM
    {
        public int id { get; set; }
        public Nullable<int> ReqID { get; set; }
        public string RegionName { get; set; }
        public string AreaName { get; set; }
        public string CeAreaName { get; set; }
        public string DbName { get; set; }
        public string cluster { get; set; }
        public string PSRName { get; set; }
        public string SubRoute { get; set; }
        public Nullable<int> OutletCode { get; set; }
        public string OutLetName { get; set; }
        public string Address { get; set; }
        public string OwnerName { get; set; }
        public string ContactNum { get; set; }
        public string ContactNumTwo { get; set; }
        public Nullable<bool> HaveVisicooler { get; set; }
        public string ChannelName { get; set; }
        public string OutletCatagoryName { get; set; }
        public string visicooleNeed { get; set; }
        public string remarks { get; set; }
        public string AppFlag1 { get; set; }
        public string AppFlag2 { get; set; }
        public string AppFlag3 { get; set; }
        public string AppFlag4 { get; set; }
        public string AppFlag5 { get; set; }
        public string AppFlag6 { get; set; }
        public string SE { get; set; }
        public string ASM { get; set; }
        public string RSM { get; set; }
        public string MEM { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string NightGuard { get; set; }
        public Nullable<int> VisiSize { get; set; }
        public string TotalCase { get; set; }
       // public HttpPostedFileBase[] UploadImage { get; set; }
        public HttpPostedFileBase UploadImage1 { get; set; }
        public HttpPostedFileBase UploadImage2 { get; set; }
        public HttpPostedFileBase UploadImage3 { get; set; }
        public string VisiApplicationImg { get; set; }
        public string TradeLicenseImg { get; set; }
        public string VoterIDImg { get; set; }
    }
    public class VendorConfirmM
    {
        public int id { get; set; }
        public Nullable<int> VenReqID { get; set; }
        public Nullable<int> VenID { get; set; }
        public Nullable<int> ReqID { get; set; }
        public string RegionName { get; set; }
        public string AreaName { get; set; }
        public string CeAreaName { get; set; }
        public string DbName { get; set; }
        public string cluster { get; set; }
        public string PSRName { get; set; }
        public string SubRoute { get; set; }
        public Nullable<int> OutletCode { get; set; }
        public string OutLetName { get; set; }
        public string Address { get; set; }
        public string OwnerName { get; set; }
        public string ContactNum { get; set; }
        public string ContactNumTwo { get; set; }
        public Nullable<bool> HaveVisicooler { get; set; }
        public string ChannelName { get; set; }
        public string OutletCatagoryName { get; set; }
        public Nullable<int> sinagetype { get; set; }
        public string currentsignage { get; set; }
        public string sinage { get; set; }
        public Nullable<int> otherbrand { get; set; }
        public string otherbrandd { get; set; }
        public string signageheight { get; set; }
        public string signagewidth { get; set; }
        public string signageheightNew { get; set; }
        public string signagewidthNew { get; set; }
        public Nullable<System.DateTime> dateLimit { get; set; }
        public string msg { get; set; }
        public string remarks { get; set; }
        public string AppFlag1 { get; set; }
        public string AppFlag2 { get; set; }
        public string AppFlag3 { get; set; }
        public string OrderBy { get; set; }
        public string SE { get; set; }
        public Nullable<System.DateTime> dateLimitfinal { get; set; }
        public string PicUrl { get; set; }
        public string PicUrl2 { get; set; }
        public string feedback { get; set; }
        
        public HttpPostedFileBase UploadImageb { get; set; }
        public string SEfeedback { get; set; }
        public virtual tblVendor tblVendor { get; set; }
        public Nullable<System.DateTime> installDate { get; set; }
    }
    public class UserReqVM
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        [Required(ErrorMessage = "User Name is Required.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is Required.")]
        public string UserPass { get; set; }
        public string UserPin { get; set; }
        public Nullable<int> RoleId { get; set; }
        public string MobileNo { get; set; }
        public string UserStatus { get; set; }
        public Nullable<int> PlantNo { get; set; }
        [Required(ErrorMessage = "Email is Required.")]
        public string Email { get; set; }
        public string LoginType { get; set; }
        public Nullable<int> ActiveSession { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
    }
   
    public class ReceiveVM
    {
        public int PlantID { get; set; }
        public int ReqRecID { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public Nullable<int> ReqID { get; set; }
        public Nullable<System.DateTime> RecDate { get; set; }
        public Nullable<int> TypeCode { get; set; }
        public string UserNote { get; set; }
        public string AppBy { get; set; }
        public int DeptID { get; set; }
        public int PayType { get; set; }
        public int PayMode { get; set; }
        //public Nullable<System.DateTime> AppDate { get; set; }
        public string AppFlag { get; set; }
        //public string AppRemarks { get; set; }
        //public string AppBy2 { get; set; }
        //public Nullable<System.DateTime> AppDate2 { get; set; }
        public string AppFlag2 { get; set; }
        public string AppRemarks2 { get; set; }
        // public string CreateBy { get; set; }
        // public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<decimal> VATPer { get; set; }
        public Nullable<decimal> TAXPer { get; set; }
        public Nullable<decimal> DiscountPer { get; set; }
        public Nullable<decimal> DiscountAmt { get; set; }
        public Nullable<int> CashCheque { get; set; }
        public Nullable<int> PaymentType { get; set; }
        public string MultipleSup { get; set; }
       
    }
    public  class ApprovalVM
    {
        public int PlantID { get; set; }
        public int RefNo { get; set; }
        public int AppID { get; set; }
        public string AppCode { get; set; }
        public int AppType { get; set; }
        public string FirstApp { get; set; }
        public string SecondApp { get; set; }
        public string FirstStatus { get; set; }
        public string SecondStatus { get; set; }
        public Nullable<System.DateTime> FirstDate { get; set; }
        public Nullable<System.DateTime> SecondDate { get; set; }
        public string FirstRemarks { get; set; }
        public string SecondRemarks { get; set; }
    }
    public  class TransactionVM
    {
        public int PlantID { get; set; }
        public int ReceivedTranNo { get; set; }
        public Nullable<int> LocalForeign { get; set; }
        public Nullable<System.DateTime> TranDate { get; set; }
        public Nullable<int> RefOrderNo { get; set; }
        public Nullable<int> RefInvoice { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public string IndentNo { get; set; }
        public Nullable<int> LCNo { get; set; }
        public Nullable<decimal> TotalQty { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        // public string CreateBy { get; set; }
        // public Nullable<System.DateTime> CreateDate { get; set; }
        
    }
    public class spRequestData
    {

        public int ReqID { get; set; }

        public int PlantID { get; set; }

        public Nullable<int> DeptID { get; set; }

        public Nullable<int> CustomerID { get; set; }

        public string RefNo { get; set; }

        public int ItemID { get; set; }

        public Nullable<int> Quantity { get; set; }

        public string ItemName { get; set; }

        public Nullable<int> ItemUnitCode { get; set; }

        public int ConvertValue { get; set; }

        public Nullable<decimal> UnitPrice { get; set; }

        public string DeptName { get; set; }

        public Nullable<decimal> LastUnitPrice { get; set; }

        public Nullable<int> StockQty { get; set; }

    }
    public class IssueVm
    {
          public int PlantID { get; set; }
        public int TrNo { get; set; }
        public Nullable<System.DateTime> TrDate { get; set; }
        public Nullable<int> RefOrderNo { get; set; }
        public Nullable<int> BeneficiaryID { get; set; }
        public Nullable<int> DeptID { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> ReceivedBy { get; set; }
        public string ReceiverName { get; set; }
        public string Status { get; set; }
        //public string CreateBy { get; set; }
        //public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> TotQty { get; set; }
        public Nullable<decimal> TotAmt { get; set; }
       

    }
    public class TranVM
    {
        public int PlantID { get; set; }
        public Nullable<System.DateTime> TrDate { get; set; }
        public int TrNo { get; set; }
        public Nullable<int> TranType { get; set; }
        public Nullable<int> RefNo { get; set; }
        public string Remarks { get; set; }
        //public string CreateBy { get; set; }
        //public Nullable<System.DateTime> CreateDate { get; set; }
       

    }
    public class ProductVM
    {
        public int ItemNo { get; set; }
        public int ItemCode { get; set; }
        public string ItemName { get; set; } 
        public string ItemDescription { get; set; }
        public Nullable<int> ItemType { get; set; }
        public Nullable<int> ItemTypeCode { get; set; }
        public Nullable<int> ItemUnitCode { get; set; }
        public int PlantCode { get; set; }
        public Nullable<int> ItemMachineCode { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        //public string TaxFlag { get; set; }
        //public int ConvertValue { get; set; }
        //public string UseFor { get; set; }
        public string Show { get; set; }
        //public string CreateBy { get; set; }
        //public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> AlmariCode { get; set; }
        public string AlmariDesc { get; set; }
        public Nullable<int> RowID { get; set; }
        public Nullable<int> RacID { get; set; }
        public Nullable<int> BinID { get; set; }
        public string ItemSize { get; set; }
        public Nullable<int> ItemSource { get; set; }
        public Nullable<int> UnitCode { get; set; }
        public Nullable<int> TrackID { get; set; }
        public Nullable<int> ETrack { get; set; }
        public string Location { get; set; }

    }
    public class ItemVM
    {
        public int Id { get; set; }
        public int ItemNo { get; set; }
        public int ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public Nullable<int> ItemType { get; set; }
        public Nullable<int> ItemTypeCode { get; set; }
        public Nullable<int> ItemUnitCode { get; set; }
        public int PlantCode { get; set; }
        public Nullable<int> ItemMachineCode { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public string TaxFlag { get; set; }
        public int ConvertValue { get; set; }
        public string UseFor { get; set; }
        public string Show { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string Location { get; set; }

    }
    public class ItemVMInfo
    {
        public int PlantID { get; set; }
        public int ItemID { get; set; }
        public string ItemCode { get; set; }
        public Nullable<int> ItemType { get; set; }
        public Nullable<int> ItemUse { get; set; }
        public Nullable<int> MachineID { get; set; }
        public Nullable<int> AlmariCode { get; set; }
        public string AlmariDesc { get; set; }
        public Nullable<int> RowID { get; set; }
        public Nullable<int> RacID { get; set; }
        public Nullable<int> BinID { get; set; }
        public string ItemSize { get; set; }
        public Nullable<int> ItemSource { get; set; }
        public Nullable<int> UnitCode { get; set; }
        public Nullable<int> TrackID { get; set; }
        public Nullable<int> ETrack { get; set; }

    }
    public class FrdSupplierVM
    {

        public int SupplierID { get; set; }
        [Required(ErrorMessage = "please enter supplier code")]
        public string SupplierCode { get; set; }
        [Required(ErrorMessage = "please enter supplier name")]
        public string SupplierName { get; set; }
        [Required(ErrorMessage = "please enter supplier address")]
        public string SupplierAddress { get; set; }
        [Required(ErrorMessage = "please enter phone number")]
        public string PhoneNumber { get; set; }

        public string MobileNumber { get; set; }
        [Required(ErrorMessage = "please enter Email address")]
        public string EmailAddress { get; set; }

        public string Status { get; set; }

        public Nullable<int> ZoneCode { get; set; }

        public Nullable<int> BranchCode { get; set; }

        public string MultiFlag { get; set; }

        public string Remarks { get; set; }
        [Required(ErrorMessage = "please enter contact person name")]
        public string ContactPerson { get; set; }
        [Required(ErrorMessage = "please enter vat reg name")]
        public string TINNo { get; set; }

        public string BINNo { get; set; }
        [Required(ErrorMessage = "please enter VAT Registration Number if None press any number")]
        public string VATRegNo { get; set; }

        public string CreateBy { get; set; }

        public Nullable<System.DateTime> CraeteDate { get; set; }

    }
}