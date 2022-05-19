using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation.Samples
{

    [RequireComponent(typeof(ARRaycastManager))]
    public class PlaceOnPlane : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Instantiates this prefab on a plane at the touch location.")]
        GameObject m_PlacedPrefab;

        static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

        ARRaycastManager m_RaycastManager;

        public GameObject ClosePortalBtn;

        public GameObject spawnedObject { get; private set; }

        bool videoPlaying = true;

        void Awake()
        {
            m_RaycastManager = GetComponent<ARRaycastManager>();
        }
        public void ClosePortal()
        {
            ClosePortalBtn.SetActive(false);
            spawnedObject.GetComponentInChildren<PortalManager>().Close();
            spawnedObject = null;
        }
        public void PlayPauseVideo(Image img)
        {
            if (videoPlaying == true)
            {
                img.fillCenter = true;
                videoPlaying = false;
                spawnedObject.GetComponentInChildren<PortalManager>().videoPlayer.Pause();
            }
            else
            {
                img.fillCenter = false;
                videoPlaying = true;
                spawnedObject.GetComponentInChildren<PortalManager>().videoPlayer.Play();
            }
        }
        void Update()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    var touchPosition = touch.position;

                    bool isOverUI = touchPosition.IsPointOverUIObject();

                    if (!isOverUI && m_RaycastManager.Raycast(touchPosition, s_Hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                    {
                        var hitPose = s_Hits[0].pose;
                        if (spawnedObject == null)
                        {
                            spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);

                            //spawnedObject.transform.LookAt(MainCamera.transform);
                            //spawnedObject.transform.eulerAngles = new Vector3(0, spawnedObject.transform.rotation.y + 90f, 0);
                            ClosePortalBtn.SetActive(true);
                            spawnedObject.transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y + 90f, 0);
                        }
                    }
                }
            }
        }
    }
}
