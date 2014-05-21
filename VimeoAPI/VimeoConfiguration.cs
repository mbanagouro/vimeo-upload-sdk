using System.Configuration;

namespace VimeoAPI
{
    /// <summary>
    /// Configurações da API do Vimeo
    /// </summary>
    public class VimeoConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("accessTokenRequestUrl", DefaultValue = "http://vimeo.com/oauth/access_token", IsRequired = false)]
        public string AccessTokenRequestUrl
        {
            get
            {
                return (string)base["accessTokenRequestUrl"];
            }
            set
            {
                base["accessTokenRequestUrl"] = value;
            }
        }

        [ConfigurationProperty("apiVersion", DefaultValue = "1.0", IsRequired = false)]
        public string APIVersion
        {
            get
            {
                return (string)base["apiVersion"];
            }
            set
            {
                base["apiVersion"] = value;
            }
        }

        [ConfigurationProperty("applicationName", DefaultValue = "", IsRequired = false)]
        public string ApplicationName
        {
            get
            {
                return (string)base["applicationName"];
            }
            set
            {
                base["applicationName"] = value;
            }
        }

        [ConfigurationProperty("authorizationURL", DefaultValue = "http://vimeo.com/oauth/authorize", IsRequired = false)]
        public string AuthorizationURL
        {
            get
            {
                return (string)base["authorizationURL"];
            }
            set
            {
                base["authorizationURL"] = value;
            }
        }

        [ConfigurationProperty("consumerKey", DefaultValue = "", IsRequired = true)]
        public string ConsumerKey
        {
            get
            {
                return (string)base["consumerKey"];
            }
            set
            {
                base["consumerKey"] = value;
            }
        }

        [ConfigurationProperty("consumerSecret", DefaultValue = "", IsRequired = true)]
        public string ConsumerSecret
        {
            get
            {
                return (string)base["consumerSecret"];
            }
            set
            {
                base["consumerSecret"] = value;
            }
        }

        [ConfigurationProperty("maxAllowedTagsCount", DefaultValue = "20", IsRequired = false)]
        public int MaxAllowedTagsCount
        {
            get
            {
                return (int)base["maxAllowedTagsCount"];
            }
            set
            {
                base["maxAllowedTagsCount"] = value;
            }
        }

        [ConfigurationProperty("maxLengthOfEachTag", DefaultValue = "32", IsRequired = false)]
        public int MaxLengthOfEachTag
        {
            get
            {
                return (int)base["maxLengthOfEachTag"];
            }
            set
            {
                base["maxLengthOfEachTag"] = value;
            }
        }

        [ConfigurationProperty("oauthSecret", DefaultValue = "", IsRequired = false)]
        public string OauthSecret
        {
            get
            {
                return (string)base["oauthSecret"];
            }
            set
            {
                base["oauthSecret"] = value;
            }
        }

        [ConfigurationProperty("oauthToken", DefaultValue = "", IsRequired = false)]
        public string OauthToken
        {
            get
            {
                return (string)base["oauthToken"];
            }
            set
            {
                base["oauthToken"] = value;
            }
        }

        [ConfigurationProperty("restAPIUrl", DefaultValue = "http://vimeo.com/api/rest/v2", IsRequired = false)]
        public string RestAPIUrl
        {
            get
            {
                return (string)base["restAPIUrl"];
            }
            set
            {
                base["restAPIUrl"] = value;
            }
        }

        [ConfigurationProperty("signatureMethod", DefaultValue = "HMAC-SHA1", IsRequired = false)]
        public string SignatureMethod
        {
            get
            {
                return (string)base["signatureMethod"];
            }
            set
            {
                base["signatureMethod"] = value;
            }
        }

        [ConfigurationProperty("unauthorizedRequestTokenUrl", DefaultValue = "http://vimeo.com/oauth/request_token", IsRequired = false)]
        public string UnauthorizedRequestTokenUrl
        {
            get
            {
                return (string)base["unauthorizedRequestTokenUrl"];
            }
            set
            {
                base["unauthorizedRequestTokenUrl"] = value;
            }
        }
    }
}
