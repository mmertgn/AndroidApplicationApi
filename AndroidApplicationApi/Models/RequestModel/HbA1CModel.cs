using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AndroidApplicationApi.Models.RequestModel
{
  public class HbA1CModel
  {
    public int KullaniciId { get; set; }
    public DateTime Tarih { get; set; }
    public string HbA1C { get; set; }
    public string Yorum { get; set; }

  }
}