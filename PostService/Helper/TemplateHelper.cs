using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostService.Helper
{
    /// <summary>
    /// Helper methods for associated with message templates
    /// </summary>
    class TemplateHelper
    {
        /// <summary>
        /// Take a HTML template and parse it with provided data
        /// </summary>
        /// <param name="template">The html template file</param>
        /// <param name="data">the data to be populated in the template</param>
        /// <returns></returns>
        public string ParseTemplate(string htmlTemplate, Dictionary<string, string> data)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Helper method to return a PNG image from a HTML template
        /// </summary>
        public void ConvertHtmlToPng()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Helper method to return a PDF from a Html template
        /// </summary>
        public void ConvertHtmlToPdf()
        {
            throw new NotImplementedException();
        }
    }
}
