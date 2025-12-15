using System;
using UnityEngine;

namespace PublisherSubscriber
{
    public class SelectPoseEventHandler : MonoBehaviour
    {
        public static SelectPoseEventHandler Instance { get; private set; }
        public event EventHandler<string> PoseChosen;

        void Awake()
        {
            // singleton because it is a shared context
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }


        void Update()
        {

        }


        public void PublishSelectedPose(string selectedPoseName)
        {
            PoseChosen?.Invoke(this, selectedPoseName);
            Debug.Log("STATUE >>> selected pose: " + selectedPoseName);
        }
    }
}