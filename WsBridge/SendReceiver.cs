using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace WsBridge
{
    public class SendReceiver
    {
        #region MÉTODOS BASES

        public enum Method
        {
            Get,
            Post,
            Put
        }

        public static async Task<JToken> RestAsync(Method method, string url, HttpContent content = null, string token = null, string tipoAutenticao = "Bearer")
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((a, b, c, d) => true);

                using (var client = new HttpClient())
                {
                    client.MaxResponseContentBufferSize = int.MaxValue;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add((new MediaTypeWithQualityHeaderValue("application/json")));

                    if (!string.IsNullOrWhiteSpace(token))
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tipoAutenticao, token);

                    using (var response = await (method == Method.Get ? client.GetAsync(url) : method == Method.Post ? client.PostAsync(url, content) : client.PutAsync(url, content)))
                    {
                        var result = await response.Content.ReadAsStringAsync();

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                return JObject.FromObject(new
                {
                    Code = "999",
                    Data = ex.Message,
                    Mensagem = ex.Message
                });
            }
        }

        public static async Task<JToken> GetAsync(string url, string token = null) =>
            await RestAsync(Method.Get, url, null, token);

        public static async Task<JToken> PostAsync(string url, dynamic content, string token = null, string tipoAutenticao = "Bearer")
        {
            var jObj = JsonConvert.SerializeObject((object)content);

            using (var strCont = new StringContent(jObj, Encoding.UTF8, "application/json"))
                return await RestAsync(Method.Post, url, strCont, token, tipoAutenticao);
        }

        public static async Task<JToken> PutAsync(string url, dynamic content, string token = null, string tipoAutenticao = "Bearer")
        {
            var jObj = JsonConvert.SerializeObject((object)content);

            using (var strCont = new StringContent(jObj, Encoding.UTF8, "application/json"))
                return await RestAsync(Method.Put, url, strCont, token, tipoAutenticao);
        }

        public static async Task<JToken> TokenAsync(string url, string macAdress)
        {
            var data = new Dictionary<string, string>()
            {
                { "grant_type", "password" },
                {"username",macAdress},
                {"password","MaisPostos#17101112"}
            };

            return await SolicitaToken(url, data);
        }

        public static async Task<JToken> TokenTerceiros(string url, Dictionary<string, string> data)
        {
            return await SolicitaToken(url, data);
        }

        public static async Task<JToken> LoginMobile(string url, string cnpj, string senha)
        {
            var data = new Dictionary<string, string>()
            {
                { "grant_type", "password" },
                {"username",cnpj},
                {"password",senha},
                {"layout","1" }
            };

            return await SolicitaToken(url, data);

        }

        public static async Task<JToken> LoginWeb(string url, Dictionary<string, string> data)
        {
            return await SolicitaToken(url, data);

        }


        private static async Task<JToken> SolicitaToken(string url, Dictionary<string, string> data)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((a, b, c, d) => true);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                using (var client = new HttpClient())
                {
                    client.MaxResponseContentBufferSize = int.MaxValue;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add((new MediaTypeWithQualityHeaderValue("application/json")));



                    using (var response = await (client.PostAsync(url, new FormUrlEncodedContent(data))))
                    {
                        var result = await response.Content.ReadAsStringAsync();

                        try
                        {
                            var token = JObject.FromObject(JsonConvert.DeserializeObject(result.ToString()));

                            if (token.ContainsKey("error"))
                            {
                                return JObject.FromObject(new
                                {
                                    Code = "200",
                                    Data = token["error"].ToString(),
                                    Mensagem = "Erro",
                                });
                            }
                            else
                            {
                                var tokenOk = token["access_token"].ToString();

                                return JObject.FromObject(new
                                {
                                    Code = "200",
                                    Data = result,
                                    Mensagem = "Sucesso",
                                    DataExpiracao = DateTime.Now.AddDays(1)
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            return JObject.FromObject(new
                            {
                                Code = "200",
                                Data = result + "  - " ,//+ clfException.RetornarErro(ex),
                                Mensagem = ""//clfException.RetornarErro(ex)
                            });
                        }



                    }
                }
            }
            catch (Exception ex)
            {
                return JObject.FromObject(new
                {
                    Code = "999",
                    Data = "",//clfException.RetornarErro(ex),
                    Mensagem ="",// clfException.RetornarErro(ex)
                });
            }

        }

        public static HttpClient GetClient(string username, string password)
        {
            var authValue = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}")));

            var client = new HttpClient()
            {
                DefaultRequestHeaders = { Authorization = authValue }
            };
            return client;
        }

        #endregion
    }
}
