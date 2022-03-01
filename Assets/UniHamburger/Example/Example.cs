using UniRx;
using UnityEngine;

namespace nkjzm.UniHamburger.Example
{
    /// <summary>
    /// サンプル用クラス
    /// </summary>
    public class Example : MonoBehaviour
    {
        [SerializeField] private ElementController elementController;

        // Salt で保存字のキーを区別しています。シーンやモード毎に別々の設定をしたい場合に設定してください。
        private const string Salt = "sample";

        private enum Fruit
        {
            Apple,
            Orange,
            Grape,
        }

        private void Start()
        {
            // 例：整数スライダー
            elementController
                .CreateIntItem("スライダー(int)", 1, min: -10, max: 10, salt: Salt)
                .Subscribe(value => Debug.Log($"スライダー(int): {value}"))
                .AddTo(this);

            // 例：少数スライダー
            elementController
                .CreateFloatItem("スライダー(float)", 2, min: -0.2f, max: 3f, salt: Salt)
                .Subscribe(value => Debug.Log($"スライダー(float): {value}"))
                .AddTo(this);

            var activeFruit = new Subject<bool>();

            // 例：トグル（チェックボックス）
            elementController
                .CreateBoolItem("フルーツをロック", false, salt: Salt)
                .Subscribe(locked =>
                {
                    activeFruit.OnNext(!locked);
                    Debug.Log($"フルーツをロック: {locked}");
                })
                .AddTo(this);

            // 例：列挙型版ドロップダウン
            elementController.CreateEnumItem(
                    "フルーツ", Fruit.Apple, salt: Salt, activeUpdated: activeFruit)
                .Subscribe(value => Debug.Log($"フルーツ: {value}"))
                .AddTo(this);

            // 例：配列版ドロップダウン
            var types = new[] { "草タイプ", "水タイプ", "炎タイプ" };
            elementController
                .CreateSelectionItem("タイプ", types[1], () => types, salt: Salt)
                .Subscribe(value => Debug.Log($"個数: {value}"))
                .AddTo(this);
        }
    }
}