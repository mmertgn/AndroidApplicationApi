using AndroidApplicationData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace AndroidApplicationApi.Repository
{
  public class KanSekeriRepository
  {
    private readonly applicationEntities context = new applicationEntities();
    public bool Add(KanSekeri entity)
    {
      context.KanSekeri.Add(entity);
      var returnValue = context.SaveChanges();
      if (returnValue > 0)
        return true;
      else
        return false;
    }

    public List<KanSekeri> GetList(Expression<Func<KanSekeri, bool>> filter = null)
    {
      List<KanSekeri> list = new List<KanSekeri>();
      try
      {
        if (filter != null)
        {
          list = context.KanSekeri.Where(filter).ToList();
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