using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Diagnostics;

namespace ITI.Parser.Tests
{
    [TestFixture]
    public class TokenizerTests
    {
        [TestCase("3")]
        [TestCase("3 8")]
        [TestCase("3 8 12 8787 3712")]
        public void list_of_numbers(string toParse)
        {
            var t = new StringTokenizer(toParse);
            List<double> values = new List<double>();
            t.GetNextToken();
            while (t.CurrentToken != TokenType.EndOfInput)
            {
                Assert.That(t.CurrentToken == TokenType.Number);
                double v;
                Assert.That(t.MatchDouble(out v));
                values.Add(v);
            }
            CollectionAssert.AreEqual(toParse.Split(' ').Select(s => Double.Parse(s)).ToList(), values);
        }

        [Test]
        public void analyserTest()
        {
            Analyser a = new Analyser();
            var tokenizer = new StringTokenizer("3 + 5 * 125 / 7 - 6 + 10");

            Node n = a.Analyse(tokenizer);
            Console.WriteLine(n.ToString());

            a = new Analyser();
            tokenizer = new StringTokenizer("1-1+1");

            n = a.Analyse(tokenizer);
            Console.WriteLine(n.ToString());
            //Debugger.Break();
        }
    }
}
