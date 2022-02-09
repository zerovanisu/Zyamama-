using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public string Skill_Name;

    public int Skill_Number;

    [Header("�G�t�F�N�g������")]
    [SerializeField]
    ParticleSystem[] Effect;

    [Header("�X�L���̐F�����߂�")]
    [SerializeField]
    Color Red, Blue, Yellow;

    // Start is called before the first frame update
    void Start()
    {
        switch (Skill_Number)
        {
            case 0:
                Skill_Name = null;
                break;

            case 1:
                Skill_Name = "Avatar";
                CollerChange(Blue);
                break;

            case 2:
                Skill_Name = "MultipleBall";
                CollerChange(Yellow);
                break;

            case 3:
                Skill_Name = "TimeFast";
                CollerChange(Red);
                break;
        }
    }

    void CollerChange(Color effect)
    {
        for(int i = 0; i < Effect.Length; i++)
        {
            Effect[i].startColor = effect;
        }
    }
}
