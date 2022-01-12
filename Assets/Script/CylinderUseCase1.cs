using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderUseCase : MonoBehaviour
{
    //  内部オブジェクトリスト
    [SerializeField]
    private List<GameObject> _itemObject = new List<GameObject>();

    //  オブジェクトが存在するかどうかのチェック
    public bool IsSetting { get; private set; } = false;
    //  選択したオブジェクト番号を返す
    public int SelectIndex { get; private set; } = -1;

    /// <summary>
    /// ランダムにアイテムを設定する
    /// </summary>
    public void SetRandomItem()
    {
        //  内部で予定しているオブジェクトの数から乱数を取得
        SelectIndex = Random.Range(0, _itemObject.Count);
        //  対象のオブジェクトを表示する
        _itemObject[SelectIndex].SetActive(true);
        //  表示されているフラグを設定
        IsSetting = true;
    }

    /// <summary>
    /// アイテムを消す（表示）
    /// </summary>
    public void ClearItem()
    {
        _itemObject.ForEach(item => item.SetActive(false));
    }

    private void OnCollisionEnter(Collision hit)
    {
        if(hit.gameObject.tag == "ball")
        {
            Destroy(this.gameObject);
        }
    }
}
