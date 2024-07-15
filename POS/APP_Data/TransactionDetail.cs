//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace POS.APP_Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class TransactionDetail
    {
        public TransactionDetail()
        {
            this.DeleteLogs = new HashSet<DeleteLog>();
            this.PackagePurchasedInvoices = new HashSet<PackagePurchasedInvoice>();
            this.PurchaseDetailInTransactions = new HashSet<PurchaseDetailInTransaction>();
            this.SPDetails = new HashSet<SPDetail>();
            this.Tickets = new HashSet<Ticket>();
        }
    
        public long Id { get; set; }
        public string TransactionId { get; set; }
        public Nullable<long> ProductId { get; set; }
        public Nullable<int> Qty { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal TaxRate { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<decimal> ConsignmentPrice { get; set; }
        public Nullable<bool> IsConsignmentPaid { get; set; }
        public Nullable<bool> IsFOC { get; set; }
        public Nullable<decimal> SellingPrice { get; set; }
    
        public virtual ICollection<DeleteLog> DeleteLogs { get; set; }
        public virtual ICollection<PackagePurchasedInvoice> PackagePurchasedInvoices { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<PurchaseDetailInTransaction> PurchaseDetailInTransactions { get; set; }
        public virtual ICollection<SPDetail> SPDetails { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual Transaction Transaction { get; set; }
    }
}
