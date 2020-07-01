
using IBM.Cloud.SDK.Core.Authentication.Iam;
using IBM.Watson.Assistant.v2;
using IBM.Watson.Assistant.v2.Model;
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
        public static string assistantId = "c338c7f5-f00a-4530-81e6-90e953e94397";
        public static string workSpaceId = "cc2bbb09-e6f5-4be5-9059-2cfbaef1431c";
        public static string userId = "apikey";
        public static string password = "KCnfmMFb4VlunGdgoPQqcwsbw3A6rkB465752jKB7yBo";
        public string serverLink = string.Format("https://api.us-south.assistant.watson.cloud.ibm.com/instances/971a4d32-9f85-498d-a858-e12ef24556ef/v1/workspaces/{0}/message?version=2020-04-01", workSpaceId);
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
            var session = CreateSessionId(assistantId);
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
        public string[] ResponseMessageV2([FromBody]JObject data)
        {
            string sessionId = data["sessionId"].ToString();
            string input = data["text"].ToString();
            String[] responce = new String[2];

            if (string.IsNullOrEmpty(sessionId))
            {
                sessionId = CreateSessionId(assistantId);
            }

            IamAuthenticator authenticator = new IamAuthenticator(
            apikey: "KCnfmMFb4VlunGdgoPQqcwsbw3A6rkB465752jKB7yBo"
            );

            AssistantService assistant = new AssistantService("2020-04-01", authenticator);
            assistant.SetServiceUrl("https://api.us-south.assistant.watson.cloud.ibm.com/instances/971a4d32-9f85-498d-a858-e12ef24556ef");

            try {
                var result = assistant.Message(
                              assistantId: "c338c7f5-f00a-4530-81e6-90e953e94397",
                              sessionId: "" + sessionId + "",
                              input: new MessageInput()
                                    {
                                        Text = input
                                    }
                              );

                Console.WriteLine(result.Response);
                string responseBody = JsonConvert.DeserializeObject(result.Response).ToString();
                JObject jo = JObject.Parse(responseBody);
                string responseOut = jo["output"]["generic"][0]["text"].ToString();
                responce[0] = responseOut;
                responce[1] = sessionId;
                return responce;

            }
            catch(Exception ex)
            {
                return null;
            }



        }


        public string CreateSessionId(string assistantId)
        {
            string sessionId = null;

            try {
                 IamAuthenticator authenticator = new IamAuthenticator(
                  apikey: "KCnfmMFb4VlunGdgoPQqcwsbw3A6rkB465752jKB7yBo"
                  );

                 AssistantService assistant = new AssistantService("2020-04-01", authenticator);
                 assistant.SetServiceUrl("https://api.us-south.assistant.watson.cloud.ibm.com/instances/971a4d32-9f85-498d-a858-e12ef24556ef");

                 var result = assistant.CreateSession(
                    assistantId: "c338c7f5-f00a-4530-81e6-90e953e94397"
                    );

                 Console.WriteLine(result.Response);

                 sessionId = result.Result.SessionId;

                 return sessionId;
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }
        }


    }
}
