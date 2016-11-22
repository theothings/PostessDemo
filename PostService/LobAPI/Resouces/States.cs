using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostService.LobAPI.Resouces
{
    public class States : Resource
    {
        public States(Lob lob) : base(lob)
        {
        }

        protected override string ResourcePathName
        {
            get
            {
                return "states";
            }
        }
    }
}
