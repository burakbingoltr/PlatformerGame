using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

  [SerializeField]  private float moveSpeed = 7.5f;
  [SerializeField] private GameObject rightLeg;
  [SerializeField] private GameObject leftLeg;
    [HideInInspector] private bool isFacingRight;
  private Rigidbody2D rb;
  private float moveInput;
  
  private void Start() {
    rb = GetComponent<Rigidbody2D>();
    startDirectionCheck();
  }

  private void Update() {
    Move();
  }

  private void Move(){
    moveInput = UserInput.instance.moveInput.x;
    rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

    if(moveInput > 0 || moveInput < 0)
        {
            turnCheck();
        }
  }

    private void startDirectionCheck()
    {
        if (rightLeg.transform.position.x > leftLeg.transform.position.x)
        {
            isFacingRight = true;
        }
        else
        {
            isFacingRight = false;
        }
    }

    private void turnCheck()
    {
        if(UserInput.instance.moveInput.x > 0 && !isFacingRight)
        {
            Turn();
        }
        else if(UserInput.instance.moveInput.x < 0 && isFacingRight)
        {
            Turn();
        }
    }

    private void Turn()
    {
        if (isFacingRight)
        {
            Vector3 rotator = new Vector3(transform.position.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight= !isFacingRight;
        }
        else
        {
            Vector3 rotator = new Vector3(transform.position.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
        }
    }

}
