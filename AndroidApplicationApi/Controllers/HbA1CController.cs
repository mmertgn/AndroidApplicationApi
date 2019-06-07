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
  public class HbA1CController : ApiController
  {

    StringBuilder brokenRules;

    private bool checkParameters(int kullaniciId, DateTime tarih, string hbA1C)
    {
      if (string.IsNullOrEmpty(kullaniciId.ToString()) || string.IsNullOrEmpty(tarih.ToString()) || string.IsNullOrEmpty(hbA1C.ToString()))
      {
        brokenRules = new StringBuilder();
        brokenRules.AppendLine("Lütfen tüm alanları doldurun.");
        return true;
      }
      return false;

    }

    [HttpGet]
    public ResponseModel HbA1CEkle(string request)
    {
      ResponseModel result = new ResponseModel();
      HbA1CRepository repo = new HbA1CRepository();
      HbA1CModel viewModel = JsonConvert.DeserializeObject<HbA1CModel>(request);
      try
      {
        bool saveResult = false;
        var jsonObject = string.Empty;


        bool parameterValidate = checkParameters(viewModel.KullaniciId, viewModel.Tarih, viewModel.HbA1C);
        if (parameterValidate)
        {
          result.Data = null;
          result.Message = brokenRules.ToString();
          result.Status = false;
        }

        HbA1c model = new HbA1c();
        model.KullaniciId = viewModel.KullaniciId;
        model.HbA1c1 = viewModel.HbA1C;
        model.Tarih = viewModel.Tarih;
        model.Yorum = viewModel.Yorum;
        model.OlusturmaTarihi = DateTime.Now;

        saveResult = repo.Add(model);

        if (saveResult == false)
        {
          result.Data = null;
          result.Message = "Kayıt işlemi gerçekleştirilemedi.";
          result.Status = false;
          return result;
        }

        jsonObject = Newtonsoft.Json.JsonConvert.SerializeObject(model);
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
        throw;
      }
      return result;

    }

    [HttpGet]
    public ResponseModel HbA1CGetir(string request)
    {
      ResponseModel result = new ResponseModel();
      HbA1CRepository repo = new HbA1CRepository();
      List<HbA1c> resultModel = new List<HbA1c>();
      HbA1c hbA1C = new HbA1c();
      try
      {
        string jsonObject = "";
        HbA1CGetirModel requestModel = JsonConvert.DeserializeObject<HbA1CGetirModel>(request);
        var list = repo.GetList(x => x.KullaniciId == requestModel.KullaniciId).OrderByDescending(x=>x.OlusturmaTarihi).ToList();

        if (list.Count > 0)
        {
          foreach (var itemData in list)
          {
            hbA1C = new HbA1c();
            hbA1C.Id = itemData.Id;
            hbA1C.HbA1c1 = itemData.HbA1c1;
            hbA1C.KullaniciId = itemData.KullaniciId;
            hbA1C.OlusturmaTarihi = itemData.OlusturmaTarihi;
            hbA1C.Tarih = itemData.Tarih;
            hbA1C.Yorum = itemData.Yorum;

            resultModel.Add(hbA1C);
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
      catch (Exception)
      {

        throw;
      }

    }

  }
}
