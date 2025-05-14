using System;
using System.Collections.Generic;
using UnityEngine;

namespace HuggingFace.API {

    [Serializable]
    public class Message
    {
        public string role;
        public string content;
        public static Message CreateUserMessage(string content)
        {
            return new Message { role = "user", content = content };
        }

        public static Message CreateAssistantMessage(string content)
        {
            return new Message { role = "assistant", content = content };
        }
    }
    public class Conversation {

        private List<Message> _messages = new List<Message>();

        public void AddUserInput(string userInput) {
            var message = Message.CreateUserMessage(userInput);
            _messages.Add(message);
        }

        public void AddGeneratedResponse(string generatedResponse) {
            var message = Message.CreateAssistantMessage(generatedResponse);
            _messages.Add(message);
        }

        public Message GetLatestMessage() {
            if(_messages.Count == 0) {
                Debug.LogWarning("No generated responses found.");
                return null;
            }
            return _messages[_messages.Count - 1];
        }        

        public List<Message> GetMessages() {
            return _messages;
        }

        public void Clear() {
            _messages.Clear();
        }
    }
}