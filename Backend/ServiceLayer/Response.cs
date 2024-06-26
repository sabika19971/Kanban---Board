using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class Response
    {
        public object ReturnValue { get; }
        public string ErrorMessage { get; }

        public Response()
        {

        }
        public Response(object returnValue, string errorMassage)
        {
            ReturnValue = returnValue;
            ErrorMessage = errorMassage;
        }     
    }
}
