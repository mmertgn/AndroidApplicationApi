using AndroidApplicationData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace AndroidApplicationApi.Repository
{
  public class EgzersizRepository
  {
    private readonly applicationEntities context = new applicationEntities();
    public bool Add(Egzersiz entity)
    {
      context.Egzersiz.Add(entity);
      var returnValue = context.SaveChanges();
      if (returnValue > 0)
        return true;
      else
        return false;
    }

    public List<Egzersiz> GetList(Expression<Func<Egzersiz, bool>> filter = null)
    {
      List<Egzersiz> list = new List<Egzersiz>();
      try
      {
        if (filter != null)
        {
          list = context.Egzersiz.Where(filter).ToList();
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