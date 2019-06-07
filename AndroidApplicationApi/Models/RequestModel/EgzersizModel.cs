using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AndroidApplicationApi.Models.RequestModel
{
  public class EgzersizModel
  {
    public int KullaniciId { get; set; }
    public string EgzersizTipi { get; set; }
    public int Sure { get; set; } // süre
    public DateTime Tarih { get; set; }
  }
}