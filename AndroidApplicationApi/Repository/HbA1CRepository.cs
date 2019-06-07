using AndroidApplicationData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace AndroidApplicationApi.Repository
{
  public class HbA1CRepository
  {
    private readonly applicationEntities context = new applicationEntities();
    public bool Add(HbA1c entity)
    {
      context.HbA1c.Add(entity);
      var returnValue = context.SaveChanges();
      if (returnValue > 0)
        return true;
      else
        return false;
    }

    public List<HbA1c> GetList(Expression<Func<HbA1c, bool>> filter = null)
    {
      List<HbA1c> list = new List<HbA1c>();
      try
      {
        if (filter != null)
        {
          list = context.HbA1c.Where(filter).ToList();
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