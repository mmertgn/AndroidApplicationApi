using AndroidApplicationData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace AndroidApplicationApi.Repository
{
  public class SuRepository
  {
    private readonly applicationEntities context = new applicationEntities();
    public bool Add(Su entity)
    {
      context.Su.Add(entity);
      var returnValue = context.SaveChanges();
      if (returnValue > 0)
        return true;
      else
        return false;
    }

    public List<Su> GetList(Expression<Func<Su, bool>> filter = null)
    {
      List<Su> list = new List<Su>();
      try
      {
        if (filter != null)
        {
          list = context.Su.Where(filter).ToList();
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