using System;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace HuggingFace.API {
    public class ByteArrayPayload : IPayload {
        public byte[] payload { get; private set; }
        private Dictionary<string, string> curlParams = null;

        public ByteArrayPayload(byte[] payload, Dictionary<string, string> curlParams = null) {
            this.payload = payload;
            this.curlParams = curlParams;
        }

        public void Prepare(UnityWebRequest request) {
            request.uploadHandler = new UploadHandlerRaw(payload);

            if (curlParams != null)
            {
                foreach (var param in curlParams)
                {
                    request.SetRequestHeader(param.Key, param.Value);
                }
            }
        }

        public override string ToString() {
            return BitConverter.ToString(payload);
        }
    }
}
