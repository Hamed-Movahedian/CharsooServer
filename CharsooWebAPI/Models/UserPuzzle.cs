//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CharsooWebAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserPuzzle
    {
        public int ID { get; set; }
        public int ClientID { get; set; }
        public int CreatorID { get; set; }
        public string Clue { get; set; }
        public string Content { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public System.DateTime LastUpdate { get; set; }
        public Nullable<int> Rate { get; set; }
        public Nullable<int> PlayCount { get; set; }
    
        public virtual Category Category { get; set; }
    }
}