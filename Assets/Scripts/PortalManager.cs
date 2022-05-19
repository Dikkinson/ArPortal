using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Video;

public class PortalManager : MonoBehaviour
{
    GameObject MainCamera;

    public GameObject ObjectInside;

    private Material[] ObjectMaterials;

    private Material PortalPlaneMaterial;

    [HideInInspector]
    public VideoPlayer videoPlayer;
    [HideInInspector]
    public double time;
    [HideInInspector]
    public double currentTime;

    void Start()
    {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        videoPlayer = ObjectInside.GetComponent<VideoPlayer>();
        ObjectMaterials = ObjectInside.GetComponent<Renderer>().sharedMaterials;
        PortalPlaneMaterial = GetComponent<Renderer>().sharedMaterial;
        for (int i = 0; i < ObjectMaterials.Length; i++)
        {
            ObjectMaterials[i].SetInt("_StencilComp", (int)CompareFunction.Equal);
        }
        PortalPlaneMaterial.SetInt("_CullMode", (int)CullMode.Back);
        videoPlayer.loopPointReached += VideoPlayer_loopPointReached;
    }

    private void VideoPlayer_loopPointReached(VideoPlayer source)
    {
        Close();
    }

    public void Close()
    {
        Destroy(transform.parent.gameObject, 1.6f);
        GetComponentInParent<AudioSource>().Play();
        GetComponentInParent<Animation>().Play("PortalCloseAnim");
    }
    void OnTriggerStay(Collider collider)
    {
        Vector3 camPosInPortalSpace = transform.InverseTransformPoint(MainCamera.transform.position);

        if (camPosInPortalSpace.y <= 0.0f) //inside portal
        {
            for (int i = 0; i < ObjectMaterials.Length; i++)
            {
                ObjectMaterials[i].SetInt("_StencilComp", (int)CompareFunction.NotEqual);
            }

            PortalPlaneMaterial.SetInt("_CullMode",(int)CullMode.Front);

        }
        else if (camPosInPortalSpace.y < 0.5f)// outside portal
        {
            for (int i = 0; i < ObjectMaterials.Length; i++)
            {
                ObjectMaterials[i].SetInt("_StencilComp", (int)CompareFunction.Always);
            }
            PortalPlaneMaterial.SetInt("_CullMode", (int)CullMode.Off);
        }
        else
        {
            for (int i = 0; i < ObjectMaterials.Length; i++)
            {
                ObjectMaterials[i].SetInt("_StencilComp", (int)CompareFunction.Equal);
            }
            PortalPlaneMaterial.SetInt("_CullMode", (int)CullMode.Back);
        }
    }
}
