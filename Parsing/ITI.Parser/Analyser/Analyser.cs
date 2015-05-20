using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public class Analyser
    {
        ITokenizer _tokenizer;

        public Node Analyse( ITokenizer tokenizer )
        {
            _tokenizer = tokenizer;
            if( _tokenizer.CurrentToken == TokenType.None ) _tokenizer.GetNextToken();
            return HandleSuperExpression();
        }

        private Node HandleSuperExpression()
        {
            var left = HandleExpression();
            if (_tokenizer.CurrentToken == TokenType.QuestionMark)
            {
                _tokenizer.GetNextToken();

                Node whenTrue = HandleExpression();

                Debug.Assert(_tokenizer.CurrentToken == TokenType.Colon);
                _tokenizer.GetNextToken();

                Node whenFalse = HandleExpression();

                return new IfNode(left, whenTrue, whenFalse);
            } else {
                return left;
            }
        }

        private Node HandleExpression()
        {
            var left = HandleTerm();
            while (_tokenizer.CurrentToken == TokenType.Plus || _tokenizer.CurrentToken == TokenType.Minus)
            {
                var type = _tokenizer.CurrentToken;
                _tokenizer.GetNextToken();
                left = new BinaryNode(type, left, HandleTerm());
            }
            return left;
        }

        private Node HandleTerm()
        {
            var left = HandleFactor();
            while( _tokenizer.CurrentToken == TokenType.Mult || _tokenizer.CurrentToken == TokenType.Div )
            {
                var type = _tokenizer.CurrentToken;
                _tokenizer.GetNextToken();
                left = new BinaryNode( type, left, HandleFactor() );
            }
            return left;
        }

        private Node HandleFactor()
        {
            bool isNeg = _tokenizer.Match( TokenType.Minus );
            var e = HandlePositiveFactor();
            return isNeg ? new UnaryNode( TokenType.Minus, e ) : e;
        }
        
        private Node HandlePositiveFactor()
        {
            double numberValue;
            if( _tokenizer.MatchDouble( out numberValue ) ) return new ConstantNode( numberValue );
            if( _tokenizer.Match( TokenType.OpenPar ) )
            {
                var e = HandleSuperExpression();
                if( !_tokenizer.Match( TokenType.ClosePar ) ) return new ErrorNode( "Expected )." );
                return e;
            }
            return new ErrorNode( "Expected number or (expression)." );
        }
    }
}
