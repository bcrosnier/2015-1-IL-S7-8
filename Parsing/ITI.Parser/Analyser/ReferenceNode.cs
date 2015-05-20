using System;

namespace ITI.Parser
{
    public class ReferenceNode : Node
    {
        public ReferenceNode( string name )
        {
            Name = name;
        }

        public string Name { get; private set; }

        internal override void Accept( NodeVisitor visitor )
        {
            visitor.Visit( this );
        }
    }
}