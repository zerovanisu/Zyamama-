using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleSceneManager : MonoBehaviour
{
    //  一列に用意されるシリンダーオブジェクト
    [Serializable]
    private class LinePack
    {
        public List<GameObject> PackCylinder = new List<GameObject>();
    }
    //  シリンダー列リスト
    [SerializeField]
    private List<LinePack> _linePack = new List<LinePack>();

    //[SerializeField]
    //private Button _button;

    private void Start()
    {
        //  ボタン対応
        RandomObjectSet();
    }

    /// <summary>
    /// ボタンを押したらランダムにセットする
    /// </summary>
    private void RandomObjectSet()
    {
        //  各列に対して１つだけアイテムを設定
        _linePack.ForEach(packLines => {
            //  一旦中身をクリアする
            packLines.PackCylinder.ForEach(pack => pack.GetComponent<CylinderUseCase>().ClearItem());
            //  ０～５までの乱数取得
            var rndIndex = UnityEngine.Random.Range(0, packLines.PackCylinder.Count);
            //  対象のシリンダーにオブジェクトを設定
            packLines.PackCylinder[rndIndex].GetComponent<CylinderUseCase>().SetRandomItem();
        });
    }

}