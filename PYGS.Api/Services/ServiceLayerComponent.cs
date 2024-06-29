using RestSharp;
using System.Net;
using System.Text.Json.Serialization;
using System.Text.Json;
using PYGS.Api.Models;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using JsonObject = System.Text.Json.Nodes.JsonObject;
using JsonSerializer = System.Text.Json.JsonSerializer;
using JsonArray = System.Text.Json.Nodes.JsonArray;

namespace PYGS.Api.Services
{
    public class ServiceLayerComponent : IServiceLayerComponent
    {
        public string tokenString { get; set; }
        public string RouteId { get; set; }
        public string url { get; set; }
        public string database { get; set; }
        public string user { get; set; }
        public string pass { get; set; }

        private bool _isError;
        private string _errorMessage = "";

        public bool IsError => _isError;

        public string ErrorMessage => _errorMessage;

        public ServiceLayerComponent(string receivedUrl, string receivedDatabase, string receivedUser, string receivedPass)
        {
            url = receivedUrl;
            database = receivedDatabase;
            user = receivedUser;
            pass = receivedPass;
        }

        private async Task<IRestResponse> CallServiceLayer(string path, Method method, string body, string sessionId, string routeId, CancellationToken cancellationToken = default)
        {
            try
            {
                var Client = new RestClient(path);
                Client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

                IRestResponse response = null;
                //IRestClient Client = new RestClient(path);
                IRestRequest Request = new RestRequest(path, method); // ("Auth/SignIn");
                Request.Method = method;
                Request.Parameters.Clear();

                if (method == Method.GET)
                {
                    var paramList = body.Split("&", StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in paramList)
                    {
                        var param = item.Split("=", StringSplitOptions.RemoveEmptyEntries);
                        if (param.Length > 0)
                        {
                            Request.AddParameter(param[0], param[1]);
                        }
                    }

                }
                else
                {
                    Request.AddParameter("application/json", body, ParameterType.RequestBody);
                }
                if (!string.IsNullOrEmpty(sessionId))
                {
                    Request.AddCookie("B1SESSION", sessionId);
                    Request.AddCookie("ROUTEID", routeId);
                }
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                switch (method)
                {
                    case Method.GET:
                        response = await Client.ExecuteGetAsync(Request, cancellationToken);
                        break;
                    case Method.POST:
                        response = await Client.ExecutePostAsync(Request, cancellationToken);
                        break;
                    case Method.PUT:
                        response = await Client.ExecuteAsync(Request, method, cancellationToken);
                        break;
                    case Method.DELETE:
                        response = await Client.ExecuteAsync(Request, method, cancellationToken);
                        break;
                    case Method.HEAD:
                        break;
                    case Method.OPTIONS:
                        break;
                    case Method.PATCH:
                        response = await Client.ExecuteAsync(Request, method, cancellationToken);
                        break;
                    case Method.MERGE:
                        break;
                    case Method.COPY:
                        break;
                    default:
                        break;
                }

                return response;
            }
            catch (TaskCanceledException tex)
            {
                return new RestResponse() { StatusCode = HttpStatusCode.Unused };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Login(CancellationToken cancellationToken = default)
        {
            _isError = false;
            _errorMessage = "";
            try
            {
                string jsonLogin = "{\"CompanyDB\": \"" + database + "\",\"UserName\": \"" + user + "\",\"Password\": \"" + pass + "\",\"Language\": \"ln_Spanish_La\"}";
                string path = url + "/Login";
                System.Text.Json.Nodes.JsonObject login = new System.Text.Json.Nodes.JsonObject();

                IRestResponse response = await CallServiceLayer(path, Method.POST, jsonLogin, null, "", cancellationToken);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    login = (System.Text.Json.Nodes.JsonObject)System.Text.Json.Nodes.JsonNode.Parse(response.Content ?? "");

                    foreach (var item in response.Cookies)
                    {
                        if (item.Name == "ROUTEID" && item.Path == "/b1s")
                        {
                            RouteId = item.Value;
                        }
                    }
                }
                else
                {
                    tokenString = null;
                    RouteId = null;
                    _isError = true;
                    _errorMessage = GetError(response, "Error al intentar realizar el Login!");
                    return false;
                }

                // Obtengo y asigno el token
                tokenString = login["SessionId"].ToString(); //response.Content.Replace("\n", "").Replace("\"", "").Replace("{", "").Replace("}", "").Split(',')[1].Split(':')[1].Replace(" ", String.Empty);

                return true;
            }
            catch (Exception ex)
            {
                _isError = true;
                _errorMessage = ex.Message;
                return false;
            }

        }

        public async Task<bool> Login(string db, string usuario, string passw)
        {
            _isError = false;
            _errorMessage = "";
            try
            {
                string jsonLogin = "{\"CompanyDB\": \"" + db + "\",\"UserName\": \"" + usuario + "\",\"Password\": \"" + passw + "\",\"Language\": \"ln_Spanish_La\"}";
                string path = url + "/Login";
                System.Text.Json.Nodes.JsonObject login = new System.Text.Json.Nodes.JsonObject();

                IRestResponse response = await CallServiceLayer(path, Method.POST, jsonLogin, null, "");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    login = (System.Text.Json.Nodes.JsonObject)System.Text.Json.Nodes.JsonNode.Parse(response.Content ?? "");

                    foreach (var item in response.Cookies)
                    {
                        if (item.Name == "ROUTEID" && item.Path == "/b1s")
                        {
                            RouteId = item.Value;
                        }
                    }
                }
                else
                {
                    tokenString = null;
                    RouteId = null;
                    _isError = true;
                    _errorMessage = GetError(response, "Error al intentar realizar el Login!");
                    return false;
                }

                // Obtengo y asigno el token
                tokenString = login["SessionId"].ToString(); //response.Content.Replace("\n", "").Replace("\"", "").Replace("{", "").Replace("}", "").Split(',')[1].Split(':')[1].Replace(" ", String.Empty);

                return true;
            }
            catch (Exception ex)
            {
                _isError = true;
                _errorMessage = ex.Message;
                return false;
            }

        }

        public async Task Logout(CancellationToken cancellationToken = default)
        {
            string path = url + "/Logout";
            if (!string.IsNullOrEmpty(tokenString))
                await CallServiceLayer(path, Method.POST, null, tokenString, RouteId, cancellationToken);

            tokenString = null;
            RouteId = null;
        }

        
        public async Task<string> GetItemCode(string IdAdicional, CancellationToken cancellationToken = default)
        {
            _isError = false;
            _errorMessage = "";
            try
            {
                string path = url + "/Items";
                string filter = $"$select=ItemCode&$filter=SWW eq '{IdAdicional}'";
                var result = await CallServiceLayer(path, Method.GET, filter, tokenString, RouteId, cancellationToken);

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    System.Text.Json.Nodes.JsonObject data = new System.Text.Json.Nodes.JsonObject();
                    data = (System.Text.Json.Nodes.JsonObject)System.Text.Json.Nodes.JsonNode.Parse(result.Content ?? "");

                    if (data != null)
                    {
                        if (data["value"] != null && data["value"].ToJsonString() != "[]")
                        {
                            return data["value"][0]["ItemCode"].ToString();
                        }
                    }
                    return "";
                }

                _isError = true;
                _errorMessage = GetError(result, "Error al Obtener el Código del Articulo");
                return "";
            }
            catch (Exception ex)
            {
                _isError = true;
                _errorMessage = ex.Message;
                return "";
            }
        }


        public async Task SetIdAdicionalArticulo(string ItemCode, string IdAdicional, CancellationToken cancellationToken = default)
        {
            _isError = false;
            _errorMessage = "";
            try
            {
                string path = $"{url}/Items('{ItemCode}')";
                string body = "{\"SWW\": \"" + IdAdicional + "\"}";
                var result = await CallServiceLayer(path, Method.PATCH, body, tokenString, RouteId, cancellationToken);

                if (result.StatusCode == HttpStatusCode.NoContent)
                {
                    return;
                }

                _isError = true;
                _errorMessage = GetError(result, "Error al Actualizar el Id Adicional del Articulo");
                return;
            }
            catch (Exception ex)
            {
                _isError = true;
                _errorMessage = ex.Message;
            }
        }

        public async Task<Document> CreatePurchaseDeliveryNotes(Document document, CancellationToken cancellationToken = default)
        {
            _isError = false;
            _errorMessage = "";
            try
            {
                string path = $"{url}/PurchaseDeliveryNotes";

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };
                string jsonString = JsonSerializer.Serialize(document, options);

                string body = jsonString;
                var result = await CallServiceLayer(path, Method.POST, body, tokenString, RouteId, cancellationToken);

                if (result.StatusCode == HttpStatusCode.Created)
                {
                    var retorno = result.Content;
                    if (String.IsNullOrEmpty(retorno))
                    {
                        return new Document();
                    }
                    Document? returDocument =
                        JsonSerializer.Deserialize<Document>(retorno);
                    return returDocument;
                }

                _isError = true;
                _errorMessage = GetError(result, "Error al Crear la Entrada de Mercancia!");
                return new Document();
            }
            catch (Exception ex)
            {
                _isError = true;
                _errorMessage = ex.Message;
                return new Document();
            }
        }

        public async Task<Document> CreateInventoryGenEntries(Document inventoryGenEntries, CancellationToken cancellationToken = default)
        {
            _isError = false;
            _errorMessage = "";
            try
            {
                string path = $"{url}/InventoryGenEntries";

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };
                string jsonString = JsonSerializer.Serialize(inventoryGenEntries, options);

                string body = jsonString;
                var result = await CallServiceLayer(path, Method.POST, body, tokenString, RouteId, cancellationToken);

                if (result.StatusCode == HttpStatusCode.Created)
                {
                    var retorno = result.Content;
                    if (String.IsNullOrEmpty(retorno))
                    {
                        return new Document();
                    }
                    Document? returinventoryGenEntries =
                        JsonSerializer.Deserialize<Document>(retorno);
                    return returinventoryGenEntries;
                }

                _isError = true;
                _errorMessage = GetError(result, "Error al Crear la Terminación de la orden de producción!");
                return new Document();
            }
            catch (Exception ex)
            {
                _isError = true;
                _errorMessage = ex.Message;
                return new Document();
            }
        }

        public async Task SetProductionOrderStatus(int absoluteEntry, string productionOrderStatus, CancellationToken cancellationToken = default)
        {
            _isError = false;
            _errorMessage = "";
            try
            {
                string path = $"{url}/ProductionOrders({absoluteEntry})";
                string _productionOrderStatus;
                switch (productionOrderStatus)
                {
                    case "C":
                        _productionOrderStatus = "boposClosed";
                        break;
                    case "P":
                        _productionOrderStatus = "boposPlanned";
                        break;
                    case "L":
                        _productionOrderStatus = "boposReleased";
                        break;
                    case "boposPlanned":
                        _productionOrderStatus = productionOrderStatus;
                        break;
                    case "boposReleased":
                        _productionOrderStatus = productionOrderStatus;
                        break;
                    case "boposClosed":
                        _productionOrderStatus = productionOrderStatus;
                        break;
                    case "boposCancelled":
                        _productionOrderStatus = productionOrderStatus;
                        break;
                    default:
                        throw new ArgumentException("Debe de igresar un valor valido para productionOrderStatus!");
                        break;
                }
                string body = "{\"ProductionOrderStatus\": \"" + _productionOrderStatus + "\"}";
                var result = await CallServiceLayer(path, Method.PATCH, body, tokenString, RouteId, cancellationToken);

                if (result.StatusCode == HttpStatusCode.NoContent)
                {
                    return;
                }

                _isError = true;
                _errorMessage = GetError(result, "Error al Actualizar el Estatus de la Orden de Producción");
                return;
            }
            catch (Exception ex)
            {
                _isError = true;
                _errorMessage = ex.Message;
            }
        }

        private string GetError(IRestResponse response, string messageDefaul = "Error")
        {
            try
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NoContent:
                        return response.Content;
                        break;
                    case HttpStatusCode.BadRequest:
                    case HttpStatusCode.NotFound:
                    case HttpStatusCode.Unauthorized:
                    case HttpStatusCode.Forbidden:
                        System.Text.Json.Nodes.JsonObject data = new System.Text.Json.Nodes.JsonObject();
                        data = (System.Text.Json.Nodes.JsonObject)System.Text.Json.Nodes.JsonNode.Parse(response.Content ?? "");

                        if (data != null)
                        {
                            var mensaje = data["error"]["message"]["value"].ToString();
                            if (mensaje != null)
                            {
                                return mensaje;
                            }
                        }
                        return messageDefaul;
                        break;
                    default:
                        return messageDefaul;
                        break;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "";
        }

        public async Task<bool> VerifConexion(CancellationToken cancellationToken = default)
        {
            try
            {
                var retorno = await Login(cancellationToken);
                if (retorno)
                    await Logout(cancellationToken);
                return retorno;
            }
            catch (Exception)
            {
                return false;
            }

        }

        // SALIDA DE MERCANCIA
        public async Task<Document> CreateInventoryGenExits(Document inventoryGenExits, CancellationToken cancellationToken = default)
        {
            _isError = false;
            _errorMessage = "";
            try
            {
                string path = $"{url}/InventoryGenExits";

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };
                string jsonString = JsonSerializer.Serialize(inventoryGenExits, options);

                string body = jsonString;
                var result = await CallServiceLayer(path, Method.POST, body, tokenString, RouteId, cancellationToken);

                if (result.StatusCode == HttpStatusCode.Created)
                {
                    var retorno = result.Content;
                    if (String.IsNullOrEmpty(retorno))
                    {
                        return new Document();
                    }
                    Document? returinventoryGenExits =
                        JsonSerializer.Deserialize<Document>(retorno);
                    return returinventoryGenExits;
                }

                _isError = true;
                _errorMessage = GetError(result, "Error al dar salida al alimento!");
                return new Document();
            }
            catch (Exception ex)
            {
                _isError = true;
                _errorMessage = ex.Message;
                return new Document();
            }
        }

        public async Task<JsonObject?> GetCardDetails(string RUC, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tokenString) || string.IsNullOrEmpty(RouteId))
            {
                // Intentar realizar el login nuevamente
                bool loginSuccess = await Login(cancellationToken);
                if (!loginSuccess)
                {
                    _isError = true;
                    _errorMessage = "Error al realizar el login";
                    return null;
                }
            }

            _isError = false;
            _errorMessage = "";
            try
            {
                string path = url + "/BusinessPartners";
                string filter = $"$select=CardCode,CardName,CardType,GroupCode,PayTermsGrpCode&$filter=contains(FederalTaxID, '{RUC}') and CardType eq 'cCustomer'";
                var result = await CallServiceLayer(path, Method.GET, filter, tokenString, RouteId, cancellationToken);

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var data = JsonNode.Parse(result.Content)?.AsObject();
                    var values = data?["value"]?.AsArray();

                    if (values != null && values.Count > 0)
                    {
                        return values[0]?.AsObject();
                    }
                    return null;
                }

                _isError = true;
                _errorMessage = GetError(result, "Error al Obtener los detalles del Proveedor");
                return null;
            }
            catch (Exception ex)
            {
                _isError = true;
                _errorMessage = ex.Message;
                return null;
            }
        }

        public async Task<bool> CreateBusinessPartner(BusinessPartner businessPartner, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tokenString) || string.IsNullOrEmpty(RouteId))
            {
                // Intentar realizar el login nuevamente
                bool loginSuccess = await Login(cancellationToken);
                if (!loginSuccess)
                {
                    _isError = true;
                    _errorMessage = "Error al realizar el login";
                    return false;
                }
            }

            _isError = false;
            _errorMessage = "";
            try
            {
                string path = url + "/BusinessPartners";
                var result = await CallServiceLayer(path, Method.POST, JsonConvert.SerializeObject(businessPartner), tokenString, RouteId, cancellationToken);

                if (result.StatusCode == HttpStatusCode.Created)
                {
                    return true;
                }

                _isError = true;
                _errorMessage = GetError(result, "Error al crear el BusinessPartner");
                return false;
            }
            catch (Exception ex)
            {
                _isError = true;
                _errorMessage = ex.Message;
                return false;
            }
        }
    }

}
