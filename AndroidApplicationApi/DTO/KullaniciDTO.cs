﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AndroidApplicationApi.DTO
{
  public class KullaniciDTO
  {
    public int Id { get; set; }
    public string Adi { get; set; }
    public string Soyadi { get; set; }
    public string Email { get; set; }
    public string Sifre { get; set; }
    public DateTime DogumTarihi { get; set; }
    public string DiyabetTipi { get; set; }
    public DateTime TeshisKondoguTarih { get; set; }
    public string il { get; set; }
    public string ilce { get; set; }
    public DateTime OlusturmaTarihi { get; set; }
    public string Cinsiyet { get; set; }
  }
}