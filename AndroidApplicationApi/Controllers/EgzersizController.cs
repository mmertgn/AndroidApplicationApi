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
  public class EgzersizController : ApiController
  {
    StringBuilder brokenRules;

    private bool parameterControl(int KullaniciId, string Sure, string EgzersizTipi, DateTime Tarih)
    {
      if (string.IsNullOrEmpty(KullaniciId.ToString()) || string.IsNullOrEmpty(Sure.ToString()) || string.IsNullOrEmpty(EgzersizTipi.ToString()) || string.IsNullOrEmpty(Tarih.ToString()))
      {
        brokenRules = new StringBuilder();
        brokenRules.AppendLine("Lütfen tüm alanları doldurun.");
        return true;
      }
      return false;
    }
    [HttpGet]
    public ResponseModel EgzersizEkle(string request)
    {
      ResponseModel result = new ResponseModel();
      EgzersizRepository repo = new EgzersizRepository();
      try
      {

        bool saveResult = false;
        string jsonObject;
        EgzersizModel viewModel = JsonConvert.DeserializeObject<EgzersizModel>(request);
        bool validate = parameterControl(viewModel.KullaniciId, viewModel.Sure.ToString(), viewModel.EgzersizTipi.ToString(), viewModel.Tarih);
        if (validate)
        {
          result.Status = false;
          result.Data = null;
          result.Message = brokenRules.ToString();
          return result;
        }

        Egzersiz egzersiz = new Egzersiz();
        egzersiz.KullaniciId = viewModel.KullaniciId;
        egzersiz.Tarih = viewModel.Tarih;
        egzersiz.Sure = viewModel.Sure;
        egzersiz.EgzersizTipi = viewModel.EgzersizTipi;
        egzersiz.OlusturmaTarihi = DateTime.Now;

        saveResult = repo.Add(egzersiz);
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
    public ResponseModel EgzersizGetir(string request)
    {
      ResponseModel result = new ResponseModel();
      EgzersizRepository repo = new EgzersizRepository();
      List<Egzersiz> resultModel = new List<Egzersiz>();
      Egzersiz egzersiz = new Egzersiz();
    
      try
      {
        string jsonObject="";
        EgzersizGetirModel requestModel = JsonConvert.DeserializeObject<EgzersizGetirModel>(request);
        var list = repo.GetList(x => x.KullaniciId == requestModel.KullaniciId).OrderByDescending(x => x.OlusturmaTarihi).ToList();
        if (list.Count>0)
        {
          foreach (var itemData in list)
          {
            egzersiz = new Egzersiz();
            egzersiz.Id = itemData.Id;
            egzersiz.KullaniciId = itemData.KullaniciId;
            egzersiz.OlusturmaTarihi = itemData.OlusturmaTarihi;
            egzersiz.Sure = itemData.Sure;
            egzersiz.Tarih = itemData.Tarih;
            egzersiz.EgzersizTipi = itemData.EgzersizTipi;

            //Add List
            resultModel.Add(egzersiz);
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
