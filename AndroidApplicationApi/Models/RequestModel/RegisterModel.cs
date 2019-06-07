using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AndroidApplicationApi.Models.RequestModel
{
  public class RegisterModel
  {
    public string Adi { get; set; }
    public string Soyadi { get; set; }
    public string  Email { get; set; }
    public string Sifre { get; set; }
    public string SifreTekrar { get; set; }
    public string Cinsiyet { get; set; }
    public DateTime DogumTarihi { get; set; }
    public string DiyabetTipi { get; set; }
    public DateTime TeshisKonduguTarih { get; set; }
    public string il { get; set; }
    public string  ilce { get; set; }
  }
}