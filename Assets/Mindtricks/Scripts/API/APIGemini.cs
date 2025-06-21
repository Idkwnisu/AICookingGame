using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;



[Serializable]
public struct GeminiInputStep1
{
    public GeminiParts[] contents;
    public GeminiGenerationConfigStep1 generationConfig;
}
[Serializable]
public struct GeminiInputStep2
{
    public GeminiParts[] contents;
    public GeminiGenerationConfigStep2 generationConfig;
}
[Serializable]
public struct GeminiInputStep3
{
    public GeminiParts[] contents;
    public GeminiGenerationConfigStep3 generationConfig;
}

[Serializable]
public struct GeminiParts
{
    public GeminiText[] parts;
}

[Serializable]
public struct GeminiText
{
    public string text;
}

[Serializable]
public struct GeminiGenerationConfigStep1
{
    public string responseMimeType;
    public ResponseSchemaStep1 responseSchema;
}

[Serializable]
public struct ResponseSchemaStep1
{
    public string type;
    public PropertiesStep1 properties;
    public string[] required;
}


[Serializable]
public struct PropertiesStep1
{
    public SingleProperty recipeName;
    public SingleProperty recipeDescription;
}


[Serializable]
public struct GeminiGenerationConfigStep2
{
    public string responseMimeType;
    public ResponseSchemaStep2 responseSchema;
}


[Serializable]
public struct ResponseSchemaStep2
{
    public string type;
    public PropertiesStep2 properties;
    public string[] required;
}


[Serializable]
public struct PropertiesStep2
{
    public SingleProperty score;
    public SingleProperty motivation;
}


[Serializable]
public struct GeminiGenerationConfigStep3
{
    public string responseMimeType;
    public ResponseSchemaStep3 responseSchema;
}


[Serializable]
public struct ResponseSchemaStep3
{
    public string type;
    public PropertiesStep3 properties;
    public string[] required;
}

[Serializable]
public struct PropertiesStep3
{
    public SingleProperty answer;
}

[Serializable]
public struct SingleProperty
{
    public string type;
}

[Serializable]
public class Candidate
{
    public Content content;
    public string finishReason;
    public double avgLogprobs; }

[Serializable]
public class CandidatesTokensDetail
{
    public string modality;
    public int tokenCount;
}
[Serializable]
public class Content
{
    public List<Part> parts;
    public string role;
}
[Serializable]
public class Part
{
    public string text;
}

[Serializable]
public class PromptTokensDetail
{
    public string modality;
    public int tokenCount;
}

[Serializable]
public class GeminiRoot
{
    public List<Candidate> candidates;
    public UsageMetadata usageMetadata;
    public string modelVersion;
}

public class UsageMetadata
{
    public int promptTokenCount { get; set; }
    public int candidatesTokenCount { get; set; }
    public int totalTokenCount { get; set; }
    public List<PromptTokensDetail> promptTokensDetails { get; set; }
    public List<CandidatesTokensDetail> candidatesTokensDetails { get; set; }
}


        public class APIGemini : APISender
        {

            public string url = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key=";

            public TextAsset key;
            GeminiRoot output;
            GeminiInputStep1 inputStep1;
            GeminiInputStep2 inputStep2;
            GeminiInputStep3 inputStep3;
            string json;
            UnityWebRequest request;

            public UnityEvent<string> error;

            private void Awake()
            {
                inputStep1 = new GeminiInputStep1();
                inputStep1.contents = new GeminiParts[1];
                inputStep1.contents[0].parts = new GeminiText[1];
                inputStep1.generationConfig = new GeminiGenerationConfigStep1();
                inputStep1.generationConfig.responseMimeType = "application/json";
                inputStep1.generationConfig.responseSchema = new ResponseSchemaStep1();
                inputStep1.generationConfig.responseSchema.required = new string[] { "recipeName", "recipeDescription"};
                inputStep1.generationConfig.responseSchema.type = "OBJECT";
                inputStep1.generationConfig.responseSchema.properties = new PropertiesStep1();
                inputStep1.generationConfig.responseSchema.properties.recipeName = new SingleProperty();
                inputStep1.generationConfig.responseSchema.properties.recipeName.type = "string";
                inputStep1.generationConfig.responseSchema.properties.recipeDescription = new SingleProperty();
                inputStep1.generationConfig.responseSchema.properties.recipeDescription.type = "string";


                inputStep2 = new GeminiInputStep2();
                inputStep2.contents = new GeminiParts[1];
                inputStep2.contents[0].parts = new GeminiText[1];
                inputStep2.generationConfig = new GeminiGenerationConfigStep2();
                inputStep2.generationConfig.responseMimeType = "application/json";
                inputStep2.generationConfig.responseSchema = new ResponseSchemaStep2();
                inputStep2.generationConfig.responseSchema.required = new string[] { "score", "motivation" };
                inputStep2.generationConfig.responseSchema.type = "object";
                inputStep2.generationConfig.responseSchema.properties = new PropertiesStep2();
                inputStep2.generationConfig.responseSchema.properties.score = new SingleProperty();
                inputStep2.generationConfig.responseSchema.properties.score.type = "integer";
                inputStep2.generationConfig.responseSchema.properties.motivation = new SingleProperty();
                inputStep2.generationConfig.responseSchema.properties.motivation.type = "string";

                inputStep3 = new GeminiInputStep3();
                inputStep3.contents = new GeminiParts[1];
                inputStep3.contents[0].parts = new GeminiText[1];
                inputStep3.generationConfig = new GeminiGenerationConfigStep3();
                inputStep3.generationConfig.responseSchema.required = new string[] { "answer"};
                inputStep3.generationConfig.responseSchema = new ResponseSchemaStep3();
                inputStep3.generationConfig.responseSchema.type = "object";
                inputStep3.generationConfig.responseSchema.properties = new PropertiesStep3();
                inputStep3.generationConfig.responseMimeType = "application/json";
                inputStep3.generationConfig.responseSchema.properties.answer = new SingleProperty();
                inputStep3.generationConfig.responseSchema.properties.answer.type = "string";


    }

            public void ErrorDefault(string m1, string m2)
            {
                Debug.Log(m1);
                Debug.Log(m2);
            }

            public override void PostStringStep1(string m, Action<string> onSuccess, Action<string, string> onFailure = null)
            {
                inputStep1.contents[0].parts[0].text = m;

                json = JsonUtility.ToJson(inputStep1);
                StartCoroutine(PostStringCoroutine(json, onSuccess, onFailure));
            }
            public override void PostStringStep2(string m, Action<string> onSuccess, Action<string, string> onFailure = null)
            {
                inputStep2.contents[0].parts[0].text = m;

                json = JsonUtility.ToJson(inputStep2);
                StartCoroutine(PostStringCoroutine(json, onSuccess, onFailure));
             }
            public override void PostStringStep3(string m, Action<string> onSuccess, Action<string, string> onFailure = null)
            {
                inputStep3.contents[0].parts[0].text = m;

                json = JsonUtility.ToJson(inputStep3);
                StartCoroutine(PostStringCoroutine(json, onSuccess, onFailure));
            }

            protected IEnumerator PostStringCoroutine(string m, Action<string> onSuccess = null, Action<string, string> onFailure = null)
            {

                request = new UnityWebRequest(url + key.text, "POST");
                request.SetRequestHeader("Content-Type", "application/json");
                byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
                request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                yield return request.SendWebRequest();
                Debug.Log("Status Code: " + request.responseCode);

                if (request.result != UnityWebRequest.Result.Success)
                {
                    onFailure?.Invoke(request.error, request.downloadHandler.text);
                    error.Invoke(request.downloadHandler.text);
                    if (onFailure == null)
                        ErrorDefault(request.error, request.downloadHandler.text);
                }
                else
                {
                    Debug.Log(request.downloadHandler.text);
                    output = JsonUtility.FromJson<GeminiRoot>(request.downloadHandler.text);
                    onSuccess.Invoke(output.candidates[0].content.parts[0].text);
                }
            }
        }

