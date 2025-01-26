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
    public float shakeMagnitude = 0.5f;
    public float shakeDuration = 0.5f;
    
    public float suckForce = 20;
    public float blowForce = 20;
    public float frictionValue = 0.5f;
    public float rotationValue = 0.5f;
    public float dropTorqueForce = 20f;
    public float dropForce = 20f;
    public float dropBlowForce = 20f;
    public float minSize = 0.3f;
    public float endingDropVelocity = 0.5f;
    public float endingAngularVelocity = 5f;
    public Vector3 currentMousePos = Vector3.zero;
    
    public Rigidbody2D rb2d;
    public Collider2D col2d;

    public float blowSizeChange = 0.001f;
    public float suckSizeChange = 0.005f;
    
    public CameraScript cam;
    
    public GameManager gm;

    public Sprite idle;
    public Sprite blow;
    public Sprite suck;
    public Sprite impazzito;
    
    public SpriteRenderer face;
    public ParticleSystem blowParticle;
    public ParticleSystem suckParticle;
    public ParticleSystem impazzendoParticle;
    
    public ParticleSystem.MainModule blowParticleMainModule;
    public ParticleSystem.MainModule suckParticleMainModule;
    public ParticleSystem.MainModule impazzendoParticleMainModule;
    
    public GameObject bubbleSprite;
    public float bubbleDeformationForce = 20f;
    public float bubbleDeforationSpeed = 20f;
    
    public SpriteRenderer crownSprite;

    public GameObject startingPoint;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        blowParticleMainModule = blowParticle.main;
        suckParticleMainModule = suckParticle.main;
        impazzendoParticleMainModule = impazzendoParticle.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (canPlayerMove && !gm.hasGameEnded)
        {
            PlayerInput();

            RotateToMouse();
            
        }else if(isImpazzendo && !gm.hasGameEnded)
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
            face.sprite = idle;
            rb2d.angularVelocity = 0.0f;
            canPlayerMove = true; 
            isImpazzendo = false;
            countImpazzendoTime = 0f;
            gameObject.layer = LayerMask.NameToLayer("Player");
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
            face.sprite = suck;
            isLeftMouseDown = true;
            currentMousePos= Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentMousePos.z=transform.position.z;
            if (!suckParticle.isEmitting)
            {
                suckParticle.Play();
            }
        }else {
            isLeftMouseDown = false;
            suckParticle.Stop();
        }

        if (Input.GetMouseButton(1) && transform.localScale.x > minSize)
        {
            face.sprite = blow;
            isRightMouseDown = true;
            currentMousePos= Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentMousePos.z=transform.position.z;
            if (!blowParticle.isEmitting)
            {
                blowParticle.Play();
            }
        }else {
            isRightMouseDown = false;
            blowParticle.Stop();
        }

        if (!(Input.GetMouseButton(0) || (Input.GetMouseButton(1) && transform.localScale.x > minSize)))
        {
            face.sprite = idle;
        }
    }

    private void FixedUpdate()
    {
        if (!gm.hasGameEnded)
        {
            if (isLeftMouseDown)
            {
                rb2d.AddRelativeForce(new Vector2(suckForce, 0.0f));
                transform.localScale += new Vector3(suckSizeChange, suckSizeChange, suckSizeChange);
                
                blowParticleMainModule.startSize = new ParticleSystem.MinMaxCurve(
                    blowParticleMainModule.startSize.constantMin + suckSizeChange,
                    blowParticleMainModule.startSize.constantMax + suckSizeChange);
                suckParticleMainModule.startSize = new ParticleSystem.MinMaxCurve(
                    suckParticleMainModule.startSize.constantMin + suckSizeChange,
                    suckParticleMainModule.startSize.constantMax + suckSizeChange);
                impazzendoParticleMainModule.startSize = new ParticleSystem.MinMaxCurve(
                    impazzendoParticleMainModule.startSize.constantMin + suckSizeChange,
                    impazzendoParticleMainModule.startSize.constantMax + suckSizeChange);

                float scaleChangeX = Mathf.Sin(Time.time * bubbleDeforationSpeed) * bubbleDeformationForce;
                float scaleChangeY = Mathf.Sin(Time.time * bubbleDeforationSpeed + Mathf.PI / 2) * bubbleDeformationForce;
                bubbleSprite.transform.localScale +=
                    new Vector3(suckSizeChange + scaleChangeX, suckSizeChange + scaleChangeY, suckSizeChange);

            } else if (isRightMouseDown)
            {
                rb2d.AddRelativeForce(new Vector2(-blowForce, 0.0f));
                
                transform.localScale-= new Vector3(blowSizeChange, blowSizeChange, blowSizeChange);
                
                blowParticleMainModule.startSize = new ParticleSystem.MinMaxCurve(
                    blowParticleMainModule.startSize.constantMin - blowSizeChange,
                    blowParticleMainModule.startSize.constantMax - blowSizeChange);
                suckParticleMainModule.startSize = new ParticleSystem.MinMaxCurve(
                    suckParticleMainModule.startSize.constantMin - blowSizeChange,
                    suckParticleMainModule.startSize.constantMax - blowSizeChange);
                impazzendoParticleMainModule.startSize = new ParticleSystem.MinMaxCurve(
                    impazzendoParticleMainModule.startSize.constantMin - blowSizeChange,
                    impazzendoParticleMainModule.startSize.constantMax - blowSizeChange);
                float scaleChangeX = Mathf.Sin(Time.time * bubbleDeforationSpeed) * bubbleDeformationForce;
                float scaleChangeY = Mathf.Sin(Time.time * bubbleDeforationSpeed + Mathf.PI / 2.0f) * bubbleDeformationForce;
                bubbleSprite.transform.localScale -=
                    new Vector3(blowSizeChange + scaleChangeX, blowSizeChange + scaleChangeY, blowSizeChange);
            }
            else
            {
                bubbleSprite.transform.localScale = transform.localScale;
            }

            if (isImpazzendo)
            {
                if (countImpazzendoTime < maxImpazzendoTime)
                {
                    rb2d.AddRelativeForce(new Vector2(-dropBlowForce, 0.0f));

                    if (transform.localScale.x > minSize)
                    {
                        transform.localScale -= new Vector3(blowSizeChange, blowSizeChange, blowSizeChange);

                        blowParticleMainModule.startSize = new ParticleSystem.MinMaxCurve(
                            blowParticleMainModule.startSize.constantMin - blowSizeChange,
                            blowParticleMainModule.startSize.constantMax - blowSizeChange);
                        suckParticleMainModule.startSize = new ParticleSystem.MinMaxCurve(
                            suckParticleMainModule.startSize.constantMin - blowSizeChange,
                            suckParticleMainModule.startSize.constantMax - blowSizeChange);
                        impazzendoParticleMainModule.startSize = new ParticleSystem.MinMaxCurve(
                            impazzendoParticleMainModule.startSize.constantMin - blowSizeChange,
                            impazzendoParticleMainModule.startSize.constantMax - blowSizeChange);
                    }
                }
            }

            rb2d.AddForce(-rb2d.velocity * frictionValue);
        }
        else if (gm.isEndingAnimationPlaying)
        {
            gameObject.layer = LayerMask.NameToLayer("ImpazzitoPlayer");
            face.sprite = impazzito;
            crownSprite.enabled = true;
            rb2d.velocity = new Vector2(0, -endingDropVelocity);
            rb2d.angularVelocity = endingAngularVelocity;
            if (transform.localScale.x > minSize)
            {
                transform.localScale -= new Vector3(blowSizeChange, blowSizeChange, blowSizeChange);

                blowParticleMainModule.startSize = new ParticleSystem.MinMaxCurve(
                    blowParticleMainModule.startSize.constantMin - blowSizeChange,
                    blowParticleMainModule.startSize.constantMax - blowSizeChange);
                suckParticleMainModule.startSize = new ParticleSystem.MinMaxCurve(
                    suckParticleMainModule.startSize.constantMin - blowSizeChange,
                    suckParticleMainModule.startSize.constantMax - blowSizeChange);
                impazzendoParticleMainModule.startSize = new ParticleSystem.MinMaxCurve(
                    impazzendoParticleMainModule.startSize.constantMin - blowSizeChange,
                    impazzendoParticleMainModule.startSize.constantMax - blowSizeChange);
                bubbleSprite.transform.localScale = transform.localScale;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!isImpazzendo && !other.gameObject.CompareTag("Water"))
        {
            Debug.Log("CollisionEnter");
            face.sprite = impazzito;
            canPlayerMove = false;
            isImpazzendo = true;
            isLeftMouseDown = false;
            isRightMouseDown = false;
            suckParticle.Stop();
            if (!blowParticle.isEmitting)
            {
                blowParticle.Play();
                impazzendoParticle.Play();
            }
            rb2d.angularVelocity = dropTorqueForce;
            rb2d.AddForce(new Vector2(0.0f, -dropForce), ForceMode2D.Impulse);
            gameObject.layer = LayerMask.NameToLayer("ImpazzitoPlayer");
            cam.TriggerShake(shakeDuration, shakeMagnitude);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            rb2d.velocity = Vector2.zero;
            face.sprite = idle;
            suckParticle.Stop();
            blowParticle.Stop();
            bubbleSprite.transform.localScale = transform.localScale;
            OnWinCondition();
        }

        if (gm.hasGameEnded && other.gameObject.CompareTag("Start"))
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
            gm.StartNewGame();
            rb2d.velocity = Vector2.zero;
            rb2d.angularVelocity = 0.0f;
            face.sprite = idle;
        }
    }

    private void OnWinCondition()
    {
        gm.PlayerWin();
    }
}
