using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AndroidApplicationApi.Models.RequestModel
{
  public class KanSekeriModel
  {
    public int KullaniciId { get; set; }
    public DateTime Tarih { get; set; }
    public string Zaman { get; set; }
    public int KanSekeriDegeri { get; set; }

  }
}