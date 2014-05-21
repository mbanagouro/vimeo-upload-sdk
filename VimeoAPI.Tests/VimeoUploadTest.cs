using System;
using System.IO;
using VimeoAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VimeoAPI.Tests
{
    [TestClass]
    public class VimeoUploadTest
    {
        [TestMethod]
        public void Realiza_upload_via_api()
        {
            var file = new FileInfo(@"D:\testes\13101022-601.wmv");
            var fileName = Path.GetFileNameWithoutExtension(file.FullName);
            VimeoTicket ticket = Vimeo.Upload(file.FullName, fileName, fileName);

            Assert.IsNotNull(ticket);
            Assert.IsNotNull(ticket.VideoId);
            Assert.AreNotEqual("", ticket.VideoId);
        }
    }
}
