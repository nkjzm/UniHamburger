using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using nkjzm.UniHamburger.Elements.Base;
using NUnit.Framework;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace nkjzm.UniHamburger.Tests.Elements.Base
{
    public abstract class TestBaseEnumDropdownElement<T> where T : BaseEnumDropdownElement
    {
        private T targetPrefab;
        protected T target;
        protected abstract string FileName { get; }
        protected abstract void ChangeElementValue(int value);
        protected abstract int GetSelectedIndex();

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

        private enum TestEnum
        {
            Item1 = 0,
            Item2 = 10,
            Item3 = 20
        }

        [UnityTest]
        public IEnumerator 初期値が返る()
        {
            return UniTask.ToCoroutine(async () =>
            {
                var token = Utils.CreateTokenToBeCancelled();
                // 1番目以外の値を初期値に指定
                var observable = target.Setup("", TestEnum.Item2, salt: DateTime.Now.ToFileTimeUtc().ToString());
                var result = await observable.ToUniTask(true, token);
                Assert.AreEqual(TestEnum.Item2, result);
                Assert.AreEqual(1, GetSelectedIndex());
            });
        }


        [UnityTest]
        public IEnumerator 初期化すると初期値が返る()
        {
            return UniTask.ToCoroutine(async () =>
            {
                var token = Utils.CreateTokenToBeCancelled();
                // 1番目以外の値を初期値に指定
                var observable = target.Setup("", TestEnum.Item2, salt: DateTime.Now.ToFileTimeUtc().ToString());
                var result = await observable.ToUniTask(true, token);
                Assert.AreEqual(TestEnum.Item2, result);
                Assert.AreEqual(1, GetSelectedIndex());

                // 別の値に変更
                ChangeElementValue(0);
                var result2 = await observable.ToUniTask(true, token);
                Assert.AreEqual(TestEnum.Item1, result2);
                Assert.AreEqual(0, GetSelectedIndex());

                // 初期化
                target.ResetParam();
                var result3 = await observable.ToUniTask(true, token);
                Assert.AreEqual(TestEnum.Item2, result3);
                Assert.AreEqual(1, GetSelectedIndex());
            });
        }

        [UnityTest]
        public IEnumerator 値が保存される()
        {
            return UniTask.ToCoroutine(async () =>
            {
                var token = Utils.CreateTokenToBeCancelled();
                var seed = DateTime.Now.ToFileTimeUtc().ToString();
                // 1番目以外の値を初期値に指定
                var observable = target.Setup("", TestEnum.Item2, salt: seed);
                var result = await observable.ToUniTask(true, token);
                Assert.AreEqual(TestEnum.Item2, result);
                Assert.AreEqual(1, GetSelectedIndex());

                // defaultValueよりも保存された値が優先されれる
                var observable2 = target.Setup("", TestEnum.Item3, salt: seed);
                var result2 = await observable2.ToUniTask(true, token);
                Assert.AreEqual(TestEnum.Item2, result2);
                Assert.AreEqual(1, GetSelectedIndex());
            });
        }

        [UnityTest]
        public IEnumerator 操作した結果が返る()
        {
            return UniTask.ToCoroutine(async () =>
            {
                var token = Utils.CreateTokenToBeCancelled();
                // 1番目以外の値を初期値に指定
                var observable = target.Setup("", TestEnum.Item2, salt: DateTime.Now.ToFileTimeUtc().ToString());
                var result = await observable.ToUniTask(true, token);
                Assert.AreEqual(TestEnum.Item2, result);
                Assert.AreEqual(1, GetSelectedIndex());

                // 別の値に変更
                ChangeElementValue(2);
                var result2 = await observable.ToUniTask(true, token);
                Assert.AreEqual(TestEnum.Item3, result2);
                Assert.AreEqual(2, GetSelectedIndex());
            });
        }

        [UnityTest]
        public IEnumerator 存在しない値を指定したら最初の要素を返す()
        {
            return UniTask.ToCoroutine(async () =>
            {
                var token = Utils.CreateTokenToBeCancelled();
                var observable = target.Setup("", TestEnum.Item3 + 100, salt: DateTime.Now.ToFileTimeUtc().ToString());
                var result = await observable.ToUniTask(true, token);
                Assert.AreEqual(TestEnum.Item1, result);
                Assert.AreEqual(0, GetSelectedIndex());
            });
        }

        [UnityTest]
        public IEnumerator 存在しない負の値を指定したら最初の要素を返す()
        {
            return UniTask.ToCoroutine(async () =>
            {
                var token = Utils.CreateTokenToBeCancelled();
                var observable = target.Setup("", TestEnum.Item1 - 100, salt: DateTime.Now.ToFileTimeUtc().ToString());
                var result = await observable.ToUniTask(true, token);
                Assert.AreEqual(TestEnum.Item1, result);
                Assert.AreEqual(0, GetSelectedIndex());
            });
        }
    }
}