using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class VRMap
{
    public Transform vrTarget;
    public Transform ikTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;

    public Transform playerCenter; // Center of player circle
    public Transform statueCenter; // Center of statue body
    public float positionScaleX = 2f;
    public float positionScaleY = 2f;
    public float positionScaleZ = 2f;

    public void Map()
    {
        /* Vector3 localPos = vrTarget.TransformPoint(trackingPositionOffset);

        // Scale the position relative to IK target's parent
        Vector3 scaledPos = ikTarget.parent.position +
            (localPos - ikTarget.parent.position) * positionScale;

        ikTarget.position = scaledPos;
        ikTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset); */



        /*  Vector3 localPos = vrTarget.TransformPoint(trackingPositionOffset);

         // Get offset from player center
         Vector3 offsetFromPlayer = localPos - playerCenter.position;

         // Flip the Z axis (front/back)
         offsetFromPlayer.z *= -1;

         // Scale and apply to statue center
         ikTarget.position = statueCenter.position + (offsetFromPlayer * positionScale);
         ikTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset); */


        Vector3 localPos = vrTarget.TransformPoint(trackingPositionOffset);
        
        // Get offset from player center
        Vector3 offsetFromPlayer = localPos - playerCenter.position;
        
        // Scale per axis
        offsetFromPlayer.x *= positionScaleX;
        offsetFromPlayer.y *= positionScaleY;
        offsetFromPlayer.z *= -positionScaleZ;
        
        ikTarget.position = statueCenter.position + offsetFromPlayer;
        
        // Flip rotation 180 degrees around Y axis
        ikTarget.rotation = Quaternion.Euler(0, 180, 0) * vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}

public class IKTargetFollowVRRig : MonoBehaviour
{
    public VRMap leftHand;
    public VRMap rightHand;
    
    public void Update()
    {
        //UpdatePostion();
    }

    public void UpdatePostion()
    {
        leftHand.Map();
        rightHand.Map();
    }
}