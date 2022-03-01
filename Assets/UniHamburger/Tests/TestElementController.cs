using NUnit.Framework;
using UnityEngine;

namespace nkjzm.UniHamburger.Tests
{
    public class TestElementController
    {
        private ElementController target;
        private ElementController targetPrefab;
        private static string FileName => "UniHamburger";

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            targetPrefab = Utils.LoadPrefab<ElementController>(FileName);
        }

        [SetUp]
        public void Setup()
        {
            target = Object.Instantiate(targetPrefab);
        }

        [Test]
        public void 存在する()
        {
            Assert.IsNotNull(target);
        }

        [Test]
        public void Startを呼べる()
        {
            target.Start();
        }

        [Test]
        public void CreateIntItemを呼べる()
        {
            target.CreateIntItem("", 0);
        }

        [Test]
        public void CreateFloatItemを呼べる()
        {
            target.CreateFloatItem("", 0);
        }

        [Test]
        public void CreateBoolItemを呼べる()
        {
            target.CreateBoolItem("", true);
        }

        [Test]
        public void CreateSelectionItemを呼べる()
        {
            target.CreateSelectionItem("", "", () => new[] { "" });
        }

        [Test]
        public void CreateEnumItemを呼べる()
        {
            target.CreateEnumItem("", Test.Item);
        }

        [Test]
        public void リセットを呼べる()
        {
            // 一通り IResettable を呼ぶ
            CreateIntItemを呼べる();
            CreateFloatItemを呼べる();
            CreateBoolItemを呼べる();
            CreateSelectionItemを呼べる();
            CreateEnumItemを呼べる();
            // リセットを呼んでエラーが出ないことを確認
            target.ResetAllElements();
        }

        private enum Test
        {
            Item
        }
    }
}