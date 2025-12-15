using UnityEngine;
using System;
using PublisherSubscriber;

public class SubscribeSelectedPose : MonoBehaviour
{
    [SerializeField] private GameObject[] smallStatueObjects;
    [SerializeField] private GameObject currentSmallStatue;
    [SerializeField] private GameObject[] bigStatueObjects;
    [SerializeField] private GameObject currentBigStatue;



    void Start()
    {
        SelectPoseEventHandler publisher = SelectPoseEventHandler.Instance;
        publisher.PoseChosen += OnPoseSelected;
    }


    private void OnPoseSelected(object sender, string poseName)
    {   
        // replace small statue
        currentSmallStatue.SetActive(false);
        
        GameObject smallStatue = Array.Find(smallStatueObjects, p => p.name == poseName);
        smallStatue.SetActive(true);
        currentSmallStatue = smallStatue;

        // replace big statue
        currentBigStatue.SetActive(false);
        
        GameObject bigStatue = Array.Find(bigStatueObjects, p => p.name == poseName);
        bigStatue.SetActive(true);
        currentBigStatue = bigStatue;

        Debug.Log("STATUE >>> activated: " + poseName);
    }
}
