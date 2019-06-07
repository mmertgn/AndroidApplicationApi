using AndroidApplicationApi.DataModels;
using AndroidApplicationApi.Models.RequestModel;
using AndroidApplicationApi.Repository;
using AndroidApplicationData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace AndroidApplicationApi.Controllers
{
  public class SuController : ApiController
  {
    StringBuilder brokenRules;
    #region validation
    private bool parameterControl(int kullaniciId, string toplamBardak)
    {
      if (string.IsNullOrEmpty(kullaniciId.ToString()) || string.IsNullOrEmpty(toplamBardak.ToString()))
      {
        brokenRules = new StringBuilder();
        brokenRules.AppendLine("Lütfen tüm alanları doldurun.");
        return true;
      }
      return false;
    }
    #endregion
    [HttpGet]
    public ResponseModel SuEkle(string request)
    {
      ResponseModel result = new ResponseModel();
      SuRepository repo = new SuRepository();
      try
      {
        bool saveResult = false;
        string jsonObject;
        SuModel viewModel = JsonConvert.DeserializeObject<SuModel>(request);
        bool validate = parameterControl(viewModel.KullaniciId, viewModel.ToplamBardak.ToString());
        if (validate)
        {
          result.Status = false;
          result.Data = null;
          result.Message = brokenRules.ToString();
          return result;
        }

        Su su = new Su();
        su.KullaniciId = viewModel.KullaniciId;
        su.GunlukToplamBardak = viewModel.ToplamBardak;
        su.Tarih = viewModel.Tarih;
        su.OlusturmaTarihi = DateTime.Now;

        saveResult = repo.Add(su);
        if (saveResult == false)
        {
          result.Data = null;
          result.Message = "Kayıt işlemi gerçekleştirilemedi.";
          result.Status = false;

          return result;
        }

        jsonObject = Newtonsoft.Json.JsonConvert.SerializeObject(su);
        result.Data = jsonObject;
        result.Message = "Kayıt işlemi başarılı.";
        result.Status = true;

        return result;

      }
      catch (Exception ex)
      {
        result.Data = null;
        result.Message = ex.Message;
        result.Status = false;
      }
      return result;
    }

    [HttpGet]
    public ResponseModel SuGetir(string request)
    {
      ResponseModel result = new ResponseModel();
      SuRepository repo = new SuRepository();
      List<Su> resultModel = new List<Su>();
      Su su = new Su();

      try
      {
        string jsonObject = "";
        SuGetirModel requestModel = JsonConvert.DeserializeObject<SuGetirModel>(request);
        var list = repo.GetList(x => x.KullaniciId == requestModel.KullaniciId).GroupBy(x => x.Tarih).Select(a => new Su
        {
          GunlukToplamBardak = a.Sum(x => x.GunlukToplamBardak),
          Tarih = a.Key
        }).OrderByDescending(x => x.OlusturmaTarihi).ToList();
        if (list.Count > 0)
        {
          foreach (var itemData in list)
          {
            su = new Su();
            //su.Id = itemData.Id;
            //su.KullaniciId = itemData.KullaniciId;
            //su.OlusturmaTarihi = itemData.OlusturmaTarihi;
            su.Tarih = itemData.Tarih;
            su.GunlukToplamBardak = itemData.GunlukToplamBardak;

            //Add List
            resultModel.Add(su);
          }

          jsonObject = Newtonsoft.Json.JsonConvert.SerializeObject(resultModel);

        }
        else
        {
          result.Data = null;
          result.Message = "Veri bulunamadı";
          result.Status = false;

          return result;

        }


        result.Data = jsonObject;
        result.Message = "Liste başarıyla getirildi";
        result.Status = true;

        return result;
      }
      catch (Exception ex)
      {
        result.Data = null;
        result.Message = ex.Message;
        result.Status = false;
        throw;
      }
    }


    [HttpGet]
    public string Hello()
    {
      var selam = "selam ben objeyim";

      var result = Newtonsoft.Json.JsonConvert.SerializeObject(selam);
      return result;
    }

  }

}

