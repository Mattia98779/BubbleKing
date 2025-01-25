using System;
using UnityEngine;
using UnityEngine.UIElements;

public class player : MonoBehaviour
{
    public bool isLeftMouseDown = false;
    public bool isRightMouseDown = false;

    public bool canPlayerMove = true;
    public bool isImpazzendo = false;
    private float countImpazzendoTime = 0f;
    public float maxImpazzendoTime = 2f;
    
    public float suckForce = 20;
    public float blowForce = 20;
    public float frictionValue = 0.5f;
    public float rotationValue = 0.5f;
    public float dropTorqueForce = 20f;
    public float dropForce = 20f;
    public float dropBlowForce = 20f;
    
    public Vector3 currentMousePos = Vector3.zero;
    
    public Rigidbody2D rb2d;
    public Collider2D col2d;

    public float blowSizeChange = 0.001f;
    public float suckSizeChange = 0.005f;
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canPlayerMove)
        {
            PlayerInput();

            RotateToMouse(); 
        }else if(isImpazzendo)
        {
            HandleImpazzito();
        }
        
    }

    private void HandleImpazzito()
    {
        if (countImpazzendoTime < maxImpazzendoTime)
        {
            countImpazzendoTime += Time.deltaTime;  
        }
        else
        {
            rb2d.angularVelocity = 0.0f;
            canPlayerMove = true; 
            isImpazzendo = false;
            countImpazzendoTime = 0f;
        }
    }

    private void RotateToMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 direction = mousePos - transform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float currentAngle = transform.eulerAngles.z;
        float angle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationValue * Time.deltaTime);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void PlayerInput()
    {
        if (Input.GetMouseButton(0))
        {
            isLeftMouseDown = true;
            currentMousePos= Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentMousePos.z=transform.position.z;
        }else {
            isLeftMouseDown = false;
        }

        if (Input.GetMouseButton(1))
        {
            isRightMouseDown = true;
            currentMousePos= Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentMousePos.z=transform.position.z;
        }else {
            isRightMouseDown = false;
        }
    }

    private void FixedUpdate()
    {
        if (isLeftMouseDown)
        {
            rb2d.AddRelativeForce(new Vector2(suckForce, 0.0f));
            transform.localScale+= new Vector3(suckSizeChange, suckSizeChange, suckSizeChange);
        }
        
        if (isRightMouseDown)
        {
            rb2d.AddRelativeForce(new Vector2(-blowForce, 0.0f));
            transform.localScale-= new Vector3(blowSizeChange, blowSizeChange, blowSizeChange);
        }

        if (isImpazzendo)
        {
            if (countImpazzendoTime < maxImpazzendoTime)
            {
                rb2d.AddRelativeForce(new Vector2(-dropBlowForce, 0.0f));
            }
        }

        rb2d.AddForce(-rb2d.velocity * frictionValue);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("CollisionEnter");
        canPlayerMove = false;
        isImpazzendo = true;
        isLeftMouseDown = false;
        isRightMouseDown = false;
        rb2d.angularVelocity = dropTorqueForce;
        rb2d.AddForce(new Vector2(0.0f, -dropForce), ForceMode2D.Impulse);
    }
}
