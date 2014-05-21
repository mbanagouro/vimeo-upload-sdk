using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VimeoAPI.Tests
{
    [TestClass]
    public class GenerateNonceTest
    {
        [TestMethod]
        public void Gerar_hash()
        {
            Guid nonce;
            var resultado = UrlBuilder.GenerateNonce();

            Assert.IsNotNull(resultado);
            Assert.IsTrue(Guid.TryParse(resultado, out nonce));
        }
    }
}
