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

        public Node Analyse( ITokenizer tokenizer )
        {
            //return ReadExpr(tokenizer);

            if (tokenizer.CurrentToken == TokenType.None) tokenizer.GetNextToken();
            return HandleExpression(tokenizer);
        }

        Node HandleExpression(ITokenizer tokenizer)
        {
            Node term = HandleTerm(tokenizer);

            TokenType opToken = tokenizer.CurrentToken;

            Node expr = term;

            if((opToken & TokenType.IsAddOperator) == TokenType.IsAddOperator)
            {
                while((opToken & TokenType.IsAddOperator) == TokenType.IsAddOperator)
                {
                    tokenizer.GetNextToken();
                    Node right = HandleTerm(tokenizer);
                    expr = new BinaryNode(opToken, expr, right);
                    opToken = tokenizer.CurrentToken;
                }
                return expr;
            }
            else 
            {
                return term;
            }
        }

        Node HandleTerm(ITokenizer tokenizer) {
            Node factor = HandleFactor(tokenizer);

            TokenType opToken = tokenizer.CurrentToken;

            Node expr = factor;

            if ((opToken & TokenType.IsMultOperator) == TokenType.IsMultOperator)
            {
                while ((opToken & TokenType.IsMultOperator) == TokenType.IsMultOperator)
                {
                    tokenizer.GetNextToken();
                    Node right = HandleFactor(tokenizer);
                    expr = new BinaryNode(opToken, expr, right);

                    opToken = tokenizer.CurrentToken;
                }

                return expr;
            }
            else
            {
                return factor;
            }

        }

        Node HandleFactor(ITokenizer tokenizer) {
            TokenType t = tokenizer.CurrentToken;

            bool minus = false;
            if (t == TokenType.Minus) { minus = true; tokenizer.GetNextToken(); }

            double v;
            if(tokenizer.MatchDouble(out v))
            {
                return new ConstantNode(minus?-v:v);
            } else if(tokenizer.CurrentToken == TokenType.OpenPar)
            {
                tokenizer.GetNextToken();
                Node d = HandleExpression(tokenizer);
                if(tokenizer.CurrentToken != TokenType.ClosePar)
                {
                    return new ErrorNode(String.Format("Mismatched close paren in factor: Expected CloseParen but for {0}", tokenizer.CurrentToken));
                } else
                {
                    return d;
                }
            } else
            {
                return new ErrorNode(String.Format("Invalid token in factor: Expected Number or OpenParen but got {0}", tokenizer.CurrentToken));
            }
        }

        private Node ReadExpr(ITokenizer tokenizer)
        {
            Node left;
            TokenType operatorToken;
            Node right;
            
            // Left
            TokenType token = tokenizer.GetNextToken();
            double constValue;
            if (token == TokenType.OpenPar)
            {
                left = ReadExpr(tokenizer);
            }
            else if(tokenizer.MatchDouble(out constValue))
            {
                left = new ConstantNode(constValue);
            }
            else
            {
                return new ErrorNode(String.Format("Invalid left token:", tokenizer.CurrentToken.ToString()));
            }

            // Operator
            operatorToken = tokenizer.CurrentToken;

            if ((operatorToken & TokenType.IsAddOperator) == TokenType.IsAddOperator || (operatorToken & TokenType.IsMultOperator) == TokenType.IsMultOperator)
            {
                Debug.Assert(left != null);

                return new BinaryNode(operatorToken, left, ReadExpr(tokenizer));
            }
            //else if ( operatorToken == TokenType.EndOfInput || operatorToken == TokenType.ClosePar )
            //{
            //    return left;
            //}
            else
            {
                if (left == null)
                {
                    return new ErrorNode(String.Format("Invalid operator: {0}", tokenizer.CurrentToken));
                }

                if (operatorToken == TokenType.ClosePar)
                {
                    operatorToken = tokenizer.GetNextToken();
                }
                return left;
            }

        }
    }
}
