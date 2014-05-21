using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VimeoAPI
{
    /// <summary>
    /// Informações de erro do Vimeo
    /// </summary>
    public class VimeoErrorResponse
    {
        public string Code { get; set; }

        public string Message { get; set; }

        public string Explanation { get; set; }

        public override string ToString()
        {
            return String.Format("code: {0} {1}: {2}", Code, Message, Explanation);
        }
    }
}
