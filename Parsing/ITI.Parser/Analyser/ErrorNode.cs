﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public class ErrorNode : Node
    {
        public ErrorNode( string message )
        {
            Message = message;
        }

        public string Message { get; private set; }

        public override string ToString()
        {
            return "Error: " + Message;
        }
    }
}
