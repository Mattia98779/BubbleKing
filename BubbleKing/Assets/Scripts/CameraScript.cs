using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraScript : MonoBehaviour
{
    public GameObject player;
    private float transitionDistance;
    
    private Vector3 originalPosition;
    private bool isShaking = false;
    private float shakeDuration = 0.0f;
    private float shakeMagnitude = 0.1f;
    private float shakeTime = 0.0f;

    public AudioSource audioSource;
    public AudioClip crownClip;
    private int currentCameraPosition = 1;
    public int levelNumber = 5;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource.volume = 0f;
        transitionDistance = Camera.main.ScreenToWorldPoint(new Vector3(0.0f, Screen.height, 0.0f)).y;
    }

    // Update is called once per frame
    void Update()
    {

        if (levelNumber - currentCameraPosition == 0)
        {
            audioSource.volume = 1f;
        }
        else if (levelNumber - currentCameraPosition == 1)
        {
            audioSource.volume = 0.5f;
        }
        else
        {
            audioSource.volume = 0f;
        }
    if (isShaking)
        {
            if (shakeTime > shakeDuration)
            {
                isShaking = false;
                transform.position = originalPosition;
            }
            else
            {
                transform.position = originalPosition + (Vector3)Random.insideUnitCircle * shakeMagnitude;
                shakeTime += Time.deltaTime;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isShaking)
        {
            Vector2 screenPlayerPosition = Camera.main.WorldToScreenPoint(player.transform.position);
            if (screenPlayerPosition.y > Screen.height)
            {
                transform.Translate(transitionDistance * 2 * Vector2.up);
                currentCameraPosition++;
            }
            else if (screenPlayerPosition.y < 0.0f)
            {
                transform.Translate(transitionDistance * 2 * Vector2.down);
                currentCameraPosition--;
            }
        }
    }

    public void TriggerShake(float duration, float shakeMagnitude)
    {
        this.shakeDuration = duration;
        this.shakeMagnitude = shakeMagnitude;
        originalPosition = transform.position;
        shakeTime = 0.0f;
        isShaking = true;
    }
}
