using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Webカメラのセット
public class WebCamHandler : MonoBehaviour
{
    private static int INPUT_SIZE_X = 1920;
    private static int INPUT_SIZE_Y = 1080;
    private static int FPS = 30;

    // UI
    RawImage rawImage;
    WebCamTexture webCamTexture;

    // Start is called before the first frame update
    void Start()
    {
        this.rawImage = GetComponent<RawImage>();
        this.webCamTexture = new WebCamTexture(INPUT_SIZE_X, INPUT_SIZE_Y, FPS);
        this.rawImage.texture = this.webCamTexture;
        this.webCamTexture.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
