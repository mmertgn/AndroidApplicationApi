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
  public class KanSekeriController : ApiController
  {
    StringBuilder brokenRules;
    private bool checkParameters(int kullaniciId, int? kanSekeriDegeri, DateTime tarih, string zaman)
    {
      if (string.IsNullOrEmpty(kullaniciId.ToString()) || string.IsNullOrEmpty(tarih.ToString()) || string.IsNullOrEmpty(zaman.ToString()) || string.IsNullOrEmpty(kanSekeriDegeri.ToString()))
      {
        brokenRules = new StringBuilder();
        brokenRules.AppendLine("Lütfen tüm alanları doldurun.");
        return true;
      }
      return false;
    }


    [HttpGet]
    public ResponseModel KanSekeriEkle(string request)
    {
      bool saveResult = false;
      var jsonObject = string.Empty;
      ResponseModel result = new ResponseModel();
      try
      {
        KanSekeriRepository repo = new KanSekeriRepository();
        KanSekeriModel requestModel = JsonConvert.DeserializeObject<KanSekeriModel>(request);
        bool parameterValidate = checkParameters(requestModel.KullaniciId, requestModel.KanSekeriDegeri, requestModel.Tarih, requestModel.Zaman);

        if (parameterValidate)
        {
          result.Data = null;
          result.Message = brokenRules.ToString();
          result.Status = false;
        }

        KanSekeri model = new KanSekeri();
        model.KullaniciId = requestModel.KullaniciId;
        model.KanSekeriDegeri = requestModel.KanSekeriDegeri;
        model.Tarih = requestModel.Tarih;
        model.Zaman = requestModel.Zaman;
       
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
    public ResponseModel KanSekeriGetir(string request)
    {
      ResponseModel result = new ResponseModel();
      KanSekeriRepository repo = new KanSekeriRepository();
      List<KanSekeri> resultModel = new List<KanSekeri>();
      

      try
      {
        string jsonObject = "";
        KanSekeriGetirModel requestModel = JsonConvert.DeserializeObject<KanSekeriGetirModel>(request);
        var list = repo.GetList(x => x.KullaniciId == requestModel.KullaniciId).OrderByDescending(x=>x.OlusturmaTarihi).ToList();
        if (list.Count > 0)
        {
          foreach (var itemData in list)
          {
            KanSekeri kanSekeri = new KanSekeri();
            kanSekeri.Id = itemData.Id;
            kanSekeri.KullaniciId = itemData.KullaniciId;
            kanSekeri.OlusturmaTarihi = itemData.OlusturmaTarihi;
            kanSekeri.KanSekeriDegeri = itemData.KanSekeriDegeri;
            kanSekeri.Tarih = itemData.Tarih;
            kanSekeri.Zaman = itemData.Zaman;
            
            //Add List
            resultModel.Add(kanSekeri);
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
