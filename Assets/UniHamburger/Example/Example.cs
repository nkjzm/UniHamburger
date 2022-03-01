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
        private const string SampleSalt = "sample";

        private enum Fruit
        {
            Apple,
            Orange,
            Grape
        }

        private void Start()
        {
            elementController.CreateIntItem("スライダー(int)", 1, min: -10, max: 10, salt: SampleSalt)
                .Subscribe(value => Debug.Log($"スライダー(int): {value}"))
                .AddTo(this);

            elementController.CreateFloatItem("スライダー(float)", 2, min: -0.2f, max: 3.3f, salt: SampleSalt)
                .Subscribe(value => Debug.Log($"スライダー(float): {value}"))
                .AddTo(this);

            var onActiveFruit = new Subject<bool>();

            elementController.CreateBoolItem("フルーツをロック", false, salt: SampleSalt)
                .Subscribe(locked =>
                {
                    onActiveFruit.OnNext(!locked);
                    Debug.Log($"フルーツをロック: {locked}");
                })
                .AddTo(this);

            elementController
                .CreateEnumItem("好きなフルーツ", Fruit.Apple, salt: SampleSalt, activeUpdated: onActiveFruit)
                .Subscribe(value => Debug.Log($"好きなフルーツ: {value}"))
                .AddTo(this);

            elementController
                .CreateSelectionItem("タイプ", "水タイプ", () => new[] { "草タイプ", "水タイプ", "炎タイプ" }, salt: SampleSalt)
                .Subscribe(value => Debug.Log($"個数: {value}"))
                .AddTo(this);
        }
    }
}