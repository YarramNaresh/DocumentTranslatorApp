using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace DocxJsonConverter.Translator
{
    public class TextTranslator
    {
        public static string TranslateText(string text, string fromLanguage, string toLanguage)
        {
            try
            {
                string translatedText = String.Empty;
                List<TranslationHelper> translations = new List<TranslationHelper>();
                var url = GetTranslateUriV3Api(fromLanguage, toLanguage);
                var requestBody = JsonHelper.SerializeObject(text);
                string subscriptionKey = "8a40b76588cb4b958c9d8ccd86fd8121";

                using (var client = new HttpClient())
                using (var request = new HttpRequestMessage())
                {
                    request.Method = HttpMethod.Post;
                    request.RequestUri = new Uri(url);
                    //"https://api.cognitive.microsofttranslator.com/translate?api-version=3.0&to=ar,zh-Hant,cs,da,de,el,fi,fr,he,it,ja,ko,nl,nb,pl,pt,ru,sv,th,tr,id,sl,vi,es"
                    request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                    request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                    var response = client.SendAsync(request);
                    if (response.Result != null)
                    {
                        var responseBody = response.Result.Content.ReadAsStringAsync().Result;
                        var result = JsonHelper.DeSerializeObject(responseBody);
                        translations = result[0].translations;
                        translatedText = (string)translations[0].text;
                    }
                    return translatedText;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private static string GetTranslateUriV3Api(string fromLanguage, string toLanguage)
        {
            string path = "translate?api-version=3.0";
            string BaseUrl = "https://api.cognitive.microsofttranslator.com/";
            var url = BaseUrl + path + "&from=" + fromLanguage;
            url = url + "&to=" + toLanguage;
            //if (toLanguages.Length > 0) // if more to language are more than one.
            //    url += String.Join<string>(",", toLanguages);

            return url;
        }
    }
}
