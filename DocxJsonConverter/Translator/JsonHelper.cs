using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
namespace DocxJsonConverter.Translator
{
    
        public class JsonHelper
        {
            public static List<TranslationResponse> DeSerializeObject(string ResponseString)
            {
                using (MemoryStream DeSerializememoryStream = new MemoryStream())
                {
                    //Json String that we get from web api 
                    //string ResponseString = "{\"detectedLanguage\":{\"language\":\"en\",\"score\":1.0},\"translations\":[{\"text\":\"A Datum Corporation 1\",\"to\":\"en\"}]}";

                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<TranslationResponse>));

                    //user stream writer to write JSON string data to memory stream
                    StreamWriter writer = new StreamWriter(DeSerializememoryStream);
                    writer.Write(ResponseString);
                    writer.Flush();

                    DeSerializememoryStream.Position = 0;
                    var SerializedObject = (List<TranslationResponse>)serializer.ReadObject(DeSerializememoryStream);
                    return SerializedObject;
                }
            }

            public static string SerializeObject(string text)
            {
                using (MemoryStream SerializememoryStream = new MemoryStream())
                {
                    var translationRequestList = new List<TranslationRequest>();

                    //Create a sample data of type TranslationRequest Class add details
                    TranslationRequest request = new TranslationRequest();
                    request.Text = text;
                    translationRequestList.Add(request);

                    //Initialize DataContractJsonSerializer object and pass TranslationResponse class type to it
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<TranslationRequest>));
                    //write newly created object into memory stream
                    serializer.WriteObject(SerializememoryStream, translationRequestList);
                    SerializememoryStream.Position = 0;
                    //use stream reader to read serialized data from memory stream
                    StreamReader sr = new StreamReader(SerializememoryStream);

                    //get JSON data serialized in string format in string variable 
                    string Serializedresult = sr.ReadToEnd();
                    return Serializedresult;
                }
            }
        }
        public class Language
        {
            public string language;
            public string score;

        }

        public class TranslationHelper
        {
            public string text;
            public string to;
        }
        [DataContract]
        public class TranslationRequest
        {
            [DataMember]
            public string Text { get; set; }

        }
        [DataContract]
        public class TranslationResponse
        {
            [DataMember]
            public Language detectedLanguage { get; set; }

            [DataMember]
            public List<TranslationHelper> translations { get; set; }

        }
    
}
