using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public float speed = 10.0f; // La velocidad de movimiento

    private Rigidbody2D rb;
    public Joystick joystick;
    public float distance = 10.0f;

    public Vector2 ray;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
         ray = transform.right;
    }

    private void Update()
    {        
        float moveHorizontal = Input.GetAxis("Horizontal") + joystick.Horizontal;
        float moveVertical = Input.GetAxis("Vertical") + joystick.Vertical;


        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        if (movement.magnitude > 0.2)
        {
            ray = movement;
        }
        
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, ray, distance);
        rb.velocity = movement * speed;
        if (hit.collider != null)
        {
            Debug.Log("El raycast ha golpeado a: ");
        }
    }
}
