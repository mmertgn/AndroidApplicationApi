using AndroidApplicationData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace AndroidApplicationApi.Repository
{
  public class TansiyonRepository
  {
    private readonly applicationEntities context = new applicationEntities();
    public bool Add(Tansiyon entity)
    {
      context.Tansiyon.Add(entity);
      var returnValue = context.SaveChanges();
      if (returnValue > 0)
        return true;
      else
        return false;
    }
    public List<Kullanicilar> GetUserList(Expression<Func<Kullanicilar, bool>> filter = null)
    {
      List<Kullanicilar> users = new List<Kullanicilar>();
      try
      {
        if (filter != null)
        {
          users = context.Kullanicilar.Where(filter).ToList();
        }

      }
      catch (Exception)
      {

        throw;
      }
      return users;

    }
    public List<Tansiyon> GetList(Expression<Func<Tansiyon, bool>> filter = null)
    {
      List<Tansiyon> list = new List<Tansiyon>();
      try
      {
        if (filter != null)
        {
          list = context.Tansiyon.Where(filter).ToList();
        }
        else
        {
          list = null;
        }
      }
      catch (Exception)
      {

        throw;
      }
      return list;
    }
  }
}