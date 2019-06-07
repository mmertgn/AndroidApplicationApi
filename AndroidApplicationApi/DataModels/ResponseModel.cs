using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AndroidApplicationApi.DataModels
{
  public class ResponseModel
  {
    public  string Message { get; set; }
    public  bool Status { get; set; }
    public  object Data { get; set; }
  }
}