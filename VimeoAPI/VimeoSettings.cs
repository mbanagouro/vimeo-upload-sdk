using System;
using System.Configuration;

namespace VimeoAPI
{
    /// <summary>
    /// 
    /// </summary>
    public static class VimeoSettings
    {
        static VimeoConfiguration currentSettings;
        static string oauthSecret = string.Empty;
        static string oauthToken = string.Empty;

        public static string AccessPermission
        {
            get
            {
                return "write";
            }
        }

        public static string AccessTokenRequestUrl
        {
            get
            {
                if ((CurrentSettings != null) && (CurrentSettings.AccessTokenRequestUrl != string.Empty))
                {
                    return CurrentSettings.AccessTokenRequestUrl;
                }
                return "http://vimeo.com/oauth/access_token";
            }
        }

        public static string APIVersion
        {
            get
            {
                if ((CurrentSettings != null) && (CurrentSettings.APIVersion != string.Empty))
                {
                    return CurrentSettings.APIVersion;
                }
                return "1.0";
            }
        }

        public static string ApplicationName
        {
            get
            {
                if (CurrentSettings != null)
                {
                    return CurrentSettings.ApplicationName;
                }
                return string.Empty;
            }
        }

        public static string AuthorizationURL
        {
            get
            {
                if ((CurrentSettings != null) && (CurrentSettings.AuthorizationURL != string.Empty))
                {
                    return CurrentSettings.AuthorizationURL;
                }
                return "http://vimeo.com/oauth/authorize";
            }
        }

        public static string ConsumerKey
        {
            get
            {
                if ((CurrentSettings == null) || !(CurrentSettings.ConsumerKey != string.Empty))
                {
                    throw new Exception("Consumer Key não está configurado (Vimeo API)");
                }
                return CurrentSettings.ConsumerKey;
            }
        }

        public static string ConsumerSecret
        {
            get
            {
                if ((CurrentSettings == null) || !(CurrentSettings.ConsumerSecret != string.Empty))
                {
                    throw new Exception("Consumer Secret não está configurado (Vimeo API)");
                }
                return CurrentSettings.ConsumerSecret;
            }
        }

        private static VimeoConfiguration CurrentSettings
        {
            get
            {
                if (currentSettings == null)
                {
                    currentSettings = (VimeoConfiguration)ConfigurationManager.GetSection("vimeoAPI");
                }
                return currentSettings;
            }
        }

        public static int MaxAllowedTagsCount
        {
            get
            {
                if ((CurrentSettings != null) && (CurrentSettings.MaxAllowedTagsCount > 0))
                {
                    return CurrentSettings.MaxAllowedTagsCount;
                }
                return 20;
            }
        }

        public static int MaxLengthOfEachTag
        {
            get
            {
                if ((CurrentSettings != null) && (CurrentSettings.MaxLengthOfEachTag > 0))
                {
                    return CurrentSettings.MaxLengthOfEachTag;
                }
                return 0x20;
            }
        }

        public static string OauthCallback
        {
            get
            {
                return "oob";
            }
        }

        public static string OauthSecret
        {
            get
            {
                if (oauthSecret == string.Empty)
                {
                    if ((CurrentSettings == null) || !(CurrentSettings.OauthSecret != string.Empty))
                    {
                        throw new Exception("Oauth Secret não está configurado (Vimeo API)");
                    }
                    return CurrentSettings.OauthSecret;
                }
                return oauthSecret;
            }
            set
            {
                oauthSecret = value;
            }
        }

        public static string OauthToken
        {
            get
            {
                if (oauthToken == string.Empty)
                {
                    if ((CurrentSettings == null) || !(CurrentSettings.OauthToken != string.Empty))
                    {
                        throw new Exception("Oauth Token não está configurado (Vimeo API)");
                    }
                    return CurrentSettings.OauthToken;
                }
                return oauthToken;
            }
            set
            {
                oauthToken = value;
            }
        }

        public static string RestAPIUrl
        {
            get
            {
                if ((CurrentSettings != null) && (CurrentSettings.RestAPIUrl != string.Empty))
                {
                    return CurrentSettings.RestAPIUrl;
                }
                return "http://vimeo.com/api/rest/v2";
            }
        }

        public static string SignatureMethod
        {
            get
            {
                if ((CurrentSettings != null) && (CurrentSettings.SignatureMethod != string.Empty))
                {
                    return CurrentSettings.SignatureMethod;
                }
                return "HMAC-SHA1";
            }
        }

        public static string UnauthorizedRequestTokenUrl
        {
            get
            {
                if ((CurrentSettings != null) && (CurrentSettings.UnauthorizedRequestTokenUrl != string.Empty))
                {
                    return CurrentSettings.UnauthorizedRequestTokenUrl;
                }
                return "http://vimeo.com/oauth/request_token";
            }
        }

        public static string UploadMethodStreaming
        {
            get
            {
                return "streaming";
            }
        }

        public class Constants
        {
            public const string OAUTH_PARAMETER_PREFIX = "oauth_";
            public const string SIGNATURE_HTTPMETHOD = "GET";
            public const string URL_SPECIAL_CHARS = "!*'();:@&=+$,/?%#[] ";
            public const string URLPARAM_CALLBACK = "oauth_callback";
            public const string URLPARAM_CONSUMERKEY = "oauth_consumer_key";
            public const string URLPARAM_DESCRIPTION = "description";
            public const string URLPARAM_FILENAME = "filename";
            public const string URLPARAM_METHODNAME = "method";
            public const string URLPARAM_NONCE = "oauth_nonce";
            public const string URLPARAM_PERMISSION = "permission";
            public const string URLPARAM_SECRET = "oauth_token_secret";
            public const char URLPARAM_SEPERATOR = '&';
            public const string URLPARAM_SIGNATURE = "oauth_signature";
            public const string URLPARAM_SIGNATUREMETHOD = "oauth_signature_method";
            public const char URLPARAM_STARTER = '?';
            public const string URLPARAM_TAGS = "tags";
            public const string URLPARAM_TICKETID = "ticket_id";
            public const string URLPARAM_TIMESTAMP = "oauth_timestamp";
            public const string URLPARAM_TITLE = "title";
            public const string URLPARAM_TOKEN = "oauth_token";
            public const string URLPARAM_UPLOADMETHOD = "upload_method";
            public const string URLPARAM_VERIFIER = "oauth_verifier";
            public const string URLPARAM_VERSION = "oauth_version";
            public const string URLPARAM_VIDEOID = "video_id";
        }

        public class MethodNames
        {
            public const string CLEARTAGS = "vimeo.videos.clearTags";
            public const string COMPLETEUPLOAD = "vimeo.videos.upload.complete";
            public const string GETQUOTA = "vimeo.videos.upload.getQuota";
            public const string UPDATEDESCRIPTION = "vimeo.videos.setDescription";
            public const string UPDATETAGS = "vimeo.videos.addTags";
            public const string UPDATETITLE = "vimeo.videos.setTitle";
            public const string UPLOADTICKET = "vimeo.videos.upload.getTicket";
        }
    }
}
