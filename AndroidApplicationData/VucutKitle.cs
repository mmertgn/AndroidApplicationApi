//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AndroidApplicationData
{
    using System;
    using System.Collections.Generic;
    
    public partial class VucutKitle
    {
        public int Id { get; set; }
        public int KullaniciId { get; set; }
        public Nullable<System.DateTime> Tarih { get; set; }
        public Nullable<int> Boy { get; set; }
        public Nullable<int> Kilo { get; set; }
        public Nullable<double> VucutKitleIndexi { get; set; }
        public Nullable<System.DateTime> OlusturmaTarihi { get; set; }
    
        public virtual Kullanicilar Kullanicilar { get; set; }
    }
}
