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
  public class TansiyonController : ApiController
  {

    StringBuilder brokenRules;
    #region validation
    private bool parameterControl(int kullaniciId, string buyukTansiyon, string kucukTansiyon, DateTime tarih)
    {

      if (string.IsNullOrEmpty(kullaniciId.ToString()) || string.IsNullOrEmpty(buyukTansiyon.ToString()) || string.IsNullOrEmpty(kucukTansiyon.ToString()) || string.IsNullOrEmpty(tarih.ToString()))
      {
        brokenRules = new StringBuilder();
        brokenRules.AppendLine("Lütfen tüm alanları doldurun.");
        return true;
      }
      return false;
    }
    #endregion
    [HttpGet]
    public ResponseModel TansiyonEkle(string request)
    {
      ResponseModel result = new ResponseModel();
      TansiyonRepository repo = new TansiyonRepository();
      try
      {

        bool saveResult = false;
        string jsonObject;
        TansiyonModel viewModel = JsonConvert.DeserializeObject<TansiyonModel>(request);
        bool validate = parameterControl(viewModel.KullaniciId, viewModel.BuyukTansiyon.ToString(), viewModel.KucukTansiyon.ToString(), viewModel.Tarih);
        if (validate)
        {
          result.Status = false;
          result.Data = null;
          result.Message = brokenRules.ToString();
          return result;
        }

        Tansiyon tansiyon = new Tansiyon();
        tansiyon.BuyukTansiyon = viewModel.BuyukTansiyon;
        tansiyon.KucukTansiyon = viewModel.KucukTansiyon;
        tansiyon.Tarih = viewModel. Tarih;
        tansiyon.KullaniciId = viewModel.KullaniciId;
        tansiyon.OlusturmaTarihi = DateTime.Now;

        saveResult = repo.Add(tansiyon);
        if (saveResult == false)
        {
          result.Data = null;
          result.Message = "Kayıt işlemi gerçekleştirilemedi.";
          result.Status = false;
          return result;
        }

        jsonObject = Newtonsoft.Json.JsonConvert.SerializeObject(viewModel);
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
    public ResponseModel TansiyonGetir(string request)
    {
      ResponseModel result = new ResponseModel();
      TansiyonRepository repo = new TansiyonRepository();
      List<Tansiyon> resultModel = new List<Tansiyon>();
      Tansiyon tansiyon = new Tansiyon();

      try
      {
        string jsonObject = "";
        TansiyonGetirModel requestModel = JsonConvert.DeserializeObject<TansiyonGetirModel>(request);
        var list = repo.GetList(x => x.KullaniciId == requestModel.KullaniciId).OrderByDescending(x => x.OlusturmaTarihi).ToList();
        if (list.Count > 0)
        {
          foreach (var itemData in list)
          {
            tansiyon = new Tansiyon();
            tansiyon.Id = itemData.Id;
            tansiyon.KullaniciId = itemData.KullaniciId;
            tansiyon.OlusturmaTarihi = itemData.OlusturmaTarihi;
            tansiyon.Tarih = itemData.Tarih;
            tansiyon.KucukTansiyon = itemData.KucukTansiyon;
            tansiyon.BuyukTansiyon = itemData.BuyukTansiyon;

            //Add List
            resultModel.Add(tansiyon);
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
  }
}
