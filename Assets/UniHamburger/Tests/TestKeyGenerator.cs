using nkjzm.UniHamburger.Utils;
using NUnit.Framework;
using UnityEngine;

namespace nkjzm.UniHamburger.Tests
{
    public class TestKeyGenerator : MonoBehaviour
    {
        [Test]
        public void 値なしシードなしクラス() => Assert.AreEqual(
            "nkjzm.UniHamburger.Tests.TestKeyGenerator++",
            KeyGenerator.CreateKey("")
        );

        [Test]
        public void 値nullシードnullクラス() => Assert.AreEqual(
            "nkjzm.UniHamburger.Tests.TestKeyGenerator++",
            KeyGenerator.CreateKey(null, null)
        );

        [Test]
        public void 値ありシードありクラス() => Assert.AreEqual(
            "nkjzm.UniHamburger.Tests.TestKeyGenerator+a+b",
            KeyGenerator.CreateKey("a", "b")
        );
    }
}