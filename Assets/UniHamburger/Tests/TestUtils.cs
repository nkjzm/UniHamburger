using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace nkjzm.UniHamburger.Tests
{
    public class TestUtils
    {
        private const string DummyFileName = "Dummy";
        private const string DummyFileName2 = "Dummy2";
        private const string AssetsRoot = "Assets";
        private static string DummyFilePath(string fileName) => $"{AssetsRoot}/{fileName}.prefab";
        private static string DummyFolderPath(string fileName) => $"{AssetsRoot}/{fileName}";


        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // ダミーファイル
            var gameObject = new GameObject();
            PrefabUtility.SaveAsPrefabAsset(gameObject, DummyFilePath(DummyFileName));
            PrefabUtility.SaveAsPrefabAsset(gameObject, DummyFilePath(DummyFileName2));
            // ダミーファイルと同名のダミーフォルダー
            AssetDatabase.CreateFolder(AssetsRoot, DummyFileName);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            AssetDatabase.DeleteAsset(DummyFilePath(DummyFileName));
            AssetDatabase.DeleteAsset(DummyFilePath(DummyFileName2));
            AssetDatabase.DeleteAsset(DummyFolderPath(DummyFileName));
        }

        [Test]
        public void 対象ファイルを読み込める1()
        {
            var target = Utils.LoadPrefab<GameObject>(DummyFileName);
            Assert.IsNotNull(target);
            Assert.AreEqual(DummyFileName, target.name);
        }

        [Test]
        public void 対象ファイルを読み込める2()
        {
            var target = Utils.LoadPrefab<GameObject>(DummyFileName2);
            Assert.IsNotNull(target);
            Assert.AreEqual(DummyFileName2, target.name);
        }

        [Test]
        public void 継承元の型指定でファイルを読み込める()
        {
            var target = Utils.LoadPrefab<Object>(DummyFileName);
            Assert.IsNotNull(target);
        }

        [Test]
        public void 誤った型名を指定するとNullが返る()
        {
            var target = Utils.LoadPrefab<Material>(DummyFileName);
            Assert.IsNull(target);
        }

        [Test]
        public void パスにnullを渡すとNullが返る()
        {
            var target = Utils.LoadPrefab<GameObject>(null);
            Assert.IsNull(target);
        }

        [Test]
        public void パスに空文字を渡すとNullが返る()
        {
            var target = Utils.LoadPrefab<GameObject>(string.Empty);
            Assert.IsNull(target);
        }

        [UnityTest]
        public IEnumerator キャンセルが発火する() => UniTask.ToCoroutine(async () =>
        {
            var token = Utils.CreateTokenToBeCancelled(0);

            bool isSuccess = false;

            try
            {
                await UniTask.DelayFrame(10, cancellationToken: token);
                Assert.Fail();
            }
            catch (OperationCanceledException)
            {
                isSuccess = true;
            }

            Assert.AreEqual(true, isSuccess);
        });
    }
}