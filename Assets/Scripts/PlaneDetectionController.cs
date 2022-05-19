using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

namespace UnityEngine.XR.ARFoundation.Samples
{
    [RequireComponent(typeof(ARPlaneManager))]
    public class PlaneDetectionController : MonoBehaviour
    {
        ARPlaneManager m_ARPlaneManager;

        bool isShowGrid = true;
        public void ShowGrid(Image img)
        {
            img.fillCenter = !isShowGrid;
            isShowGrid = !isShowGrid;
        }
        public void TogglePlaneDetection()
        {
            m_ARPlaneManager.enabled = !m_ARPlaneManager.enabled;

            if (m_ARPlaneManager.enabled)
            {
                SetAllPlanesActive(true);
            }
            else
            {
                SetAllPlanesActive(false);
            }
        }
        public void SetAllPlanesActive(bool value)
        {
            foreach (var plane in m_ARPlaneManager.trackables)
                plane.gameObject.SetActive(value);
        }
        void Awake()
        {
            m_ARPlaneManager = GetComponent<ARPlaneManager>();
        }
        private void Update()
        {
            if (isShowGrid == true)
            {
                SetAllPlanesActive(true);
            }else
            {
                SetAllPlanesActive(false);
            }
        }
    }
}