using UnityEngine;
using GoWorldUnity3D;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;

    Vector3 movement;

    //Animator anim;
    Rigidbody playerRigidbody;
    int floorMask;
    float camRayLength = 100f;
    bool isWalking;


    void Start()
    {
        floorMask = LayerMask.GetMask("Floor");
        //anim = GetComponent<Animator> ();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (GetComponent<ClientEntity>().Attrs.GetInt("hp") <= 0)
        {
            return;
        }

        Turning();
        Move(h, v);
        ChangeAction(h, v);
    }

    void Move(float h, float v)
    {
        if (h == 0 && v == 0)
        {
            return;
        }

        Vector3 input = new Vector3(h, 0, v);
        Quaternion q = Quaternion.LookRotation(input);
        Vector3 vec = transform.forward;
        vec = q * vec;
        
        movement.Set(vec.x, vec.y, vec.z);
        movement = movement.normalized * speed * Time.fixedDeltaTime;
        playerRigidbody.MovePosition(transform.position + movement);
    }


    void Turning()
    {
        Vector3 cameraPos = Camera.main.GetComponent<Transform>().position;
        Vector3 cameraToPlayer = transform.position - cameraPos;
        cameraToPlayer.y = 0f;
        Quaternion newRotation = Quaternion.LookRotation(cameraToPlayer);
        playerRigidbody.MoveRotation(newRotation);
    }


    public bool isWaling()
    {
        return isWalking;
    }

    void ChangeAction(float h, float v)
    {
        bool walking = h != 0f || v != 0f;
        if (!this.isWalking && walking)
        {
            this.isWalking = walking;
            GetComponent<ClientEntity>().CallServer("SetAction", "move");
        }
        else if (this.isWalking && !walking)
        {
            this.isWalking = walking;
            GetComponent<ClientEntity>().CallServer("SetAction", "idle");
        }
    }
}