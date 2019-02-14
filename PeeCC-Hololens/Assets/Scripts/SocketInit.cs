using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WindowsSocketWrapper;

public class SocketInit : MonoBehaviour
{

    public TextMesh text;
    public TextMesh check_width_height;
    public string host = "ws://192.168.50.132:3001/socket.io/?EIO=4&transport=websocket";
    public GameObject moveObject;
    public GameObject object_move;
    public GameObject moveObject2;
    public GameObject pen;
    private SocketWrapper socketWrapper;
    private string ReceivedMessage;
    private Camera mainCamera;
    public GameObject Cusor;
    private string iconBase64String = "";
    public Texture2D convertedBase64String;

    
    int status_move = 0;
    int status_arraw = 0;
    float x_position = 25000f;
    float y_position = 5000f;
    float prevX = 0.0f;
    float prevY = 0.0f;
    int image_push_x = 0;
    int image_push_y = 0;
    int image_push_width = 700;
    int image_push_height = 500;
    int image_rotate = 1;
    float image_scale = 0.0f;
    string test_string = "";	
    int check_trial = 0;

    // Use this for initialization
    void Start()
    {
        mainCamera = Camera.main;
        //GameObject MyObject = Resources.Load("Move") as GameObject;
        //object_move = Instantiate(MyObject) as GameObject;

    }
    public void OnStart()
    {
        socketWrapper = new SocketWrapper();
#if !UNITY_EDITOR
        socketWrapper.Connect(host);
        socketWrapper.OnReceiveMessenger += OnMessageArrived_Handler;
        socketWrapper.onDisconnect += OnDisconnect_Handler;

#endif
    }

    // Update is called once per frame
    void OnGUI()
    {
        if (iconBase64String != "")
        {
            try
            {
                //preString = stringtest;
                Texture2D newPhoto = new Texture2D(1, 1);
                convertedBase64String = new Texture2D(image_push_width, image_push_height);
                byte[] decodedBytes = Convert.FromBase64String(iconBase64String);
                convertedBase64String.LoadImage(decodedBytes);
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                //    // cube.transform.localScale = new Vector3(0.015f, 0.015f, 0.015f);
                //    // cube.transform.position = new Vector3(pointInRealWorld.x, pointInRealWorld.y, pointInRealWorld.z);
                cube.transform.localScale = new Vector3(0.28f * image_scale, 0.28f * image_scale, 0.005f);

                cube.AddComponent<BoxCollider>();
                //cube.AddComponent<Move>();
                cube.AddComponent<ClickImageAction>();
                cube.GetComponent<Renderer>().material.mainTexture = convertedBase64String;
                //text.text = iconBase64String.Length.ToString() + iconBase64String[0] + iconBase64String[1] + iconBase64String[iconBase64String.Length - 2] + iconBase64String[iconBase64String.Length - 1];
                //text.text = image_push_width + " " + image_push_height + " " + image_push_x + " " + image_push_y;
                text.text = test_string;
                iconBase64String = "";
                Vector3 poinGetposition = new Vector3();
                //cube.transform.Rotate(new Vector3(mainCamera.transform.rotation.x, mainCamera.transform.rotation.y, 180+image_rotate));
                cube.transform.eulerAngles = new Vector3(transform.eulerAngles.x, mainCamera.transform.eulerAngles.y, 180 - image_rotate);
                poinGetposition = mainCamera.ScreenToWorldPoint(new Vector3(image_push_x, image_push_y, mainCamera.nearClipPlane));
                //real screen positions
                float poi_cam_x = mainCamera.transform.position.x;
                float poi_cam_y = mainCamera.transform.position.y;
                float poi_cam_z = mainCamera.transform.position.z;
                float x_test = (5 * poinGetposition.x - poi_cam_x) / 4;
                float y_test = (5 * poinGetposition.y - poi_cam_y) / 4;
                float z_test = (5 * poinGetposition.z - poi_cam_z) / 4;
                cube.transform.position = new Vector3(x_test, y_test, z_test);
            }
            catch (Exception abcxyz)
            {
                Debug.Log(abcxyz.ToString());
            }

        }


		
        if ((prevX != x_position || prevY != y_position) && x_position < 1500 && y_position<1500)
        {
					 if (status_move == 1)
					{

						object_move = GameObject.CreatePrimitive(PrimitiveType.Cube);
						object_move.transform.localScale = new Vector3(0.0015f, 0.0015f, 0.0015f);
						status_move = 0;
						check_trial = 1;
						//check_width_height.text = string.Format("check is : {0} and {1}", x_position, y_position);

					}
                    prevX = x_position;
                    prevY = y_position;

                    Vector3 pointInRealWorld = new Vector3();

                    //size of camera
                    int camWidth = mainCamera.pixelWidth;
                    int camHeight = mainCamera.pixelHeight;
                    //ratio;
                    float ratioWidth = 1328.0f / camWidth;
                    float ratioHeight = 746.0f / camHeight;

                    //real screen positions
                    float poi_cam_x = mainCamera.transform.position.x;
                    float poi_cam_y = mainCamera.transform.position.y;
                    float poi_cam_z = mainCamera.transform.position.z;
                    // float cusor_z = Cusor.transform.position.z;
                    float cusor_z = Cusor.transform.position.z;



                    float screenX = x_position / ratioWidth;
                    float screenY = camHeight - (y_position / ratioHeight);
                    text.text = string.Format("position is : {0} and {1} and {2} and {3}", x_position, y_position, screenX, screenY);
                    //manipulate point in real - world
                    pointInRealWorld = mainCamera.ScreenToWorldPoint(new Vector3(screenX, screenY, mainCamera.nearClipPlane));
                    //moveObject.transform.position = pointInRealWorld;

                    //caculator
                    float vecto_x = -poi_cam_x + pointInRealWorld.x;
                    float vecto_y = -poi_cam_y + pointInRealWorld.y;
                    float vecto_z = -poi_cam_z + pointInRealWorld.z;

                    float x_cusor = (cusor_z - pointInRealWorld.z) * vecto_x / vecto_z + pointInRealWorld.x;
                    float y_cusor = (cusor_z - pointInRealWorld.z) * vecto_y / vecto_z + pointInRealWorld.y;

                    float kc1 = (float)Math.Sqrt((pointInRealWorld.x - poi_cam_x) * (pointInRealWorld.x - poi_cam_x) + (pointInRealWorld.y - poi_cam_y) * (pointInRealWorld.y - poi_cam_y) + (pointInRealWorld.z - poi_cam_z) * (pointInRealWorld.z - poi_cam_z));
                    float z1 = (float)Math.Sqrt(0.4f - (pointInRealWorld.x - poi_cam_x) * (pointInRealWorld.x - poi_cam_x) - (pointInRealWorld.y - poi_cam_y) * (pointInRealWorld.y - poi_cam_y)) + pointInRealWorld.z;
                    float z2 = pointInRealWorld.z - (float)Math.Sqrt(0.4f - (pointInRealWorld.x - poi_cam_x) * (pointInRealWorld.x - poi_cam_x) - (pointInRealWorld.y - poi_cam_y) * (pointInRealWorld.y - poi_cam_y));
                    float kc3 = (float)Math.Sqrt((pointInRealWorld.x - poi_cam_x) * (pointInRealWorld.x - poi_cam_x) + (pointInRealWorld.y - poi_cam_y) * (pointInRealWorld.y - poi_cam_y) + (poi_cam_z - z1) * (poi_cam_z - z1));
                    float kc4 = (float)Math.Sqrt((pointInRealWorld.x - poi_cam_x) * (pointInRealWorld.x - poi_cam_x) + (pointInRealWorld.y - poi_cam_y) * (pointInRealWorld.y - poi_cam_y) + (poi_cam_z - z2) * (poi_cam_z - z2));
                    float td_x = (poi_cam_x + pointInRealWorld.x) / 2;
                    float td_y = (poi_cam_y + pointInRealWorld.y) / 2;
                    float td_z = (poi_cam_y + pointInRealWorld.z) / 2;
                    float x_test = (5 * pointInRealWorld.x - poi_cam_x) / 4;
                    float y_test = (5 * pointInRealWorld.y - poi_cam_y) / 4;
                    float z_test = (5 * pointInRealWorld.z - poi_cam_z) / 4;
                    //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    // cube.transform.localScale = new Vector3(0.015f, 0.015f, 0.015f);
                    // cube.transform.position = new Vector3(pointInRealWorld.x, pointInRealWorld.y, pointInRealWorld.z);
                    //cube.transform.localScale = new Vector3(0.015f, 0.015f, 0.015f);
                    object_move.transform.position = new Vector3(x_test, y_test, z_test);
					if (check_trial == 1) {                
						object_move.AddComponent<TrailRenderer>();
						object_move.GetComponent<TrailRenderer>().minVertexDistance = 0.001f;
						object_move.GetComponent<TrailRenderer>().startWidth = 0.01f;
						object_move.GetComponent<TrailRenderer>().endWidth = 0.01f;
						object_move.GetComponent<TrailRenderer>().time = 20f;
						check_trial = 0;
					}
            //if (kc4 > kc1)
            //{
            //    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //    // cube.transform.localScale = new Vector3(0.015f, 0.015f, 0.015f);
            //    // cube.transform.position = new Vector3(pointInRealWorld.x, pointInRealWorld.y, pointInRealWorld.z);
            //     cube.transform.localScale = new Vector3(0.015f, 0.015f, 0.015f);
            //     cube.transform.position = new Vector3(pointInRealWorld.x, pointInRealWorld.y, z2);
            //}
            //else
            //{
            //    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //    // cube.transform.localScale = new Vector3(0.015f, 0.015f, 0.015f);
            //    // cube.transform.position = new Vector3(pointInRealWorld.x, pointInRealWorld.y, pointInRealWorld.z);
            //    cube.transform.localScale = new Vector3(0.015f, 0.015f, 0.015f);
            //    cube.transform.position = new Vector3(pointInRealWorld.x, pointInRealWorld.y, z1);

            //}

            //GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);

            //cube2.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            //cube2.transform.position = new Vector3(pointInRealWorld.x, pointInRealWorld.y, pointInRealWorld.z);
            //cube2.transform.position = new Vector3(x_cusor2, y_cusor2, y_cusor2);
            //moveObject2.transform.localScale = new Vector3(0.007f, 0.007f, 0.007f);
            //moveObject2.transform.position = new Vector3(screenX, screenY, mainCamera.nearClipPlane);
            //check_width_height.text = x_cusor.ToString() + " " + y_cusor.ToString() + " " + cusor_z.ToString();

            if (status_arraw == 1)
            {
                GameObject MyObject = Resources.Load("arraw") as GameObject;
                GameObject object_arraw = Instantiate(MyObject) as GameObject;
                object_arraw.transform.localScale = new Vector3(10.0f, 10.0f, 10.0f);
                object_arraw.transform.Rotate(180f, 0f, 80f);
                object_arraw.transform.position = new Vector3(x_test, y_test, z_test);
                status_arraw = 0;
            }

        }

    }

    public void drawobject()
    {
        GameObject MyObject = Resources.Load("pencil_a") as GameObject;
        GameObject object_draw = Instantiate(MyObject) as GameObject;
        object_draw.AddComponent<TrailRenderer>();
        object_draw.GetComponent<TrailRenderer>().minVertexDistance = 0.001f;
        object_draw.GetComponent<TrailRenderer>().startWidth = 0.01f;
        object_draw.GetComponent<TrailRenderer>().endWidth = 0.01f;
        object_draw.GetComponent<TrailRenderer>().time = 20f;
    }

    //Message Arrived Handler
    private void OnMessageArrived_Handler(string msg)
    {
        try
        {
            //received message here
            string k = msg;
            string[] start = k.Split(new string[] { "Start_move" }, StringSplitOptions.None);
            string[] end = k.Split(new string[] { "End_move" }, StringSplitOptions.None);
            string[] arraw = k.Split(new string[] { "Start_arrow" }, StringSplitOptions.None);
            string[] base64image = k.Split(new string[] { "base64string" }, StringSplitOptions.None);
            string[] base64position = k.Split(new string[] { "base64position" }, StringSplitOptions.None);
            if (base64image.Length == 2)
            {
                iconBase64String = base64image[1].Substring(0, base64image[1].Length - 3);
                //stringtest = iconBase64String;
            }
            else
            {
                if (base64position.Length == 2)
                {
                    string[] iconBase64data = (base64position[1].Substring(0, base64position[1].Length - 3)).Split(',');
                    test_string = base64position[1].Substring(0, base64position[1].Length - 3);
                    image_push_x = Convert.ToInt32(iconBase64data[0]);
                    image_push_y = Convert.ToInt32(iconBase64data[1]);
                    image_push_width = Convert.ToInt32(iconBase64data[2]);
                    image_push_height = Convert.ToInt32(iconBase64data[3]);
                    image_rotate = Convert.ToInt32(iconBase64data[4]);
                    image_scale = float.Parse(iconBase64data[5]);
                }
                else
                {
                    if (start.Length == 2)
                    {
                        status_move = 1;
                    }
                    if (arraw.Length == 2)
                    {
                        status_arraw = 1;
                    }

                    string a = k.Split(':')[3];
                    string c = k.Split(':')[4];
                    string b = string.Empty;
                    string e = string.Empty;
                    for (int i = 0; i < a.Length; i++)
                    {
                        if (Char.IsDigit(a[i]))
                            b += a[i];
                    }


                    for (int i = 0; i < c.Length; i++)
                    {
                        if (Char.IsDigit(c[i]))
                            e += c[i];
                    }
                    x_position = float.Parse(b);
                    y_position = float.Parse(e);

                }
                



            }


        }
        catch (System.Exception ex)
        {
            Debug.Log("Message arrived");
            Debug.Log(ex.Message);
        }
    }

    //Disconnect Event Handler
    private void OnDisconnect_Handler(bool connect)
    {
#if !UNITY_EDITOR
        try
        {
            if (!connect)
            {
                socketWrapper.Connect(host);
            }
            else
            {
                    // text.text = "Successfully Disconnected";
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
#endif
    }
}
