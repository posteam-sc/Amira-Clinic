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
    
    public partial class ProductPriceChange
    {
        public long Id { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<long> ProductId { get; set; }
        public Nullable<decimal> OldPrice { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
    }
}
