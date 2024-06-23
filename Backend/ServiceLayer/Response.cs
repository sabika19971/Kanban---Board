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
        private object responseValue;
        private string errorMessage { get; set; }

        public Response()
        {

        }
        public Response(object responseValue, string errorMassage)
        {
            this.responseValue = responseValue;
            errorMessage = errorMassage;
        }

        public Object getResponseValue()
        {
            return this.responseValue;
        }

        public string getErrorMessage() { return this.errorMessage; }


       
    }
}
