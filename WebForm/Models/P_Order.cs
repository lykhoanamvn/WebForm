//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebForm.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class P_Order
    {
        public int id { get; set; }
        public Nullable<int> id_agent { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public string Address_Description { get; set; }
        public string payment { get; set; }
        public Nullable<int> d_status { get; set; }
        public string descrip { get; set; }
    }
}
