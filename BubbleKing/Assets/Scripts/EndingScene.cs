using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingScene : MonoBehaviour
{
    public GameObject image1;
    public GameObject image2;
    public GameObject image3;
    public GameObject image4;

    public float timer = 0;
    public bool isEndingAnimationPlaying = false;
    public float imageTransitionTime = 2.0f;

    private float heightWs;
    private float widthWs;
    
    private Vector3 startPositionImage1;
    private Vector3 startPositionImage2;
    private Vector3 startPositionImage3;
    private Vector3 startPositionImage4;

    public float imageVelocity;
    public GameManager gameManager;

    public int levelnumber;
    // Start is called before the first frame update
    void Start()
    {
        
        heightWs = Camera.main.ScreenToWorldPoint(new Vector3(0.0f, Screen.height, -10.0f)).y * 2;
        widthWs = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0.0f, -10.0f)).x * 2;
        transform.Translate(Vector2.up * heightWs * (levelnumber-1));
        image1.transform.localScale = new Vector3(widthWs, heightWs, 1.0f);
        image2.transform.localScale = new Vector3(widthWs, heightWs, 1.0f);
        image3.transform.localScale = new Vector3(widthWs, heightWs, 1.0f);
        image4.transform.localScale = new Vector3(widthWs, heightWs, 1.0f);
        image2.transform.Translate(Vector2.up * heightWs);
        image1.transform.Translate(Vector2.left * widthWs);
        image3.transform.Translate(Vector2.right * widthWs );
        image4.transform.Translate(Vector2.left * widthWs + Vector2.up * heightWs);
        
        startPositionImage1 = image1.transform.position;
        startPositionImage2 = image2.transform.position;
        startPositionImage3 = image3.transform.position;
        startPositionImage4 = image4.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isEndingAnimationPlaying)
        {
            timer += Time.deltaTime;
            if (timer > imageTransitionTime)
            {
                
                Vector3 newPos = Vector3.MoveTowards(image1.transform.position, startPositionImage1 + new Vector3(widthWs, 0.0f, 0.0f), imageVelocity * Time.deltaTime);
                image1.transform.position = newPos;
            }

            if (timer > imageTransitionTime * 2)
            {
                Vector3 newPos = Vector3.MoveTowards(image2.transform.position, startPositionImage2 - new Vector3(0.0f, heightWs, 0.0f), imageVelocity * Time.deltaTime);
                image2.transform.position = newPos;
            }
            
            if (timer > imageTransitionTime * 3)
            {
                Vector3 newPos = Vector3.MoveTowards(image3.transform.position, startPositionImage3 - new Vector3(widthWs, 0.0f, 0.0f), imageVelocity * Time.deltaTime);
                image3.transform.position = newPos;
            }
            
            if (timer > imageTransitionTime * 4)
            {
                Vector3 newPos = Vector3.MoveTowards(image4.transform.position, startPositionImage4 + new Vector3(widthWs, -heightWs, 0.0f), imageVelocity * Time.deltaTime);
                image4.transform.position = newPos;
            }

            if (timer > imageTransitionTime * 5)
            {
                isEndingAnimationPlaying = false;
                gameManager.EndingAnimationEnded();
                image1.transform.position = startPositionImage1;
                image2.transform.position = startPositionImage2;
                image3.transform.position = startPositionImage3;
                image4.transform.position = startPositionImage4;
            }
            
        }
    }

    public void StartEndingAnimation()
    {
        isEndingAnimationPlaying = true;
        timer = 0;
    }
}
