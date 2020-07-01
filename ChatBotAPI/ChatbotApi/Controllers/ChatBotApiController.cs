using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ChatbotApi.Controllers
{
    public class CreateSession
    {

    }
    public class ChatBotApiController : ApiController
    {
        public static string assistantId = "dca0bcd8-0e4b-4b92-82f3-1a1746aa1ac2";
        public static string workSpaceId = "8074ebc3-e639-4b22-8420-29699e139618";
        public static string userId = "apikey";
        public static string password = "wiwI1g7siL00hfvuOAoMaxqN9KFYGiP2gl1mH9n-0Sa5";
        public string serverLink = string.Format("https://gateway.watsonplatform.net/conversation/api/v1/workspaces/{0}/message?version=2019-02-28", workSpaceId);
        public NetworkCredential networkCredential = new NetworkCredential(userId, password);

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        [Route("api/ChatbotApi/ResponseMessage")]
        public async Task<String[]> ResponseMessage([FromBody]JObject data)
        {
            string context = data["context"].ToString();
            string input = data["text"].ToString();
            string request = null;
            string responseOut = null;
            string contextOut = null;
            JObject jo = null;
            JObject jResult = null;
            String[] responce = new String[2];
            var session = await CreateSessionId(assistantId);
            if (string.IsNullOrEmpty(context))
            {
                request = "{\"input\": {\"text\": \"" + input + "\"}, \"alternate_intents\": true}";
            }
            else
            {
                request = "{\"input\": {\"text\": \"" + input + "\"}, \"alternate_intents\": true}, \"context\": \"" + context + "\"";
            }

            using (var handler = new HttpClientHandler
            {
                Credentials = networkCredential
            })

            using (var client = new HttpClient(handler))
            {
                var cont = new HttpRequestMessage();
                cont.Content = new StringContent(request.ToString(), Encoding.UTF8, "application/json");
                var result = await client.PostAsync(serverLink, cont.Content);
                string respose = await result.Content.ReadAsStringAsync();
                string responseBody = JsonConvert.DeserializeObject(respose).ToString();
                jo = JObject.Parse(responseBody);
                responseOut = jo["output"]["text"][0].ToString();
                contextOut = jo["context"].ToString();
                responce[0] = responseOut;
                responce[1] = contextOut;

            }
            return responce;
        }

        [HttpPost]
        [Route("api/ChatbotApi/ResponseMessageV2")]
        public async Task<String[]> ResponseMessageV2([FromBody]JObject data)
        {
            string sessionId = data["sessionId"].ToString();
            string input = data["text"].ToString();
            String[] responce = new String[2];
            
            if (string.IsNullOrEmpty(sessionId))
            {
                sessionId = await CreateSessionId(assistantId);
            }
            string serverLink = string.Format("https://gateway.watsonplatform.net/assistant/api/v2/assistants/{0}/sessions/{1}/message?version=2019-02-28", assistantId, sessionId);
            string request = "{\"input\": {\"text\": \"" + input + "\"}, \"alternate_intents\": true}";

            using (var handler = new HttpClientHandler
            {
                Credentials = networkCredential
            })

            using (var client = new HttpClient(handler))
            {
                var cont = new HttpRequestMessage();
                cont.Content = new StringContent(request.ToString(), Encoding.UTF8, "application/json");

                try
                {
                    var result = await client.PostAsync(serverLink, cont.Content);
                    string respose = await result.Content.ReadAsStringAsync();
                    string responseBody = JsonConvert.DeserializeObject(respose).ToString();
                    JObject jo = JObject.Parse(responseBody);
                    string responseOut = jo["output"]["generic"][0]["text"].ToString();
                    responce[0] = responseOut;
                    responce[1] = sessionId;
                    return responce;
                }

                catch (Exception ex)
                {
                    return null;
                }
                

            }
            
        }


        public async Task<string> CreateSessionId(string assistantId)
        {
            string sessionId = null;
            string apikey = "wiwI1g7siL00hfvuOAoMaxqN9KFYGiP2gl1mH9n-0Sa5";
            string CreateSessionserverLink = string.Format("https://gateway.watsonplatform.net/assistant/api/v2/assistants/{0}/sessions?version=2019-02-28", assistantId);
            string request = "{\"apikey\": \"" + apikey + "\"}";
            using (var handler = new HttpClientHandler
            {
                Credentials = networkCredential
            })

            using (var client = new HttpClient(handler))
            {
                var cont = new HttpRequestMessage();
                cont.Content = new StringContent(request.ToString(), Encoding.UTF8, "application/json");

                try
                {
                    var result = await client.PostAsync(CreateSessionserverLink, cont.Content);
                    string respose = await result.Content.ReadAsStringAsync();
                    string responseBody = JsonConvert.DeserializeObject(respose).ToString();
                    JObject jo = JObject.Parse(responseBody);
                    sessionId = jo["session_id"].ToString();
                    return sessionId;
                }

                catch (Exception ex)
                {
                    return "Web service error";
                }
                
            }

               
        }
            

    }
}
