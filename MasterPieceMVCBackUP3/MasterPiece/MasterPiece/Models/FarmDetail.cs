//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MasterPiece.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class FarmDetail
    {
        public int FarmDetailId { get; set; }
        public int FarmId { get; set; }
        public string ImageUrl1 { get; set; }
        public string ImageUrl2 { get; set; }
        public string ImageUrl3 { get; set; }
        public string ImageUrl4 { get; set; }
        public string ImageUrl5 { get; set; }
        public string ImageUrl6 { get; set; }
        public string Description { get; set; }
        public string Capacity { get; set; }
        public Nullable<int> Price { get; set; }
        public string PriceDetails { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
    
        public virtual Farm Farm { get; set; }
    }
}
