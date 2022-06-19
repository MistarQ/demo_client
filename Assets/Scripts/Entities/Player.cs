using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GoWorldUnity3D;
using System.Threading;

public class Player : GameEntity
{
    Animator anim;
    private Ray cameraRay = new Ray();
    private RaycastHit rayHit;
    int lockableMask;
    private GameEntity target;

    private PlayerCasting playerCasting;
    private PlayerMovement playerMovement;

    protected override void OnCreated()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);

        anim = gameObject.GetComponent<Animator>();
        playerCasting = gameObject.GetComponent<PlayerCasting>();
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        lockableMask = LayerMask.GetMask("Monster", "Player");

        Debug.Log("Player is created");
    }

    protected override void OnDestroy()
    {
        Debug.Log("Player is destroyed");
    }

    protected override void OnEnterSpace()
    {
        if (this.IsClientOwner)
        {
            SceneManager.LoadScene("Level 01", LoadSceneMode.Single);
            // UnityEngine.SceneManagement.SceneManager.LoadScene("Level 01", LoadSceneMode.Single);
        }

        gameObject.GetComponent<PlayerMovement>().enabled = this.IsClientOwner;

        string action = this.Attrs.GetStr("action");
        anim.SetTrigger(action);
        
        Debug.Log(Attrs.get("name"));
    }

    public void OnAttrChange_action()
    {
        string action = this.Attrs.GetStr("action");
        // Debug.Log (this.ToString() + "'s action is changed to " + action); 
        anim.SetTrigger(action);
    }

    public ClientEntity getTarget()
    {
        return target;
    }

    private void setTarget()
    {
        try
        {
            Debug.Log("尝试锁定目标");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out rayHit, 100, lockableMask))
            {
                if (target != null)
                {
                    GameObject oldTarget = target.transform.Find("Ring").gameObject;
                    oldTarget.SetActive(false);
                }

                GameEntity entity = rayHit.collider.GetComponent<GameEntity>();
                GameObject targetRing = entity.transform.Find("Ring").gameObject;
                targetRing.SetActive(true);
                target = entity;
                Debug.Log("目标锁定" + target);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    

    protected override void OnBecomeClientOwner()
    {
    }

    protected override void OnLeaveSpace()
    {
        throw new System.NotImplementedException();
    }

    void Update()
    {
        // GoWorldUnity3D.Logger.Debug("Player", "Player Update. ..");

        if (this.IsClientOwner)
        {
            string currentScene = SceneManager.GetActiveScene().name;
            if (currentScene == "Level 01")
            {
                GameObject camera = GameObject.Find("Main Camera");
                camera.GetComponent<CameraFollow>().target = gameObject.transform;
            }

            if (playerMovement.isWaling())
            {
                playerCasting.interruptCast();
            }

            if (Input.GetMouseButtonDown(0))
            {
                setTarget();
            }
        }
    }

    public static new GameObject CreateGameObject(MapAttr attrs)
    {
        return GameObject.Instantiate(GameObject.Find("GoWorldController").GetComponent<GoWorldController>().Player);
    }
    
    public void DisplayAttacked(string playerID)
    {
        ClientEntity player = GoWorld.GetEntity(playerID);
        GameObject effect = Instantiate((GameObject) Resources.Load("Prefab/Effect/ExBlue"));
        Vector3 vec = new Vector3(player.Position.x,
            player.Position.y + 1,
            player.Position.z);
        effect.transform.position = vec;
        Debug.Log("attacked center:" + vec);
        Destroy(effect, 3);
    }

    public void ResetCoord(float X, float Y, float Z)
    {
        this.transform.position = new Vector3(X, Y, Z);
    }
}