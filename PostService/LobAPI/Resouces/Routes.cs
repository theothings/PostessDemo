﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostService.LobAPI.Resouces
{
    public class Routes : Resource
    {
        public Routes(Lob lob) : base(lob)
        {
        }

        protected override string ResourcePathName
        {
            get
            {
                return "routes";
            }
        }
    }
}
