using AndroidApplicationData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace AndroidApplicationApi.Repository
{
  public class VucutKitleRepository : KullaniciRepository
  {
    private readonly applicationEntities context = new applicationEntities();
    public bool Add(VucutKitle entity)
    {
      context.VucutKitle.Add(entity);
      var returnValue = context.SaveChanges();
      if (returnValue > 0)
        return true;
      else
        return false;
    }

    public List<VucutKitle> GetList(Expression<Func<VucutKitle, bool>> filter = null)
    {
      List<VucutKitle> list = new List<VucutKitle>();
      try
      {
        if (filter != null)
        {
          list = context.VucutKitle.Where(filter).ToList();
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