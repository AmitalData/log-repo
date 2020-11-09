using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.Core.Abstractions
{
    public abstract class IntegrationService
    {
        protected abstract string ApiController { get; }
        public bool HasException { get; private set; }
        public string ExceptionMessage { get; private set; }

        private HttpResponseMessage response;
        public HttpResponseMessage Response
        {
            get { return response; }

            protected set
            {
                response = value;

                if (value != null)
                {
                    switch (value.StatusCode)
                    {
                        case System.Net.HttpStatusCode.OK:
                            {
                                HasException = false;
                                ExceptionMessage = null;
                                break;
                            }

                        case System.Net.HttpStatusCode.BadRequest:
                        case System.Net.HttpStatusCode.InternalServerError:
                            {
                                HasException = true;
                                ExceptionMessage = null;

                                var stringResult = value.Content.ReadAsStringAsync().Result;

                                if (!string.IsNullOrEmpty(stringResult) && stringResult != "null")
                                {
                                    IntegrationTestException ex = JsonConvert.DeserializeObject<IntegrationTestException>(stringResult);

                                    if (!string.IsNullOrEmpty(ex.ErrorMessage))
                                    {
                                        ExceptionMessage = ex.ErrorMessage.TrimEnd(Environment.NewLine.ToCharArray());
                                    }
                                }

                                break;
                            }
                    }
                }
            }
        }
    }
}
