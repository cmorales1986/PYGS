using System.Net;
using System.Text.Json;


namespace PYGS.Web.Repositories
{
    public class HttpResponseWrapper<T>
    {
        public HttpResponseWrapper(T? response, bool error, HttpResponseMessage httpResponseMessage)
        {
            Error = error;
            Response = response;
            HttpResponseMessage = httpResponseMessage;
        }

        public bool Error { get; set; }

        public T? Response { get; set; }

        public HttpResponseMessage HttpResponseMessage { get; set; }

        public async Task<string?> GetErrorMessageAsync()
        {
            if (!Error)
            {
                return null;
            }

            var statusCode = HttpResponseMessage.StatusCode;
            if (statusCode == HttpStatusCode.NotFound)
            {
                return "Recurso no encontrado";
            }
            else if (statusCode == HttpStatusCode.BadRequest)
            {
                return apiError(await HttpResponseMessage.Content.ReadAsStringAsync());
            }
            else if (statusCode == HttpStatusCode.Unauthorized)
            {
                return "Tienes que logearte para hacer esta operación";
            }
            else if (statusCode == HttpStatusCode.Forbidden)
            {
                return "No tienes permisos para hacer esta operación";
            }

            return "Ha ocurrido un error inesperado";
        }

        private string apiError(string result, string Defaul = "")
        {
            try
            {
                string mensaje = Defaul;
                if (result.Contains("title"))
                {
                    using var error = JsonDocument.Parse(result);
                    mensaje = error.RootElement.GetProperty("title").GetString();
                    if (result.Contains("errors"))
                    {
                        if (mensaje.Contains("One or more validation errors occurred"))
                        {
                            var errorcito = error.RootElement.GetProperty("errors").ToString().Replace("{", "").Replace("}", "").Replace("[", "").Replace("]", "");
                            var errorcito_ = errorcito.ToString().Replace("\"", "");
                            string[] pares = errorcito_.Split(',');
                            string nuevaErrorcito = string.Join(",", pares.Select(par =>
                            {
                                int indiceDosPuntos = par.IndexOf(':');
                                return par.Substring(indiceDosPuntos + 1);
                            }));
                            mensaje = $"{nuevaErrorcito}";

                        }

                    }
                }
                if (result.Contains("message"))
                {
                    using var error = JsonDocument.Parse(result);
                    mensaje = error.RootElement.GetProperty("message").GetString();
                }
                else if (!string.IsNullOrEmpty(result) && !result.Contains("{"))
                {
                    mensaje = result;
                }

                return mensaje;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
