using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AndroidApplicationApi.Models.RequestModel
{
  public class TansiyonGetirModel
  {
    public int KullaniciId { get; set; }
    public DateTime Tarih { get; set; }
  }
}