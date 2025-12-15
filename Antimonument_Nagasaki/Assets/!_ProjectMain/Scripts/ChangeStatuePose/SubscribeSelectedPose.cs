using UnityEngine;
using System;
using PublisherSubscriber;

public class SubscribeSelectedPose : MonoBehaviour
{
    [SerializeField] private GameObject[] statueObjects;
    [SerializeField] private GameObject currentStatue;


    void Start()
    {
        SelectPoseEventHandler publisher = SelectPoseEventHandler.Instance;
        publisher.PoseChosen += OnPoseSelected;
    }


    private void OnPoseSelected(object sender, string poseName)
    {       
        currentStatue.SetActive(false);
        
        GameObject statue = Array.Find(statueObjects, p => p.name == poseName);
        statue.SetActive(true);
        currentStatue = statue;
        Debug.Log("STATUE >>> activated: " + poseName);
    }
}
