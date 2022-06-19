using System;
using UnityEngine;
using GoWorldUnity3D;
using UnityEngine.UI;

public class MonsterCasting : MonoBehaviour
{
    private Monster monster;

    // 咏唱时间
    public float castCD;

    // 是否在咏唱
    public bool isCasting;

    // 已经咏唱时间
    public float castTime;

    // 技能名
    public Text castName;
    
    // 咏唱UI
    public GameObject castCanvas;

    // 咏唱条
    private Slider castSlider;
    
    
    private void Awake()
    {
        monster = GetComponent<Monster>();
        castCanvas = GameObject.Find("UICanvas").transform.Find("MonsterCanvas").gameObject;
        castName = castCanvas.transform.Find("CastName").gameObject.GetComponent<Text>();
        castSlider = castCanvas.transform.Find("CastSlider").GetComponent<Slider>();
        castSlider.value = 0f;
        castCanvas.SetActive(false);
        Debug.Log("monster canvas");
    }

    private void FixedUpdate()
    {
        if (isCasting)
        {
            castTime += Time.deltaTime;
            doCast();
            if (castTime >= castCD)
            {
                castCD = 0;
                castTime = 0;
                isCasting = false;
                castCanvas.SetActive(false);
            }
        }
    }

    private void doCast()
    {
        castSlider.value = castTime / castCD;
    }
}