using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using nkjzm.UniHamburger.Elements.Base;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace nkjzm.UniHamburger.Tests.Elements.Base
{
    public abstract class TestBaseToggleElement<T> where T : BaseToggleElement
    {
        private T targetPrefab;
        protected T target;
        protected abstract string FileName { get; }
        protected abstract void Switch();

        [OneTimeSetUp]
        public void OneTimeSetup() => targetPrefab = Utils.LoadPrefab<T>(FileName);

        [SetUp]
        public void Setup() => target = UnityEngine.Object.Instantiate(targetPrefab);

        [Test]
        public void 存在する()
        {
            Assert.IsNotNull(target);
        }

        [UnityTest]
        public IEnumerator 初期値が返る() => UniTask.ToCoroutine(async () =>
        {
            var token = Utils.CreateTokenToBeCancelled();
            // default以外の値を初期値に指定
            var observable = target.Setup("", true, salt: DateTime.Now.ToFileTimeUtc().ToString());
            var result = await observable.ToUniTask(true, token);
            Assert.AreEqual(true, result);
        });

        [UnityTest]
        public IEnumerator 値が保存される() => UniTask.ToCoroutine(async () =>
        {
            var token = Utils.CreateTokenToBeCancelled();
            var seed = DateTime.Now.ToFileTimeUtc().ToString();
            // default以外の値を初期値に指定
            var observable = target.Setup("", true, salt: seed);
            var result = await observable.ToUniTask(true, token);
            Assert.AreEqual(true, result);

            // defaultValueよりも保存された値が優先されれる
            var observable2 = target.Setup("", false, salt: seed);
            var result2 = await observable2.ToUniTask(true, token);
            Assert.AreEqual(true, result2);
        });

        [UnityTest]
        // ReSharper disable once InconsistentNaming
        public IEnumerator uGUIを操作した結果が返る() => UniTask.ToCoroutine(async () =>
        {
            var token = Utils.CreateTokenToBeCancelled();
            // default以外の値を初期値に指定
            var observable = target.Setup("", true, salt: DateTime.Now.ToFileTimeUtc().ToString());
            var result = await observable.ToUniTask(true, token);
            Assert.AreEqual(true, result);

            // falseに変更
            Switch();
            var result2 = await observable.ToUniTask(true, token);
            Assert.AreEqual(false, result2);
        });

        [UnityTest]
        // ReSharper disable once InconsistentNaming
        public IEnumerator 初期化すると初期値が返る() => UniTask.ToCoroutine(async () =>
        {
            var token = Utils.CreateTokenToBeCancelled();
            // default以外の値を初期値に指定
            var observable = target.Setup("", true, salt: DateTime.Now.ToFileTimeUtc().ToString());
            var result = await observable.ToUniTask(true, token);
            Assert.AreEqual(true, result);

            // falseに変更
            Switch();
            var result2 = await observable.ToUniTask(true, token);
            Assert.AreEqual(false, result2);

            // リセット
            target.ResetParam();
            var result3 = await observable.ToUniTask(true, token);
            Assert.AreEqual(true, result3);
        });
    }
}