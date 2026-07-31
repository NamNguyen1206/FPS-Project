using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace scgFullBodyController
{
    public class CameraControlledIK : MonoBehaviour
    {
        public Transform spineToOrientate;

        // Update is called once per frame
        void LateUpdate()
        {
            if (Time.timeScale == 0f)
                return;
            spineToOrientate.rotation = transform.rotation;
        }
    }
}
