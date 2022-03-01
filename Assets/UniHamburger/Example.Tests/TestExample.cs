using NUnit.Framework;
using UnityEngine;

namespace nkjzm.UniHamburger.Example.Tests
{
    public class TestExample
    {
        private Example target;

        [SetUp]
        public void Setup() => target = new GameObject().AddComponent<Example>();

        [Test]
        public void 存在する()
        {
            Assert.IsNotNull(target);
        }
    }
}