using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace VimeoAPI
{
    /// <summary>
    /// Construtor de assinatura da url da API do Vimeo
    /// </summary>
    public static class SignatureBuilder
    {
        private static string BuildUrlQueryString(IList<SignatureParameter> parameters)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < parameters.Count; i++)
            {
                SignatureParameter parameter = parameters[i];
                builder.AppendFormat("{0}={1}", parameter.Name, parameter.Value);
                if (i < (parameters.Count - 1))
                {
                    builder.Append("&");
                }
            }
            return builder.ToString();
        }

        private static string ConvertToHash(HashAlgorithm hashAlgorithm, string data)
        {
            if (hashAlgorithm == null)
            {
                throw new ArgumentNullException("hashAlgorithm");
            }
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException("data");
            }
            byte[] bytes = Encoding.ASCII.GetBytes(data);
            return Convert.ToBase64String(hashAlgorithm.ComputeHash(bytes));
        }

        private static string EncodeUrl(string value)
        {
            StringBuilder builder = new StringBuilder();
            foreach (char ch in value)
            {
                if ("!*'();:@&=+$,/?%#[] ".IndexOf(ch) != -1)
                {
                    builder.Append('%' + string.Format("{0:X2}", (int)ch));
                }
                else
                {
                    builder.Append(ch);
                }
            }
            return builder.ToString();
        }

        public static string GenerateSignature(Uri url, string consumerKey, string consumerSecret, string token, string tokenSecret, string timeStamp, string nonce, string oauth_verifier, string methodName, UploadMethod videoPostMethod, string fileName, string ticketId, bool oauthCallback, string videoId, string title, string description, string tags)
        {
            string data = GenerateSignatureBase(url, consumerKey, token, tokenSecret, timeStamp, nonce, oauth_verifier, methodName, videoPostMethod, fileName, ticketId, oauthCallback, videoId, title, description, tags);
            HMACSHA1 hashAlgorithm = new HMACSHA1
            {
                Key = Encoding.ASCII.GetBytes(string.Format("{0}&{1}", EncodeUrl(consumerSecret), string.IsNullOrEmpty(tokenSecret) ? "" : EncodeUrl(tokenSecret)))
            };
            return ConvertToHash(hashAlgorithm, data);
        }

        private static string GenerateSignatureBase(Uri url, string consumerKey, string token, string tokenSecret, string timeStamp, string nonce, string oauthVerifier, string methodName, UploadMethod videoPostMethod, string fileName, string ticketId, bool oauthCallback, string videoId, string title, string description, string tags)
        {
            List<SignatureParameter> urlParameters = GetUrlParameters(url.Query);
            urlParameters.Add(new SignatureParameter("oauth_version", VimeoSettings.APIVersion));
            urlParameters.Add(new SignatureParameter("oauth_nonce", nonce));
            urlParameters.Add(new SignatureParameter("oauth_timestamp", timeStamp));
            urlParameters.Add(new SignatureParameter("oauth_signature_method", VimeoSettings.SignatureMethod));
            urlParameters.Add(new SignatureParameter("oauth_consumer_key", consumerKey));
            if (oauthCallback)
            {
                urlParameters.Add(new SignatureParameter("oauth_callback", VimeoSettings.OauthCallback));
            }
            if (methodName != string.Empty)
            {
                urlParameters.Add(new SignatureParameter("method", methodName));
            }
            if (videoPostMethod == UploadMethod.Streaming)
            {
                urlParameters.Add(new SignatureParameter("upload_method", VimeoSettings.UploadMethodStreaming));
            }
            if (oauthVerifier != string.Empty)
            {
                urlParameters.Add(new SignatureParameter("oauth_verifier", oauthVerifier));
            }
            if (fileName != string.Empty)
            {
                fileName = EncodeUrl(fileName);
                urlParameters.Add(new SignatureParameter("filename", fileName));
            }
            if (ticketId != string.Empty)
            {
                urlParameters.Add(new SignatureParameter("ticket_id", ticketId));
            }
            if (token != string.Empty)
            {
                urlParameters.Add(new SignatureParameter("oauth_token", token));
            }
            if (videoId != string.Empty)
            {
                urlParameters.Add(new SignatureParameter("video_id", videoId));
            }
            if (title != string.Empty)
            {
                title = EncodeUrl(title);
                urlParameters.Add(new SignatureParameter("title", title));
            }
            if (description != string.Empty)
            {
                description = EncodeUrl(description);
                urlParameters.Add(new SignatureParameter("description", description));
            }
            if (tags != string.Empty)
            {
                tags = EncodeUrl(tags);
                urlParameters.Add(new SignatureParameter("tags", tags));
            }
            urlParameters.Sort(new SignatureParameterComparer());
            string str = string.Format("{0}://{1}", url.Scheme, url.Host);
            if (((url.Scheme != "http") || (url.Port != 80)) && ((url.Scheme != "https") || (url.Port != 0x1bb)))
            {
                str = str + ":" + url.Port;
            }
            str = str + url.AbsolutePath;
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("{0}&", "GET");
            builder.AppendFormat("{0}&", EncodeUrl(str));
            builder.AppendFormat("{0}", EncodeUrl(BuildUrlQueryString(urlParameters)));
            return builder.ToString();
        }

        private static List<SignatureParameter> GetUrlParameters(string urlParameters)
        {
            if (urlParameters.StartsWith("?"))
            {
                urlParameters = urlParameters.Remove(0, 1);
            }
            List<SignatureParameter> list = new List<SignatureParameter>();
            if (!string.IsNullOrEmpty(urlParameters))
            {
                string[] strArray = urlParameters.Split(new char[] { '&' });
                foreach (string str in strArray)
                {
                    if (!string.IsNullOrEmpty(str) && !str.StartsWith("oauth_"))
                    {
                        if (str.IndexOf('=') > -1)
                        {
                            string[] strArray2 = str.Split(new char[] { '=' });
                            list.Add(new SignatureParameter(strArray2[0], strArray2[1]));
                        }
                        else
                        {
                            list.Add(new SignatureParameter(str, string.Empty));
                        }
                    }
                }
            }
            return list;
        }
    }

    public class SignatureParameter
    {
        public SignatureParameter(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        public string Name { get; private set; }

        public string Value { get; private set; }
    }

    public class SignatureParameterComparer : IComparer<SignatureParameter>
    {
        public int Compare(SignatureParameter x, SignatureParameter y)
        {
            return ((x.Name == y.Name) ? string.Compare(x.Value, y.Value) : string.Compare(x.Name, y.Name));
        }
    }
}
