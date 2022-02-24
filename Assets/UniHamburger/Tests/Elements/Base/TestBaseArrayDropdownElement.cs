using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using nkjzm.UniHamburger.Elements.Base;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace nkjzm.UniHamburger.Tests.Elements.Base
{
    public abstract class TestBaseArrayDropdownElement<T> where T : BaseArrayDropdownElement
    {
        private T targetPrefab;
        protected T target;
        protected abstract string FileName { get; }
        protected abstract void ChangeElementValue(int value);
        protected abstract int GetElementCount();
        protected abstract int GetSelectedIndex();

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
            // 1番目以外の値を初期値に指定
            var observable = target.Setup("", "b", () => new[] { "a", "b" },
                salt: DateTime.Now.ToFileTimeUtc().ToString());
            var result = await observable.ToUniTask(true, token);
            Assert.AreEqual("b", result);
            Assert.AreEqual(1, GetSelectedIndex());
        });


        [UnityTest]
        public IEnumerator 初期化すると初期値が返る() => UniTask.ToCoroutine(async () =>
        {
            var token = Utils.CreateTokenToBeCancelled();
            // 1番目以外の値を初期値に指定
            var observable = target.Setup("", "b", () => new[] { "a", "b" },
                salt: DateTime.Now.ToFileTimeUtc().ToString());
            var result = await observable.ToUniTask(true, token);
            Assert.AreEqual("b", result);
            Assert.AreEqual(1, GetSelectedIndex());

            // 1番目の値（a）に変更
            ChangeElementValue(0);
            var result2 = await observable.ToUniTask(true, token);
            Assert.AreEqual("a", result2);
            Assert.AreEqual(0, GetSelectedIndex());

            // 初期化
            target.ResetParam();
            var result3 = await observable.ToUniTask(true, token);
            Assert.AreEqual("b", result3);
            Assert.AreEqual(1, GetSelectedIndex());
        });

        [UnityTest]
        public IEnumerator 初期値が存在しない時は1番目の値が返る() => UniTask.ToCoroutine(async () =>
        {
            var token = Utils.CreateTokenToBeCancelled();
            var observable = target.Setup("", "c", () => new[] { "a", "b" },
                salt: DateTime.Now.ToFileTimeUtc().ToString());
            var result = await observable.ToUniTask(true, token);
            Assert.AreEqual("a", result);
            Assert.AreEqual(0, GetSelectedIndex());
        });

        [UnityTest]
        public IEnumerator 値が保存される() => UniTask.ToCoroutine(async () =>
        {
            var token = Utils.CreateTokenToBeCancelled();
            var seed = DateTime.Now.ToFileTimeUtc().ToString();
            // 1番目以外の値を初期値に指定
            var observable = target.Setup("", "b", () => new[] { "a", "b" }, salt: seed);
            var result = await observable.ToUniTask(true, token);
            Assert.AreEqual("b", result);
            Assert.AreEqual(1, GetSelectedIndex());

            // defaultValueよりも保存された値が優先されれる
            var observable2 = target.Setup("", "a", () => new[] { "a", "b" }, salt: seed);
            var result2 = await observable2.ToUniTask(true, token);
            Assert.AreEqual("b", result2);
            Assert.AreEqual(1, GetSelectedIndex());
        });

        [UnityTest]
        // ReSharper disable once InconsistentNaming
        public IEnumerator uGUIを操作した結果が返る() => UniTask.ToCoroutine(async () =>
        {
            var token = Utils.CreateTokenToBeCancelled();
            // 1番目以外の値を初期値に指定
            var observable = target.Setup("", "b", () => new[] { "a", "b" },
                salt: DateTime.Now.ToFileTimeUtc().ToString());
            var result = await observable.ToUniTask(true, token);
            Assert.AreEqual("b", result);
            Assert.AreEqual(1, GetSelectedIndex());

            // 1番目の値（a）に変更
            ChangeElementValue(0);
            var result2 = await observable.ToUniTask(true, token);
            Assert.AreEqual("a", result2);
            Assert.AreEqual(0, GetSelectedIndex());
        });

        [Test]
        public void 同じ要素を含んでいたらエラー()
        {
            Assert.Throws<InvalidOperationException>(() =>
                target.Setup("", "b", () => new[] { "a", "a" }, salt: DateTime.Now.ToFileTimeUtc().ToString()));
        }

        [Test]
        public void 要素が1000個ある()
        {
            // 1番目以外の値を初期値に指定
            // ReSharper disable once AccessToModifiedClosure

            var list = new List<string>();
            for (int i = 0; i < 1000; i++)
            {
                list.Add(i.ToString());
            }

            target.Setup("", "b", () => list.ToArray(), salt: DateTime.Now.ToFileTimeUtc().ToString());

            // 要素が1000個
            var result = GetElementCount();
            Assert.AreEqual(1000, result);
        }

        [Test]
        public void 値の更新が可能()
        {
            var index = 0;
            var selectionList = new[]
            {
                new[] { "a", "b" },
                new[] { "a", "b", "c" }
            };

            // 1番目以外の値を初期値に指定
            // ReSharper disable once AccessToModifiedClosure
            target.Setup("", "b", () => selectionList[index], salt: DateTime.Now.ToFileTimeUtc().ToString());

            // 要素が2つ
            var result = GetElementCount();
            Assert.AreEqual(2, result);

            // 値を更新
            index = 1;
            target.InitDropdown();

            // 要素が3つ
            var result2 = GetElementCount();
            Assert.AreEqual(3, result2);
        }


        [UnityTest]
        // ReSharper disable once InconsistentNaming
        public IEnumerator 値の更新後に同じ値を選択状態になっている() => UniTask.ToCoroutine(async () =>
        {
            var token = Utils.CreateTokenToBeCancelled();

            var index = 0;
            var selectionList = new[]
            {
                new[] { "a", "b" },
                new[] { "b", "c", "a" }
            };

            // 1番目の値を初期値に指定
            // ReSharper disable once AccessToModifiedClosure
            var observable = target.Setup("", "a", () => selectionList[index],
                salt: DateTime.Now.ToFileTimeUtc().ToString());
            var result = await observable.ToUniTask(true, token);
            Assert.AreEqual("a", result);
            Assert.AreEqual(0, GetSelectedIndex());

            // 値を更新
            index = 1;
            target.InitDropdown();

            // 要素が変更されている
            var result2 = GetElementCount();
            Assert.AreEqual(3, result2);

            // 同じ値を選択している
            var result3 = await observable.ToUniTask(true, token);
            Assert.AreEqual("a", result3);
            Assert.AreEqual(2, GetSelectedIndex());
        });

        [UnityTest]
        public IEnumerator 値の更新後に選択していた値がなくなったら先頭を選択() => UniTask.ToCoroutine(async () =>
        {
            var token = Utils.CreateTokenToBeCancelled();

            var index = 0;
            var selectionList = new[]
            {
                new[] { "a", "x" },
                new[] { "b", "c", "a" }
            };

            // xを初期値に選択
            // ReSharper disable once AccessToModifiedClosure
            var observable = target.Setup("", "x", () => selectionList[index],
                salt: DateTime.Now.ToFileTimeUtc().ToString());

            // xを選択している
            var result = await observable.ToUniTask(true, token);
            Assert.AreEqual("x", result);
            Assert.AreEqual(1, GetSelectedIndex());

            // 値を更新
            index = 1;
            target.InitDropdown();

            // 要素が変更されている
            var result2 = GetElementCount();
            Assert.AreEqual(3, result2);

            // 先頭の値を選択している
            var result3 = await observable.ToUniTask(true, token);
            Assert.AreEqual("b", result3);
            Assert.AreEqual(0, GetSelectedIndex());
        });
    }
}