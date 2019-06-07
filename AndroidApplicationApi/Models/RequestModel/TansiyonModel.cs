using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AndroidApplicationApi.Models.RequestModel
{
  public class TansiyonModel
  {
    public int KullaniciId { get; set; }
    public int BuyukTansiyon { get; set; }
    public int KucukTansiyon { get; set; }
    public DateTime Tarih { get; set; }
  }
}