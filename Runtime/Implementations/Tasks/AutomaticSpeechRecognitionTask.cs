using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace HuggingFace.API {
    public class AutomaticSpeechRecognitionTask : TaskBase<byte[], string> {
        /**
            curl "https://xxxxxxxxxxxxxxxxxxxx.endpoints.huggingface.cloud" \
            -X POST \
            --data-binary '@sample1.flac' \
            -H "Accept: application/json" \
            -H "Authorization: Bearer hf_XXXXX" \
            -H "Content-Type: audio/flac" \
        */

        /*
             curl "https://xxxxxxxxxxxxxxxxxxxx.endpoints.huggingface.cloud" \
             -X POST \
             -H "Authorization: Bearer hf_XXXXXX" \
             -H "Content-Type: application/json" \
             -d '{
                   "inputs": "<AUDIO_EN_BASE64>",
                   "parameters": {
                     "language": "es"
                   }
                 }'
         */
        public override string taskName => "AutomaticSpeechRecognition";
        public override string defaultEndpoint => "https://xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx/openai/whisper-tiny";
        
        private Dictionary<string, string> curlHeaders = new Dictionary<string, string>() {
           { "Accept", "application/json" },
           { "Content-Type", "audio/flac" }
        };

        protected override string[] LoadBackupEndpoints() {
            return new string[] {                
            };
        }

        protected override IPayload GetPayload(byte[] input, object context) {
            return new ByteArrayPayload(input, curlHeaders);
            /*
             * TOOD: meter parameter language
             * return new JObjectPayload(new JObject
            {
                ["messages"] = JArray.FromObject(conversation.GetMessages()),
                ["model"] = "meta-llama/Llama-3.1-8B-Instruct",
                ["stream"] = false,
                ["parameters"] = new JObject
                {
                    ["temperature"] = 0.7,
                    ["max_new_tokens"] = 256,
                    ["top_p"] = 0.95,
                    ["top_k"] = 50,
                    ["repetition_penalty"] = 1.2
                }
            });*/
        }

        protected override bool PostProcess(object raw, byte[] input, object context, out string response, out string error) {
            error = "";
            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>((string)raw);
            if (!jsonResponse.TryGetValue("text", out JToken responseObject)) {
                error = "Response does not contain a text field.";
                response = null;
                return false;
            }
            response = responseObject.ToString();
            return true;
        }
    }
}