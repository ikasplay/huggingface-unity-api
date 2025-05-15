using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using UnityEngine;

namespace HuggingFace.API {
    public class ConversationTask : TaskBase<string, string, Conversation> {
        /*
        curl "https://xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx/v1/chat/completions" \
            -X POST \
            -H "Authorization: Bearer hf_XXXXX" \
            -H "Content-Type: application/json" \
            -d '{
                "model": "tgi",
                "messages": [
                    {
                        "role": "user",
                        "content": "What is deep learning?"
                    }
                ],
                "max_tokens": 150,
                "stream": true
            }'
        */
        public override string taskName => "Conversation";
        public override string defaultEndpoint => "https://router.huggingface.co/hf-inference/models/meta-llama/Llama-3.1-8B-Instruct/v1/chat/completions";

        protected override string[] LoadBackupEndpoints() {
            return new string[0];
        }

        protected override bool VerifyContext(object context, out Conversation conversation) {
            conversation = null;
            if (context == null) {
                conversation = new Conversation();
                return true;
            } else if (context is Conversation) {
                conversation = (Conversation)context;
                return true;
            }
            return false;
        }

        protected override IPayload GetPayload(string input, Conversation conversation)
        {
            return new JObjectPayload(new JObject
            {
                ["messages"] = JArray.FromObject(conversation.GetMessages()),
                ["model"] = "tgi",
                ["stream"] = false,
                ["max_tokens"] = 150
            });
        }

        protected override bool PostProcess(object raw, string input, Conversation conversation, out string response, out string error) {
            error = "";
            UnityEngine.Debug.Log((string)raw);
            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>((string)raw);
            string generatedResponse = (string)jsonResponse["choices"][0]["message"]["content"];

            if (generatedResponse == null) {
                error = "Response does not contain a message field.";
                response = null;
                return false;
            }

            //conversation.AddUserInput((string)input);
            //conversation.AddGeneratedResponse(generatedResponse);
            response = generatedResponse;
            return true;
        }
    }
}