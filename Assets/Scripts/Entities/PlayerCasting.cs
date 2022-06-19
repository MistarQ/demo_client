using UnityEngine;
using GoWorldUnity3D;
using UnityEngine.UI;

public class PlayerCasting : MonoBehaviour
{
    
    private Player player;

    int monsterMask;

    // 咏唱时间
    public float castCD = 2f;

    // 是否在咏唱
    public bool isCasting;

    // 已经咏唱时间
    public float castTime;

    // GCD
    private float GCD = 2.5f;

    // 咏唱UI
    private GameObject castCanvas;

    // 咏唱条
    private Slider castSlider;


    void Awake()
    {
        Debug.Log("player canvas");
        player = GetComponent<Player>();
        monsterMask = LayerMask.GetMask("Monster");
        castCanvas = GameObject.Find("UICanvas").transform.Find("CastCanvas").gameObject;
        castSlider = castCanvas.transform.Find("Slider (4)").GetComponent<Slider>();
        castSlider.value = 0;
    }

    void FixedUpdate()
    {
        if (GetComponent<ClientEntity>().IsClientOwner && GetComponent<ClientEntity>().Attrs.GetInt("hp") > 0)
        {
            if (!player.IsClientOwner)
            {
                return;
            }
            showCast();
            doCast();
            inputCast();
        }
    }

    private void doCast()
    {
        if (isCasting && castTime < castCD)
        {
            castSlider.value = castTime / castCD;
            castTime += Time.fixedDeltaTime;
            //Debug.Log("已经咏唱" + castTime + "秒");
        }
        else if (isCasting && castTime >= castCD)
        {
            //Debug.Log("咏唱结束");
            SendCast();
            castTime = 0;
            isCasting = false;
        }
    }

    private void showCast()
    {
        if (isCasting)
        {
            castCanvas.SetActive(true);
        }
        else
        {
            castCanvas.SetActive(false);
        }
    }

    private void inputCast()
    {
        // 按键响应
        if (Input.GetButton("Fire1") && !isCasting && Time.timeScale != 0)
        {
            isCasting = true;
            castTime = 0;
            Debug.Log("开始咏唱");
        }
        else if (Input.GetButton("Fire1"))
        {
            Debug.Log("正在咏唱");
        }
    }

    public void interruptCast()
    {
        if (!isCasting)
        {
            return;
        }

        isCasting = false;
        Debug.Log("中断施法");
    }


    void SendCast()
    {
        if (!player.IsClientOwner)
        {
            return;
        }

        Debug.Log("尝试发送射击请求");
        ClientEntity entity = player.getTarget();
        if (entity != null)
        {
            Debug.Log("发送射击请求" + entity.ID);
            GetComponent<ClientEntity>().CallServer("Cast", entity.ID);
        }
    }
    
}