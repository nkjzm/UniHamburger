using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using nkjzm.UniHamburger.Elements.Base;
using NUnit.Framework;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace nkjzm.UniHamburger.Tests.Elements.Base
{
    public abstract class TestBaseIntSliderElement<T> where T : BaseIntSliderElement
    {
        private T targetPrefab;
        private T target;
        protected abstract string FileName { get; }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            targetPrefab = Utils.LoadPrefab<T>(FileName);
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

        [UnityTest]
        public IEnumerator 初期値が返る()
        {
            return UniTask.ToCoroutine(async () =>
            {
                var token = Utils.CreateTokenToBeCancelled();
                // default以外の値を初期値に指定
                var observable = target.Setup("", 1, salt: DateTime.Now.ToFileTimeUtc().ToString());
                var result = await observable.ToUniTask(true, token);
                Assert.AreEqual(1, result);
            });
        }

        [UnityTest]
        public IEnumerator 値が保存される()
        {
            return UniTask.ToCoroutine(async () =>
            {
                var token = Utils.CreateTokenToBeCancelled();
                var seed = DateTime.Now.ToFileTimeUtc().ToString();
                // default以外の値を初期値に指定
                var observable = target.Setup("", 1, salt: seed);
                var result = await observable.ToUniTask(true, token);
                Assert.AreEqual(1, result);

                // defaultValueよりも保存された値が優先されれる
                var observable2 = target.Setup("", 2, salt: seed);
                var result2 = await observable2.ToUniTask(true, token);
                Assert.AreEqual(1, result2);
            });
        }

        [UnityTest]
        // ReSharper disable once InconsistentNaming
        public IEnumerator 足した結果が返る()
        {
            return UniTask.ToCoroutine(async () =>
            {
                var token = Utils.CreateTokenToBeCancelled();
                // default以外の値を初期値に指定
                var observable = target.Setup("", 1, salt: DateTime.Now.ToFileTimeUtc().ToString());
                var result = await observable.ToUniTask(true, token);
                Assert.AreEqual(1, result);

                // 足す
                target.AddValue();
                var result2 = await observable.ToUniTask(true, token);
                Assert.AreEqual(2, result2);
            });
        }

        [UnityTest]
        // ReSharper disable once InconsistentNaming
        public IEnumerator 引いた結果が返る()
        {
            return UniTask.ToCoroutine(async () =>
            {
                var token = Utils.CreateTokenToBeCancelled();
                // default以外の値を初期値に指定
                var observable = target.Setup("", 1, salt: DateTime.Now.ToFileTimeUtc().ToString());
                var result = await observable.ToUniTask(true, token);
                Assert.AreEqual(1, result);

                // 引く
                target.SubtractValue();
                var result2 = await observable.ToUniTask(true, token);
                Assert.AreEqual(0, result2);
            });
        }

        [UnityTest]
        public IEnumerator 上限を越えない()
        {
            return UniTask.ToCoroutine(async () =>
            {
                var token = Utils.CreateTokenToBeCancelled();
                // default以外の値を初期値に指定
                var observable = target.Setup("", 9, max: 10, salt: DateTime.Now.ToFileTimeUtc().ToString());
                var result = await observable.ToUniTask(true, token);
                Assert.AreEqual(9, result);

                // 足す
                target.AddValue();
                var result2 = await observable.ToUniTask(true, token);
                Assert.AreEqual(10, result2);

                // 足す
                target.AddValue();
                var result3 = await observable.ToUniTask(true, token);
                Assert.AreEqual(10, result3);
            });
        }

        [UnityTest]
        public IEnumerator 下限を下回らない()
        {
            return UniTask.ToCoroutine(async () =>
            {
                var token = Utils.CreateTokenToBeCancelled();
                // default以外の値を初期値に指定
                var observable = target.Setup("", -9, min: -10, salt: DateTime.Now.ToFileTimeUtc().ToString());
                var result = await observable.ToUniTask(true, token);
                Assert.AreEqual(-9, result);

                // 引く
                target.SubtractValue();
                var result2 = await observable.ToUniTask(true, token);
                Assert.AreEqual(-10, result2);

                // 引く
                target.SubtractValue();
                var result3 = await observable.ToUniTask(true, token);
                Assert.AreEqual(-10, result3);
            });
        }

        [UnityTest]
        // ReSharper disable once InconsistentNaming
        public IEnumerator 初期化すると初期値が返る()
        {
            return UniTask.ToCoroutine(async () =>
            {
                var token = Utils.CreateTokenToBeCancelled();
                // default以外の値を初期値に指定
                var observable = target.Setup("", 1, salt: DateTime.Now.ToFileTimeUtc().ToString());
                var result = await observable.ToUniTask(true, token);
                Assert.AreEqual(1, result);

                // 足す
                target.AddValue();
                var result2 = await observable.ToUniTask(true, token);
                Assert.AreEqual(2, result2);

                // リセット
                target.ResetParam();
                var result3 = await observable.ToUniTask(true, token);
                Assert.AreEqual(1, result3);
            });
        }
    }
}