using AndroidApplicationApi.DataModels;
using AndroidApplicationApi.DTO;
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
  public class KullaniciController : ApiController
  {
    #region Validation
    StringBuilder brokenRules;

    /// <summary>
    /// Email adresinin sistemde var olup olmadığının kontrolünü sağlar.
    /// </summary>
    /// <param name="emailAdress"></param>
    /// <returns></returns>
    public bool checkEmailControl(string emailAdress)
    {
      brokenRules = new StringBuilder();
      KullaniciRepository repo = new KullaniciRepository();
      if (repo.GetUserList(x => x.Email == emailAdress) != null)
      {
        brokenRules.AppendLine("Daha önce kullanılmış bir email adresi ile tekrar kayıt işlemi gerçekleştirilemez.");
        return true;
      }

      return false;
    }

    public bool checkParameterNullControl(string adi, string soyadi, string email, string sifre, string cinsiyet, string diyabetTipi, DateTime teshisKonduguTarih, string il)
    {
      brokenRules = new StringBuilder();

      if (string.IsNullOrEmpty(adi) || string.IsNullOrEmpty(soyadi) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(sifre) || string.IsNullOrEmpty(cinsiyet) || string.IsNullOrEmpty(diyabetTipi) || teshisKonduguTarih == null || string.IsNullOrEmpty(il)) // Tüm alanlar doluysa 
      {
        brokenRules.AppendLine("Lütfen boş alan bırakmayın.");
        return true;
      }

      return false;
    }

    public bool checkPasswordControl(string password)
    {
      brokenRules = new StringBuilder();
      if (!string.IsNullOrEmpty(password))
      {
        if (password.Length < 5)
        {
          brokenRules.AppendLine("Şifreniz en az 5 karakter olmalıdır.");
          return true;
        }

      }
      return false;
    }

    public bool checkEmailandPasswordControl(string email, string sifre)
    {
      brokenRules = new StringBuilder();
      if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(sifre))
      {
        brokenRules.AppendLine("Lütfen email adresi ve şifrenizi giriniz.");
        return true;
      }
      return false;
    }

    #endregion

    #region
    public KullaniciDTO GetUser(string email, string sifre)
    {
      KullaniciRepository repo = new KullaniciRepository();
      KullaniciDTO dtoUser = new KullaniciDTO();
      var user = repo.GetUserList(x => x.Email == email && x.Sifre == sifre);
      if (user != null)
      {

        dtoUser.Id = user.Id;
        dtoUser.Adi = user.Adi;
        dtoUser.Soyadi = user.Soyadi;
        dtoUser.Sifre = user.Sifre;
        dtoUser.il = user.il;
        dtoUser.ilce = user.ilce;
        dtoUser.OlusturmaTarihi = user.OlusturmaTarihi.Value;
        dtoUser.TeshisKondoguTarih = user.TeshisKonduguTarih.Value;
        dtoUser.Email = user.Email;
        dtoUser.DiyabetTipi = user.DiyabetTipi;
        dtoUser.Cinsiyet = user.Cinsiyet;
      }
      return dtoUser;
    }
    #endregion

    [HttpGet]
    public ResponseModel Register(string request)
    {
      ResponseModel result = new ResponseModel();
      KullaniciRepository repo = new KullaniciRepository();
      var saveResult = false;
      var jsonObject = "";

      RegisterModel model = JsonConvert.DeserializeObject<RegisterModel>(request);

      bool emailControl = checkEmailControl(model.Email);
      bool parameterControl = checkParameterNullControl(model.Adi, model.Soyadi, model.Email, model.Sifre, model.Cinsiyet, model.DiyabetTipi, model.TeshisKonduguTarih, model.il);
      bool passwordControl = checkPasswordControl(model.Sifre);
      if (emailControl)
      {
        result.Message = "Daha önce kullanılmış bir email adresi ile tekrar kayıt işlemi gerçekleştirilemez.";
        result.Status = saveResult;
        result.Data = null;

        return result;
      }
      if (parameterControl || passwordControl)
      {
        result.Message = brokenRules.ToString();
        result.Status = saveResult;
        result.Data = null;

        return result;
      }

      Kullanicilar user = new Kullanicilar();
      user.Adi = model.Adi;
      user.Soyadi = model.Soyadi;
      user.Cinsiyet = model.Cinsiyet;
      user.Email = model.Email;
      user.DiyabetTipi = model.DiyabetTipi;
      user.DogumTarihi = model.DogumTarihi;
      user.Email = model.Email;
      user.il = model.il;
      user.ilce = model.ilce;
      user.Sifre = model.Sifre;

      user.OlusturmaTarihi = DateTime.Now;
      user.TeshisKonduguTarih = model.TeshisKonduguTarih;


      saveResult = repo.Add(user);
      if (saveResult == true)
      {
        brokenRules.AppendLine("Kayıt işlemi başarılı.");
        jsonObject = Newtonsoft.Json.JsonConvert.SerializeObject(user);
      }
      else
      {
        brokenRules.AppendLine("Kayıt işlemi gerçekleştirilemedi.");
      }

      result.Message = brokenRules.ToString();
      result.Status = saveResult;
      result.Data = jsonObject;

      return result;

    }

    [HttpPost]
    public ResponseModel Login(string request)
    {
      LoginModel requestModel = JsonConvert.DeserializeObject<LoginModel>(request);

      brokenRules = new StringBuilder();
      ResponseModel result = new ResponseModel();

      var nulCheck = checkEmailandPasswordControl(requestModel.Email, requestModel.Sifre);
      if (nulCheck)
      {
        result.Message = brokenRules.ToString();
        result.Status = false;
        result.Data = null;

        return result;
      }

      KullaniciDTO user = GetUser(requestModel.Email, requestModel.Sifre);

      if (user.Email != null && user.Id > 0)
      {
        var messageAsJson = JsonConvert.SerializeObject(user);

        result.Message = brokenRules.AppendLine("Kullanıcı başarılı bir şekilde giriş yaptı.").ToString();
        result.Status = true;
        result.Data = messageAsJson;

        return result;
      }

      result.Message = brokenRules.AppendLine("Kullanıcı bulunamadı.").ToString();
      result.Status = false;
      result.Data = null;
      return result;
    }

    [HttpGet]
    public ResponseModel KullaniciGetir(string request)
    {
      try
      {
        ResponseModel result = new ResponseModel();
        KullaniciRepository repo = new KullaniciRepository();
        KullaniciGetirModel model = JsonConvert.DeserializeObject<KullaniciGetirModel>(request);
        Kullanicilar user = repo.GetUserList(x => x.Id == model.KullaniciId);
        KullaniciDTO dto = new KullaniciDTO();
        var returnUser = "";
        if (user != null)
        {
          dto.Id = user.Id;
          dto.Adi = user.Adi;
          dto.Soyadi = user.Soyadi;
          dto.Email = user.Email;
          dto.Sifre = user.Sifre;
          dto.DogumTarihi = user.DogumTarihi.Value;
          dto.DiyabetTipi = user.DiyabetTipi;
          dto.TeshisKondoguTarih = user.TeshisKonduguTarih.Value;
          dto.il = user.il;
          dto.ilce = user.ilce;
          dto.OlusturmaTarihi = user.OlusturmaTarihi.Value;
   

          returnUser = JsonConvert.SerializeObject(dto);
          result.Message = "Success";
          result.Status = true;
          result.Data = returnUser;

          return result;
        }
        else
        {
          result.Message = "Kullanıcı bulunamadı.";
          result.Status = false;
          result.Data = null;

          return result;
        }
            

      }
      catch (Exception ex)
      {

        throw;
      }
    }
  }
}
