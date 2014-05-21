using System;
using System.Globalization;
using System.Text;
using System.Web;

namespace VimeoAPI
{
    /// <summary>
    /// Construtor de url para chamadas aos métodos da API do Vimeo
    /// </summary>
    public static class UrlBuilder
    {
        public static string BuildClearVideoTagsUrl(string videoId)
        {
            ValidateOauthKeys();
            return BuildUrl(VimeoSettings.RestAPIUrl, VimeoSettings.OauthToken, VimeoSettings.OauthSecret, string.Empty, "vimeo.videos.clearTags", UploadMethod.UnDefined, string.Empty, string.Empty, false, videoId, string.Empty, string.Empty, string.Empty);
        }

        public static string BuildCompleteUploadUrl(string videoFileName, string uploadTicket)
        {
            ValidateOauthKeys();
            return BuildUrl(VimeoSettings.RestAPIUrl, VimeoSettings.OauthToken, VimeoSettings.OauthSecret, string.Empty, "vimeo.videos.upload.complete", UploadMethod.UnDefined, videoFileName, uploadTicket, false, string.Empty, string.Empty, string.Empty, string.Empty);
        }

        public static string BuildUpdateVideoDescriptionUrl(string videoId, string description)
        {
            ValidateOauthKeys();
            return BuildUrl(VimeoSettings.RestAPIUrl, VimeoSettings.OauthToken, VimeoSettings.OauthSecret, string.Empty, "vimeo.videos.setDescription", UploadMethod.UnDefined, string.Empty, string.Empty, false, videoId, string.Empty, description, string.Empty);
        }

        public static string BuildUpdateVideoTagsUrl(string videoId, string tags)
        {
            ValidateOauthKeys();
            return BuildUrl(VimeoSettings.RestAPIUrl, VimeoSettings.OauthToken, VimeoSettings.OauthSecret, string.Empty, "vimeo.videos.addTags", UploadMethod.UnDefined, string.Empty, string.Empty, false, videoId, string.Empty, string.Empty, tags);
        }

        public static string BuildUpdateVideoTitleUrl(string videoId, string title)
        {
            ValidateOauthKeys();
            return BuildUrl(VimeoSettings.RestAPIUrl, VimeoSettings.OauthToken, VimeoSettings.OauthSecret, string.Empty, "vimeo.videos.setTitle", UploadMethod.UnDefined, string.Empty, string.Empty, false, videoId, title, string.Empty, string.Empty);
        }

        public static string BuildUploadTicketUrl()
        {
            ValidateOauthKeys();
            return BuildUrl(VimeoSettings.RestAPIUrl, VimeoSettings.OauthToken, VimeoSettings.OauthSecret, string.Empty, "vimeo.videos.upload.getTicket", UploadMethod.Streaming, string.Empty, string.Empty, false, string.Empty, string.Empty, string.Empty, string.Empty);
        }

        public static string BuildUrl(string url, string authToken, string authSecret, string authVerifier, string methodName, UploadMethod videoPostMethod, string fileName, string ticketId, bool authCallback, string videoId, string title, string description, string tags)
        {
            string nonce = GenerateNonce();
            string timeStamp = GenerateTimeStamp();
            Uri uri = new Uri(url);
            string paramValue = HttpUtility.UrlEncode(SignatureBuilder.GenerateSignature(uri, VimeoSettings.ConsumerKey, VimeoSettings.ConsumerSecret, authToken, authSecret, timeStamp, nonce, authVerifier, methodName, videoPostMethod, fileName, ticketId, authCallback, videoId, title, description, tags));
            StringBuilder builder = new StringBuilder(url);
            if (authCallback)
            {
                builder.Append(AddUrlParameter('?', "oauth_callback", VimeoSettings.OauthCallback));
                builder.Append(AddUrlParameter('&', "oauth_consumer_key", VimeoSettings.ConsumerKey));
            }
            else
            {
                builder.Append(AddUrlParameter('?', "oauth_consumer_key", VimeoSettings.ConsumerKey));
            }
            builder.Append(AddUrlParameter('&', "oauth_nonce", nonce));
            builder.Append(AddUrlParameter('&', "oauth_timestamp", timeStamp));
            builder.Append(AddUrlParameter('&', "oauth_signature_method", VimeoSettings.SignatureMethod));
            if (authToken != string.Empty)
            {
                builder.Append(AddUrlParameter('&', "oauth_token", authToken));
            }
            if (authVerifier != string.Empty)
            {
                builder.Append(AddUrlParameter('&', "oauth_verifier", authVerifier));
            }
            if (methodName != string.Empty)
            {
                builder.Append(AddUrlParameter('&', "method", methodName));
            }
            if (videoPostMethod == UploadMethod.Streaming)
            {
                builder.Append(AddUrlParameter('&', "upload_method", VimeoSettings.UploadMethodStreaming));
            }
            if (fileName != string.Empty)
            {
                builder.Append(AddUrlParameter('&', "filename", fileName));
            }
            if (ticketId != string.Empty)
            {
                builder.Append(AddUrlParameter('&', "ticket_id", ticketId));
            }
            if (videoId != string.Empty)
            {
                builder.Append(AddUrlParameter('&', "video_id", videoId));
            }
            if (title != string.Empty)
            {
                builder.Append(AddUrlParameter('&', "title", title));
            }
            if (description != string.Empty)
            {
                builder.Append(AddUrlParameter('&', "description", description));
            }
            if (tags != string.Empty)
            {
                builder.Append(AddUrlParameter('&', "tags", tags));
            }
            builder.Append(AddUrlParameter('&', "oauth_version", VimeoSettings.APIVersion));
            if (paramValue != string.Empty)
            {
                builder.Append(AddUrlParameter('&', "oauth_signature", paramValue));
            }
            return builder.ToString();
        }

        public static string BuildUrlToCheckUsersVideoUploadQuota()
        {
            if (VimeoSettings.OauthToken == string.Empty)
            {
                throw new Exception("OAuth Token não encontrado. Por favor, configure-o (Vimeo API)");
            }
            if (VimeoSettings.OauthSecret == string.Empty)
            {
                throw new Exception("OAuth Secret não encontrado. Por favor, configure-o (Vimeo API)");
            }
            return BuildUrl(VimeoSettings.RestAPIUrl, VimeoSettings.OauthToken, VimeoSettings.OauthSecret, string.Empty, "vimeo.videos.upload.getQuota", UploadMethod.UnDefined, string.Empty, string.Empty, false, string.Empty, string.Empty, string.Empty, string.Empty);
        }

        private static CultureInfo GetCulture()
        {
            return CultureInfo.GetCultureInfo("en-US");
        }

        private static void ValidateOauthKeys()
        {
            if (VimeoSettings.OauthToken == string.Empty)
            {
                throw new Exception("OAuth Token não encontrado. Por favor, configure-o (Vimeo API)");
            }
            if (VimeoSettings.OauthSecret == string.Empty)
            {
                throw new Exception("OAuth Secret não encontrado. Por favor, configure-o (Vimeo API)");
            }
        }

        private static string AddUrlParameter(char initials, string paramName, string paramValue)
        {
            if (initials != ' ')
            {
                paramName = initials + paramName;
            }
            return (paramName + "=" + paramValue);
        }

        private static string GenerateTimeStamp()
        {
            string str = DateTime.Now.Subtract(new DateTime(0x7b2, 1, 1, 0, 0, 0, 0)).TotalSeconds.ToString(UrlBuilder.GetCulture());
            return str.Substring(0, str.IndexOf("."));
        }

        internal static string GenerateNonce()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
