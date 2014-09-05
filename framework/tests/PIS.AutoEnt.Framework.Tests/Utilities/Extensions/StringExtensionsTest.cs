using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PIS.AutoEnt.Tests.Utilities.Extensions
{
    [TestClass]
    public class StringExtensionsTest
    {
        [TestMethod]
        public void Framework_StringExtensions_ToBoolean()
        {
            bool? rtn = "".ToBoolean();
            Assert.IsNull(rtn);

            bool rtn1 = "".ToBoolean(true);
            Assert.IsTrue(rtn1);

            rtn1 = "true".ToBoolean().Value;
            Assert.IsTrue(rtn1);

            rtn1 = "tRue".ToBoolean().Value;
            Assert.IsTrue(rtn1);

            rtn1 = "True".ToBoolean().Value;
            Assert.IsTrue(rtn1);

            rtn1 = "1".ToBoolean().Value;
            Assert.IsTrue(rtn1);

            rtn1 = "false".ToBoolean().Value;
            Assert.IsFalse(rtn1);

            rtn1 = "False".ToBoolean().Value;
            Assert.IsFalse(rtn1);

            rtn1 = "FAlse".ToBoolean().Value;
            Assert.IsFalse(rtn1);

            rtn1 = "0".ToBoolean().Value;
            Assert.IsFalse(rtn1);

            rtn1 = "2".ToBoolean(false);
            Assert.IsFalse(rtn1);
        }

        [TestMethod]
        public void Framework_StringExtensions_Join()
        {
            IList<object> a = new List<object>();
            a.Add("a");
            a.Add(1);
            a.Add(1.12);
            a.Add(true);

            string rtn = a.Join();
            Assert.AreEqual("a,1,1.12,True", rtn);

            rtn = a.Join(';');
            Assert.AreEqual("a;1;1.12;True", rtn);

            rtn = a.JoinAndQuota("{", "}");
            Assert.AreEqual("{a},{1},{1.12},{True}", rtn);

            rtn = a.JoinAndQuota(';', "{", "}");
            Assert.AreEqual("{a};{1};{1.12};{True}", rtn);

            rtn = a.JoinAndQuota(';');
            Assert.AreEqual("\"a\";\"1\";\"1.12\";\"True\"", rtn);
        }

        [TestMethod]
        public void Framework_StringExtensions_Tmpl()
        {
            string expr = null;

            try
            {
                expr.Trim();
                Assert.Fail();
            }
            catch (NullReferenceException) { }
            catch (Exception) { Assert.Fail(); }

            Assert.AreEqual("abcd".TrimStart("ab"), "cd");
            Assert.AreEqual("abcd".TrimStart("bc"), "abcd");
            Assert.AreEqual("abcd".TrimEnd("ab"), "abcd");
            Assert.AreEqual("abcd".TrimEnd("cd"), "ab");
            Assert.AreEqual(" abc ".Trim(), "abc");
            Assert.AreEqual("abcd".Trim('d'), "abc");
            Assert.AreEqual("abcd".Trim("cd"), "ab");
            Assert.AreEqual("abcd".Trim("ab"), "cd");
            Assert.AreEqual("abcd".Trim("bc"), "abcd");
            Assert.AreEqual("abcd".Trim("x"), "abcd");

            Assert.AreEqual("{abcd}".PeerOff("{", "}"), "abcd");
            Assert.AreEqual("{{abcd}}".PeerOff("{", "}"), "{abcd}");
            Assert.AreEqual("{abcd}a".PeerOff("{", "}"), "abcd}a");
            Assert.AreEqual("b{abcd}}".PeerOff("{", "}"), "b{abcd}");

            Assert.AreEqual("abcd".Wrap("{", "}"), "{abcd}");

            Assert.AreEqual("abcd".Repeat(0), "");
            Assert.AreEqual("abcd".Repeat(1), "a");
            Assert.AreEqual("abcd".Repeat(2), "ab");
            Assert.AreEqual("abcd".Repeat(4), "abcd");
            Assert.AreEqual("abcd".Repeat(6), "abcdab");
            Assert.AreEqual("abcd".Repeat(10), "abcdabcdab");
        }
    }
}
