using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

namespace VimeoAPI
{
    public static class Vimeo
    {
        /// <summary>
        /// Realiza o upload de um vídeo
        /// </summary>
        /// <param name="filePath">Caminho completo do arquivo</param>
        /// <param name="title">Título</param>
        /// <param name="description">Descrição</param>
        /// <param name="tags">Tags</param>
        /// <returns>Ticket de upload Vimeo</returns>
        public static VimeoTicket Upload(string filePath, string title, string description, string tags = "")
        {
            XmlNodeList elementsByTagName;
            XmlNode node;
            XmlAttribute attribute;
            VimeoTicket ticket = new VimeoTicket();
            byte[] buffer = null;

            if (File.Exists(filePath))
            {
                try
                {
                    FileStream stream = File.OpenRead(filePath);
                    buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, Convert.ToInt32(stream.Length));
                    stream.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro durante a leitura do arquivo (Vimeo API)", ex);
                }
            }
            else
            {
                throw new Exception("Arquivo não encontrado (Vimeo API)");
            }

            try
            {
                ticket = CheckVideoUploadQuota();
                if ((((ticket != null) && (ticket.UserId != 0)) && (buffer != null)) && (buffer.Length > 0))
                {
                    if (buffer.Length > ticket.SpaceFree)
                    {
                        throw new Exception("Não há espaço disponível para realizar o upload do arquivo. (Vimeo API)");
                    }
                }
                else
                {
                    throw new Exception("Erro durante a verificação da quota (Vimeo API)");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a verificação da quota (Vimeo API)", ex);
            }

            if (buffer != null)
            {
                ticket.FilePath = filePath;
                ticket.FileSize = buffer.Length;
                XmlDocument document2 = DoHttpRequest(UrlBuilder.BuildUploadTicketUrl());
                if (document2 != null)
                {
                    elementsByTagName = document2.GetElementsByTagName("ticket");
                    if ((elementsByTagName != null) && (elementsByTagName.Count > 0))
                    {
                        node = elementsByTagName[0];
                        attribute = node.Attributes["endpoint"];
                        if (attribute != null)
                        {
                            ticket.EndPoint = attribute.Value;
                        }
                        attribute = node.Attributes["id"];
                        if (attribute != null)
                        {
                            ticket.TicketId = attribute.Value;
                        }
                    }
                }
                if (ticket.EndPoint != string.Empty)
                {
                    PostVideo(ticket.EndPoint, ticket.FilePath, buffer);
                }
            }
            if (ticket.TicketId != string.Empty)
            {
                FileInfo info = new FileInfo(filePath);
                XmlDocument document2 = DoHttpRequest(UrlBuilder.BuildCompleteUploadUrl(info.Name, ticket.TicketId));
                if (document2 != null)
                {
                    elementsByTagName = document2.GetElementsByTagName("ticket");
                    if ((elementsByTagName != null) && (elementsByTagName.Count > 0))
                    {
                        node = elementsByTagName[0];
                        attribute = node.Attributes["video_id"];
                        if (attribute != null)
                        {
                            ticket.VideoId = attribute.Value;
                        }
                    }
                }
            }
            if (ticket.VideoId != string.Empty)
            {
                UpdateVideo(ticket.VideoId, title, description, tags);
            }
            return ticket;
        }

        /// <summary>
        /// Realiza o upload de um vídeo de forma assíncrona
        /// </summary>
        /// <param name="filePath">Caminho completo do arquivo</param>
        /// <param name="title">Título</param>
        /// <param name="description">Descrição</param>
        /// <param name="tags">Tags</param>
        /// <returns>Ticket de upload Vimeo</returns>
        public static async Task<VimeoTicket> UploadAsync(string filePath, string title, string description, string tags)
        {
            XmlNodeList elementsByTagName;
            XmlNode node;
            XmlAttribute attribute;
            VimeoTicket ticket = new VimeoTicket();
            byte[] buffer = null;

            if (File.Exists(filePath))
            {
                try
                {
                    FileStream stream = File.OpenRead(filePath);
                    buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, Convert.ToInt32(stream.Length));
                    stream.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro durante a leitura do arquivo (Vimeo API)", ex);
                }
            }
            else
            {
                throw new Exception("Arquivo não encontrado (Vimeo API)");
            }

            try
            {
                ticket = CheckVideoUploadQuota();
                if ((((ticket != null) && (ticket.UserId != 0)) && (buffer != null)) && (buffer.Length > 0))
                {
                    if (buffer.Length > ticket.SpaceFree)
                    {
                        throw new Exception("Não há espaço disponível para realizar o upload do arquivo. (Vimeo API)");
                    }
                }
                else
                {
                    throw new Exception("Erro durante a verificação da quota (Vimeo API)");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a verificação da quota (Vimeo API)", ex);
            }

            if (buffer != null)
            {
                ticket.FilePath = filePath;
                ticket.FileSize = buffer.Length;
                XmlDocument document2 = DoHttpRequest(UrlBuilder.BuildUploadTicketUrl());
                if (document2 != null)
                {
                    elementsByTagName = document2.GetElementsByTagName("ticket");
                    if ((elementsByTagName != null) && (elementsByTagName.Count > 0))
                    {
                        node = elementsByTagName[0];
                        attribute = node.Attributes["endpoint"];
                        if (attribute != null)
                        {
                            ticket.EndPoint = attribute.Value;
                        }
                        attribute = node.Attributes["id"];
                        if (attribute != null)
                        {
                            ticket.TicketId = attribute.Value;
                        }
                    }
                }
                if (ticket.EndPoint != string.Empty)
                {
                    await PostVideoAsync(ticket.EndPoint, ticket.FilePath, buffer);
                }
            }
            if (ticket.TicketId != string.Empty)
            {
                FileInfo info = new FileInfo(filePath);
                XmlDocument document2 = DoHttpRequest(UrlBuilder.BuildCompleteUploadUrl(info.Name, ticket.TicketId));
                if (document2 != null)
                {
                    elementsByTagName = document2.GetElementsByTagName("ticket");
                    if ((elementsByTagName != null) && (elementsByTagName.Count > 0))
                    {
                        node = elementsByTagName[0];
                        attribute = node.Attributes["video_id"];
                        if (attribute != null)
                        {
                            ticket.VideoId = attribute.Value;
                        }
                    }
                }
            }
            if (ticket.VideoId != string.Empty)
            {
                UpdateVideo(ticket.VideoId, title, description, tags);
            }
            return ticket;
        }

        /// <summary>
        /// Atualiza os dados de um vídeo do Vimeo
        /// </summary>
        /// <param name="videId">Identificador do vídeo no Vimeo</param>
        /// <param name="title">Título</param>
        /// <param name="description">Descrição</param>
        /// <param name="tags">Tags</param>
        /// <returns>True ou False</returns>
        public static bool UpdateVideo(string videId, string title, string description, string tags)
        {
            bool flag = true;
            if ((title != string.Empty) && !UpdateTitle(videId, title))
            {
                flag = false;
            }
            if ((description != string.Empty) && !UpdateDescription(videId, description))
            {
                flag = false;
            }
            if ((tags != string.Empty) && !UpdateTags(videId, tags))
            {
                flag = false;
            }
            return flag;
        }

        /// <summary>
        /// Verifica as cotas de upload disponível 
        /// </summary>
        /// <returns>Ticket de upload do Vimeo</returns>
        public static VimeoTicket CheckVideoUploadQuota()
        {
            XmlDocument document = DoHttpRequest(UrlBuilder.BuildUrlToCheckUsersVideoUploadQuota());
            VimeoTicket ticket = new VimeoTicket();

            if (document != null)
            {
                XmlNode node;
                XmlAttribute attribute;
                XmlNodeList elementsByTagName = document.GetElementsByTagName("user");
                if ((elementsByTagName != null) && (elementsByTagName.Count > 0))
                {
                    node = elementsByTagName[0];
                    attribute = node.Attributes["id"];
                    if (attribute != null)
                    {
                        ticket.UserId = Convert.ToInt32(attribute.Value);
                    }
                }
                elementsByTagName = document.GetElementsByTagName("upload_space");
                if ((elementsByTagName == null) || (elementsByTagName.Count <= 0))
                {
                    return ticket;
                }
                node = elementsByTagName[0];
                attribute = node.Attributes["free"];
                if (attribute != null)
                {
                    ticket.SpaceFree = Convert.ToInt32(attribute.Value);
                }
                attribute = node.Attributes["max"];
                if (attribute != null)
                {
                    ticket.MaxUpload = Convert.ToInt32(attribute.Value);
                }
                attribute = node.Attributes["used"];
                if (attribute != null)
                {
                    ticket.SpaceUsed = Convert.ToInt32(attribute.Value);
                }
            }

            return ticket;
        }

        /// <summary>
        /// Atualiza a descrição de um vídeo no Vimeo
        /// </summary>
        /// <param name="videoId">Identificador do vídeo no Vimeo</param>
        /// <param name="description">Descrição</param>
        /// <returns>True ou False</returns>
        private static bool UpdateDescription(string videoId, string description)
        {
            XmlDocument document = DoHttpRequest(UrlBuilder.BuildUpdateVideoDescriptionUrl(videoId, description));
            if (document != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Atualiza as tags de um vídeo no Vimeo
        /// </summary>
        /// <param name="videoId">Identificador do vídeo no Vimeo</param>
        /// <param name="tags">Tags</param>
        /// <returns>True ou False</returns>
        private static bool UpdateTags(string videoId, string tags)
        {
            if (tags != string.Empty)
            {
                string[] strArray = tags.Split(new char[] { ',' });
                if (strArray.Length > VimeoSettings.MaxAllowedTagsCount)
                {
                    throw new Exception("O número máximo de tags permitidas é " + VimeoSettings.MaxAllowedTagsCount.ToString());
                }
                foreach (string str in strArray)
                {
                    if (str.Length > VimeoSettings.MaxLengthOfEachTag)
                    {
                        throw new Exception("O número mínimo de tags permitidas é " + VimeoSettings.MaxLengthOfEachTag.ToString());
                    }
                }
                if (ClearTags(videoId))
                {
                    XmlDocument document = DoHttpRequest(UrlBuilder.BuildUpdateVideoTagsUrl(videoId, tags));
                    if (document != null)
                    {
                        return true;
                    }
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// Atualiza o título de um vídeo no Vimeo
        /// </summary>
        /// <param name="videoId">Identificador do vídeo no Vimeo</param>
        /// <param name="title">Título</param>
        /// <returns>True ou False</returns>
        private static bool UpdateTitle(string videoId, string title)
        {
            XmlDocument document = DoHttpRequest(UrlBuilder.BuildUpdateVideoTitleUrl(videoId, title));
            if (document != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Limpa as tags de um vídeo no Vimeo
        /// </summary>
        /// <param name="videoId">Identificador do vídeo no Vimeo</param>
        /// <returns>True ou False</returns>
        private static bool ClearTags(string videoId)
        {
            XmlDocument document = DoHttpRequest(UrlBuilder.BuildClearVideoTagsUrl(videoId));
            if (document != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Realiza o upload do vídeo de forma síncrona
        /// </summary>
        /// <param name="endPoint">EndPoint</param>
        /// <param name="fileName">Nome do arquivo</param>
        /// <param name="fileContent">Conteúdo do arquivo</param>
        /// <returns>True ou False</returns>
        private static bool PostVideo(string endPoint, string fileName, byte[] fileContent)
        {
            try
            {
                var response = Post(endPoint, fileName, fileContent).GetResponse();
                var httpResponse = (HttpWebResponse)response;
                return (httpResponse.StatusCode.ToString() == "OK");
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Realiza o upload do vídeo de forma assíncrona
        /// </summary>
        /// <param name="endPoint">EndPoint</param>
        /// <param name="fileName">Nome do arquivo</param>
        /// <param name="fileContent">Conteúdo do arquivo</param>
        /// <returns>True ou False</returns>
        private static async Task<bool> PostVideoAsync(string endPoint, string fileName, byte[] fileContent)
        {
            try
            {
                var response = await Post(endPoint, fileName, fileContent).GetResponseAsync();
                var httpResponse = (HttpWebResponse)response;
                return (httpResponse.StatusCode.ToString() == "OK");
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Realiza o upload do vídeo
        /// </summary>
        /// <param name="endPoint">EndPoint</param>
        /// <param name="fileName">Nome do arquivo</param>
        /// <param name="fileContent">Conteúdo do arquivo</param>
        /// <returns>HttpWebRequest</returns>
        private static HttpWebRequest Post(string endPoint, string fileName, byte[] fileContent)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endPoint);
                request.Method = "PUT";
                request.ContentType = GetContentType(fileName);
                request.ContentLength = fileContent.Length;
                request.KeepAlive = true;
                request.Timeout = fileContent.Length;
                request.ReadWriteTimeout = fileContent.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContent, 0, fileContent.Length);
                requestStream.Close();
                return request;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Obtém o content type do arquivo
        /// </summary>
        /// <param name="filePath">Caminho completo do arquivo</param>
        /// <returns>Content Type</returns>
        private static string GetContentType(string filePath)
        {
            string str = string.Empty;
            string extension = Path.GetExtension(filePath);
            if (extension != string.Empty)
            {
                switch (extension.ToLower())
                {
                    case ".wmv":
                        return "video/x-ms-wmv";

                    case ".flv":
                        return "video/x-flv";

                    case ".mp4":
                        str = "video/mp4";
                        break;
                }
            }
            return str;
        }

        /// <summary>
        /// Realiza um request para uma determinada url
        /// </summary>
        /// <param name="url">Url</param>
        /// <returns>XmlDocument</returns>
        private static XmlDocument DoHttpRequest(string url)
        {
            string str;
            WebClient client = new WebClient();

            try
            {
                using (Stream stream = client.OpenRead(url))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        var task = reader.ReadToEndAsync();
                        str = task.Result;
                    }
                }
                VerifyLimit(client.ResponseHeaders);
            }
            catch (WebException exception)
            {
                if (!(exception.Response is HttpWebResponse) || ((exception.Response as HttpWebResponse).StatusCode != HttpStatusCode.NotFound))
                {
                    throw;
                }
                str = null;
            }
            finally
            {
                if (client != null)
                {
                    client.Dispose();
                }
            }

            XmlDocument document = null;
            if (str != string.Empty)
            {
                document = new XmlDocument();
                document.LoadXml(str);
                VerifyResponseError(document);
            }

            return document;
        }

        /// <summary>
        /// Verifica se o limite de requisições à API do Vimeo não foi atingida
        /// </summary>
        /// <param name="client">WebClient</param>
        private static void VerifyLimit(WebHeaderCollection headers)
        {
            if (headers == null)
                return;

            long limit = 0;
            long limitRemaining = 0;
            long limitReset = 0;
            if (Int64.TryParse(headers["X-RateLimit-Limit"], out limit) &&
                Int64.TryParse(headers["X-RateLimit-Remaining"], out limitRemaining) &&
                Int64.TryParse(headers["X-RateLimit-Reset"], out limitReset))
            {
                if (limitRemaining <= 0)
                {
                    throw new Exception(String.Format("Limit requests exceeded (Limit: {0} / Limit-Remaining: {1} / Limit-Reset: {2})", limit, limitRemaining, limitReset)); 
                }
            }
        }

        /// <summary>
        /// Verfica se a resposta à chamada da API do Vimeo não retornou erros
        /// </summary>
        /// <param name="document">XmlDocument</param>
        private static void VerifyResponseError(XmlDocument document)
        {
            XmlNodeList elementsByTagName = document.GetElementsByTagName("rsp");
            if ((elementsByTagName != null) && (elementsByTagName.Count > 0))
            {
                XmlNode node = elementsByTagName[0];
                XmlAttribute attribute = node.Attributes["stat"];
                if ((attribute != null) && (attribute.Value == "ok"))
                {
                    return;
                }
            }

            var erroResponse = new VimeoErrorResponse();

            elementsByTagName = document.GetElementsByTagName("err");
            if ((elementsByTagName != null) && (elementsByTagName.Count > 0))
            {
                var node = elementsByTagName[0];
                var attribute = node.Attributes["code"];
                if (attribute != null)
                {
                    erroResponse.Code = attribute.Value;
                }
                attribute = node.Attributes["msg"];
                if (attribute != null)
                {
                    erroResponse.Message = attribute.Value;
                }
                attribute = node.Attributes["expl"];
                if (attribute != null)
                {
                    erroResponse.Explanation = attribute.Value;
                }

                throw new Exception(erroResponse.ToString());
            }
        }
    }
}
