using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.CallBack.Handler
{
    public interface IHandlerService
    {
       void Handel(object handlerArgs, object result);
    }
}