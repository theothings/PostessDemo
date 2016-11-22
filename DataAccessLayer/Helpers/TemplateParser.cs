using SmartFormat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Helpers
{
    public class TemplateParser
    {
        public string ParseTemplate(string template, object obj)
        {
            return Smart.Format(template ?? "", obj);
        }
    }
}
