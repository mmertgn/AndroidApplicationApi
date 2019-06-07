using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AndroidApplicationApi.Models.RequestModel
{
  public class VucutKitleModel
  {
    public int kullaniciId { get; set; }
    public DateTime tarih { get; set; }
    public int  boy { get; set; }
    public int kilo { get; set; }
  }
}