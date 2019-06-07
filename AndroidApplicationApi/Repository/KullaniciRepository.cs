using AndroidApplicationData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace AndroidApplicationApi.Repository
{
  public class KullaniciRepository
  {
    private readonly applicationEntities context = new applicationEntities();
    public bool Add(Kullanicilar entity)
    {
      context.Kullanicilar.Add(entity);
      var returnValue = context.SaveChanges();
      if (returnValue > 0)
        return true;
      else
        return false;
    }
    public  Kullanicilar GetUserList(Expression<Func<Kullanicilar, bool>> filter = null)
    {
     Kullanicilar users = new Kullanicilar();
      try
      {
        if (filter !=null)
        {
          users =context.Kullanicilar.Where(filter).FirstOrDefault();
        }

      }
      catch (Exception)
      {

        throw;
      }
      return users;
     
    }
    
    
  }
}