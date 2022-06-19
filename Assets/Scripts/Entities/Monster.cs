using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GoWorldUnity3D;

public class Monster : GameEntity
{
    Animator anim;
    LineRenderer lineRenderer;
    float attackTime;
    private Int64 radius;

    private MonsterCasting monsterCasting;

    protected override void OnCreated()
    {
        anim = gameObject.GetComponent<Animator>();
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        monsterCasting = gameObject.GetComponent<MonsterCasting>();
        radius = this.Attrs.GetInt("radius");
        GameObject ring = transform.Find("Ring").gameObject;
        ring.transform.localScale = new Vector3(radius, radius, radius);
        // Debug.Log ("Monster is created at" + gameObject.transform.position);
    }

    protected override void OnDestroy()
    {
        Debug.Log("Monster is destroyed");
    }

    protected override void OnEnterSpace()
    {
        string action = this.Attrs.GetStr("action");
        anim.SetTrigger(action);
    }

    public void OnAttrChange_action()
    {
        string action = this.Attrs.GetStr("action");
        // Debug.Log (this.ToString() + "'s action is changed to " + action); 

        anim.SetTrigger(action);
    }

    public void DisplayAttack(string playerID)
    {
        ClientEntity player = GoWorld.GetEntity(playerID);
        //Debug.LogWarning (this.ToString () + " attack " + playerID + " " + player.ToString());
        Vector3 startPos = this.gameObject.transform.position;
        Vector3 endPos = player.gameObject.transform.position;
        startPos.y = 0.5f;
        endPos.y = 0.5f;
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
        lineRenderer.enabled = true;
        attackTime = Time.time;
        anim.SetTrigger("attack");
    }

    public void DisplayCastBar(float castCD,long skillType, string castName, string monsterID)
    {
        monsterCasting.castTime = 0;
        monsterCasting.castCD = castCD;
        monsterCasting.castName.text = castName;
        monsterCasting.isCasting = true;
        monsterCasting.castCanvas.SetActive(true);
        if (skillType == 2 || skillType == 3)
        {
            this.DisplayCast(monsterID);
        } else if (skillType == 4)
        {
            
        }
    }

    public void DisplayAoe(string monsterID)
    {
        Debug.Log("monster aoe");
    }

    public void DisplayCast(string monsterID)
    {
        ClientEntity monster = GoWorld.GetEntity(monsterID);
        Debug.Log("monster cast");
        GameObject effect = Instantiate((GameObject) Resources.Load("Prefab/Effect/Iron"));
        Vector3 vec = new Vector3(monster.Position.x,
            0.1f,
            monster.Position.z);
        effect.transform.position = vec;
        Debug.Log("explosion center:" + vec);
        Destroy(effect, 3);
    }

    public void DisplayAttacked(string monsterID, bool isCrit)
    {
        ClientEntity monster = GoWorld.GetEntity(monsterID);
        GameObject effect = Instantiate((GameObject) Resources.Load("Prefab/Effect/ExBlue"));
        Vector3 vec = new Vector3(monster.Position.x,
            monster.Position.y + 1,
            monster.Position.z);
        effect.transform.position = vec;
        effect.name = "effect";
        Destroy(effect, 3);
    }

    void Update()
    {
        if (Time.time >= attackTime + 0.2f)
        {
            lineRenderer.enabled = false;
        }
    }

    protected override void OnBecomeClientOwner()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnLeaveSpace()
    {
    }

    public static new GameObject CreateGameObject(MapAttr attrs)
    {
        // return GameObject.Instantiate(GameObject.Find("GoWorldController").GetComponent<GoWorldController>().ZomBunny);
        return GameObject.Instantiate(GameObject.Find("GoWorldController").GetComponent<GoWorldController>().Knight);
    }
}