using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

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

        [TestCase("3+ 8")]
        [TestCase("   2     +   2  ")]
        public void plus_operation(string toParse)
        {
            var t = new StringTokenizer(toParse);
            List<double> values = new List<double>();
            Assert.That(t.GetNextToken(), Is.EqualTo(TokenType.Number));
            Assert.That(t.GetNextToken(), Is.EqualTo(TokenType.Plus));
            Assert.That(t.GetNextToken(), Is.EqualTo(TokenType.Number));
            Assert.That(t.GetNextToken(), Is.EqualTo(TokenType.EndOfInput));
        }

        [TestCase(@"4/2 // comment")]
        [TestCase(@"4/ // comment
2")]
        [TestCase(@"4// comment /
/2")]
        [TestCase(@"4/2 /* comment")]
        [TestCase(@"4 /* 2
2
2*//2")]
        [TestCase(@"4 /*/*//2")]
        [TestCase(@"4 /**/ / 2 // 2")]
        [TestCase(@"4/2 /* comment *")]
        [TestCase(@"4/2 /")]
        [TestCase(@"4/2 //")]
        public void comments(string toParse)
        {
            var t = new StringTokenizer(toParse);
            Assert.That(t.GetNextToken(), Is.EqualTo(TokenType.Number));
            Assert.That(t.GetNextToken(), Is.EqualTo(TokenType.Div));
            Assert.That(t.GetNextToken(), Is.EqualTo(TokenType.Number));
            Assert.That(t.GetNextToken(), Is.EqualTo(TokenType.EndOfInput));
        }

        [Test]
        public void punctuation_tokens()
        {
            string toParse = "4;:,::.[]{}:";
            var t = new StringTokenizer(toParse);
            Assert.That(t.GetNextToken(), Is.EqualTo(TokenType.Number));
            Assert.That(t.GetNextToken(), Is.EqualTo(TokenType.SemiColon));
            Assert.That(t.GetNextToken(), Is.EqualTo(TokenType.Colon));
            Assert.That(t.GetNextToken(), Is.EqualTo(TokenType.Comma));
            Assert.That(t.GetNextToken(), Is.EqualTo(TokenType.DoubleColon));
            Assert.That(t.GetNextToken(), Is.EqualTo(TokenType.Dot));
            Assert.That(t.GetNextToken(), Is.EqualTo(TokenType.OpenSquare));
            Assert.That(t.GetNextToken(), Is.EqualTo(TokenType.CloseSquare));
            Assert.That(t.GetNextToken(), Is.EqualTo(TokenType.OpenBracket));
            Assert.That(t.GetNextToken(), Is.EqualTo(TokenType.CloseBracket));
            Assert.That(t.GetNextToken(), Is.EqualTo(TokenType.Colon));
            Assert.That(t.GetNextToken(), Is.EqualTo(TokenType.EndOfInput));
        }

        [Test]
        public void identifier_token()
        {
            string toParse = "4 _lol 4lol";
            string v;
            var t = new StringTokenizer(toParse);
            Assert.That(t.GetNextToken(), Is.EqualTo(TokenType.Number));
            Assert.That(t.GetNextToken(), Is.EqualTo(TokenType.Identifier));
            Assert.That(t.MatchString(out v), Is.True);
            Assert.That(v, Is.EqualTo( @"_lol" ));
            Assert.That(t.CurrentToken, Is.EqualTo(TokenType.Error));

        }


    }
}
