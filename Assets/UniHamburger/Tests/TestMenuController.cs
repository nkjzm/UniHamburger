using NUnit.Framework;

namespace nkjzm.UniHamburger.Tests
{
    public class TestMenuController
    {
        private MenuController target;
        private MenuController targetPrefab;
        private static string FileName => "UniHamburger";

        [OneTimeSetUp]
        public void OneTimeSetup() => targetPrefab = Utils.LoadPrefab<MenuController>(FileName);

        [SetUp]
        public void Setup() => target = UnityEngine.Object.Instantiate(targetPrefab);

        [Test]
        public void 存在する() => Assert.IsNotNull(target);

        [Test]
        public void 初期に開く()
        {
            target.OpenOnInit = true;
            target.Start();
            Assert.AreEqual(true, target.IsExpand);
        }

        [Test]
        public void 初期に閉じる()
        {
            target.OpenOnInit = false;
            target.Start();
            Assert.AreEqual(false, target.IsExpand);
        }

        [Test]
        public void Switchで切り替わる()
        {
            初期に開く();
            target.Switch();
            Assert.AreEqual(false, target.IsExpand);
            target.Switch();
            Assert.AreEqual(true, target.IsExpand);
        }
    }
}