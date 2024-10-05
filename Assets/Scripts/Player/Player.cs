using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

  [SerializeReference]  private float moveSpeed = 7.5f;
  private Rigidbody2D rb;
  private float moveInput;
  
  private void Start() {
    rb = GetComponent<Rigidbody2D>();
  }

  private void Update() {
    Move();
  }

  private void Move(){
    moveInput = UserInput.instance.moveInput.x;
    rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
  }
}
