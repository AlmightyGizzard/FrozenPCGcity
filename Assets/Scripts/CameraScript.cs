using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    float Xmouse = 0;
    float Ymouse = 0;
    public float sensitivity = 2;
    public Transform camObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Xmouse += Input.GetAxis("Mouse X") * sensitivity;
        Ymouse -= Input.GetAxis("Mouse Y") * sensitivity;

        Ymouse = Mathf.Clamp(Ymouse, -90f, 90f);

        transform.localEulerAngles = new Vector3(Ymouse, 0, 0);
        camObj.localEulerAngles = new Vector3(0, Xmouse, 0);
    }
}
