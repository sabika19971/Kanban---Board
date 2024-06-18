using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{

    public class Response
    {
        private Object responseValue;
        private string errorMessage;

        public Response(Object responseValue, string errorMassage)
        {
            this.responseValue = responseValue;
            this.errorMessage = errorMassage;
        }

        public Object ResponseValue
        {
            get { return ResponseValue; }
        }
        public string ErrorMessage
        {
            get { return errorMessage; }
    }
}
