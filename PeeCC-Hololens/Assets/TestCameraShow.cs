using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestCameraShow : MonoBehaviour {

    public RawImage RemoteVideoImageRender;
    // Use this for initialization
    void Start () {
        WebCamTexture _webTex = new WebCamTexture();
        _webTex.Play();
        RemoteVideoImageRender.texture = _webTex;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
