using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using nkjzm.UniHamburger.Elements.Base;
using NUnit.Framework;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.TestTools;

namespace nkjzm.UniHamburger.Tests.Elements.Base
{
    public abstract class TestBaseFloatSliderElement<T> where T : BaseFloatSliderElement
    {
        private T targetPrefab;
        private T target;
        protected abstract string FileName { get; }

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
            var observable = target.Setup("", 1f, salt: DateTime.Now.ToFileTimeUtc().ToString());
            var result = await observable.ToUniTask(true, token);
            Assert.AreEqual(1f, result);
        });

        [UnityTest]
        public IEnumerator 値が保存される() => UniTask.ToCoroutine(async () =>
        {
            var token = Utils.CreateTokenToBeCancelled();
            var seed = DateTime.Now.ToFileTimeUtc().ToString();
            var unit = 0.1f;
            // default以外の値を初期値に指定
            var observable = target.Setup("", 1f, unit, salt: seed);
            var result = await observable.ToUniTask(true, token);
            Assert.AreEqual(1f, result, unit);

            // defaultValueよりも保存された値が優先されれる
            var observable2 = target.Setup("", 2, unit, salt: seed);
            var result2 = await observable2.ToUniTask(true, token);
            Assert.AreEqual(1f, result2, unit);
        });

        [UnityTest]
        public IEnumerator 足した結果が返る() => UniTask.ToCoroutine(async () =>
        {
            var token = Utils.CreateTokenToBeCancelled();
            var unit = 0.1f;
            // default以外の値を初期値に指定
            var observable = target.Setup("", 1f, unit, salt: DateTime.Now.ToFileTimeUtc().ToString());
            var result = await observable.ToUniTask(true, token);
            Assert.AreEqual(1f, result, unit);

            // 足す
            target.AddValue();
            var result2 = await observable.ToUniTask(true, token);
            Assert.AreEqual(1.1f, result2, unit);
        });

        [UnityTest]
        // ReSharper disable once InconsistentNaming
        public IEnumerator 引いた結果が返る() => UniTask.ToCoroutine(async () =>
        {
            var token = Utils.CreateTokenToBeCancelled();
            var unit = 0.1f;
            // default以外の値を初期値に指定
            var observable = target.Setup("", 1f, unit, salt: DateTime.Now.ToFileTimeUtc().ToString());
            var result = await observable.ToUniTask(true, token);
            Assert.AreEqual(1f, result, unit);

            // 引く
            target.SubtractValue();
            var result2 = await observable.ToUniTask(true, token);
            Assert.AreEqual(0.9f, result2, unit);
        });

        [UnityTest]
        public IEnumerator 上限を越えない() => UniTask.ToCoroutine(async () =>
        {
            var token = Utils.CreateTokenToBeCancelled();
            var unit = 0.1f;
            // default以外の値を初期値に指定
            var observable = target.Setup("", 1f, unit, max: 1.1f, salt: DateTime.Now.ToFileTimeUtc().ToString());
            var result = await observable.ToUniTask(true, token);
            Assert.AreEqual(1f, result, unit);

            // 足す
            target.AddValue();
            var result2 = await observable.ToUniTask(true, token);
            Assert.AreEqual(1.1f, result2, unit);

            // 足す
            target.AddValue();
            var result3 = await observable.ToUniTask(true, token);
            Assert.AreEqual(1.1f, result3, unit);
        });

        [UnityTest]
        public IEnumerator 下限を下回らない() => UniTask.ToCoroutine(async () =>
        {
            var token = Utils.CreateTokenToBeCancelled();
            var unit = 0.1f;
            // default以外の値を初期値に指定
            var observable = target.Setup("", 1f, unit, min: 0.9f, salt: DateTime.Now.ToFileTimeUtc().ToString());
            var result = await observable.ToUniTask(true, token);
            Assert.AreEqual(1f, result, unit);

            // 引く
            target.SubtractValue();
            var result2 = await observable.ToUniTask(true, token);
            Assert.AreEqual(0.9f, result2, unit);

            // 引く
            target.SubtractValue();
            var result3 = await observable.ToUniTask(true, token);
            Assert.AreEqual(0.9f, result3, unit);
        });

        [UnityTest]
        // ReSharper disable once InconsistentNaming
        public IEnumerator 初期化すると初期値が返る() => UniTask.ToCoroutine(async () =>
        {
            var token = Utils.CreateTokenToBeCancelled();
            var unit = 0.1f;
            // default以外の値を初期値に指定
            var observable = target.Setup("", 1f, unit, salt: DateTime.Now.ToFileTimeUtc().ToString());
            var result = await observable.ToUniTask(true, token);
            Assert.AreEqual(1f, result, unit);

            // 引く
            target.SubtractValue();
            var result2 = await observable.ToUniTask(true, token);
            Assert.AreEqual(0.9f, result2, unit);

            target.ResetParam();
            var result3 = await observable.ToUniTask(true, token);
            Assert.AreEqual(1f, result3, unit);
        });

        [Test]
        public void 普通はたくさん足すと誤差が出る()
        {
            var value = 100f;
            var unit = 0.1f;

            // たくさん足す
            for (int i = 0; i < 10000; i++)
            {
                value += unit;
            }

            // 足した単位以内の誤差に収まらない
            Assert.IsFalse(Mathf.Abs(1100f - value) < unit);
        }

        [UnityTest]
        public IEnumerator たくさん足しても誤差が出ない() => UniTask.ToCoroutine(async () =>
        {
            var token = Utils.CreateTokenToBeCancelled();
            var unit = 0.1f;
            // default以外の値を初期値に指定
            var observable = target.Setup("", 100f, unit, salt: DateTime.Now.ToFileTimeUtc().ToString());
            var result = await observable.ToUniTask(true, token);
            Assert.AreEqual(100f, result, unit);

            // たくさん足す
            for (int i = 0; i < 10000; i++)
            {
                target.AddValue();
            }

            var result2 = await observable.ToUniTask(true, token);
            Assert.AreEqual(1100f, result2, unit);
        });
    }
}