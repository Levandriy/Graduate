//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Graduate.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Journal
    {
        public System.Guid Id { get; set; } = Guid.NewGuid();
        public System.DateTime Date { get; set; } = DateTime.Now;
        public System.Guid Student { get; set; } = App.user;
        public byte[] File { get; set; }
        public string Ext { get; set; }
        public Nullable<int> Mark { get; set; }
        public System.Guid Lab { get; set; }
    
        public virtual Students Students { get; set; }
        public virtual Materials Materials { get; set; }
    }
}