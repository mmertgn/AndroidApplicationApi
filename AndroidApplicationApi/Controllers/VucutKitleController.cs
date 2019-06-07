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
  public class VucutKitleController : ApiController
  {
    StringBuilder brokenRules;
    #region validation
    private bool checkParameters(int kullaniciId, DateTime tarih, int boy, int kilo)
    {
      if (string.IsNullOrEmpty(kullaniciId.ToString()) || string.IsNullOrEmpty(tarih.ToString()) || string.IsNullOrEmpty(boy.ToString()) || string.IsNullOrEmpty(kilo.ToString()))
      {
        brokenRules = new StringBuilder();
        brokenRules.AppendLine("Lütfen tüm alanları doldurun.");
        return true;
      }
      return false;
    }
    #endregion

    [HttpGet]
    public ResponseModel VucutKitleEkle(string request)
    {
      ResponseModel result = new ResponseModel();
      VucutKitleRepository repo = new VucutKitleRepository();
      VucutKitleModel viewModel = JsonConvert.DeserializeObject<VucutKitleModel>(request);
      try
      {
        bool saveResult = false;
        var jsonObject = string.Empty;
       

        bool parameterValidate = checkParameters(viewModel.kullaniciId, viewModel.tarih, viewModel.boy, viewModel.kilo);
        if (parameterValidate)
        {
          result.Data = null;
          result.Message = brokenRules.ToString();
          result.Status = false;
        }

        VucutKitle model = new VucutKitle();
        model.KullaniciId = viewModel.kullaniciId;
        model.Kilo = viewModel.kilo;
        model.Boy = viewModel.boy;
        model.Tarih = viewModel.tarih;
        model.OlusturmaTarihi = DateTime.Now;
        var vucutKitle = (double)Calculate(viewModel.boy,viewModel.kilo);
        model.VucutKitleIndexi = vucutKitle;

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
    public ResponseModel VucutKitleGetir(string request)
    {
      ResponseModel result = new ResponseModel();
      VucutKitleRepository repo = new VucutKitleRepository();
      List<VucutKitle> resultModel = new List<VucutKitle>();
      VucutKitle vucutKitle = new VucutKitle();

      try
      {
        string jsonObject = "";
        VucutKitleGetirModel requestModel = JsonConvert.DeserializeObject<VucutKitleGetirModel>(request);
        var list = repo.GetList(x => x.KullaniciId == requestModel.KullaniciId).OrderByDescending(x => x.OlusturmaTarihi).ToList();
        if (list.Count > 0)
        {
          foreach (var itemData in list)
          {
            vucutKitle = new VucutKitle();
            vucutKitle.Id = itemData.Id;
            vucutKitle.KullaniciId = itemData.KullaniciId;
            vucutKitle.OlusturmaTarihi = itemData.OlusturmaTarihi;
            vucutKitle.Tarih = itemData.Tarih;
            vucutKitle.Boy = itemData.Boy;
            vucutKitle.Kilo = itemData.Kilo;
            vucutKitle.VucutKitleIndexi = itemData.VucutKitleIndexi;

            //Add List
            resultModel.Add(vucutKitle);
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

    #region privateCalc
    private decimal Calculate(int boy, int kilo)
    {
      var boystring = boy.ToString();
      int count = 0;
      int kitleCount = 0;
      var newstring = "";
      var newKitle = "";
   
      foreach (var item in boystring)
      {
        count++;

        if (count == 2)
        {
          newstring += ",";
        }
        newstring += item;

      }
      decimal newboy = Convert.ToDecimal(newstring);
      decimal boycarpim = newboy * newboy;
      decimal vucutKitle = kilo / boycarpim;

      var yuvarlanacak = vucutKitle.ToString();
      foreach (var item in yuvarlanacak)
      {
        kitleCount++;
        if (kitleCount <6)
        {
          newKitle += item;
        }
      }
      var resultValue = decimal.Parse(newKitle);

      return resultValue;
    }
    #endregion
  }
}