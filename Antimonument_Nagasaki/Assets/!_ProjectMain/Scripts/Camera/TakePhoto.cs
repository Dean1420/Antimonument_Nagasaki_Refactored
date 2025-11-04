using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TakePhoto : MonoBehaviour
{
    // Start is called before the first frame update
    public UnityEvent onPressed, onReleased;

    //[SerializeField] private float threshold = 0.1f;
    [SerializeField] private float deadZone = 0.025f;

    public Transform Parent;

    private bool _isPressed = false;
    private Vector3 _startPos;

    private ConfigurableJoint _joint;

    [Header("Flash Effect")]
    [SerializeField] private GameObject cameraFlash;
    [SerializeField] private float flashT;

    public GameObject Prefab;
    public GameObject polParent;


    public AudioSource Shutter;
    public GameObject HandCol;

    Rigidbody rg;
    Collider Boxcol;

    public Transform ButtonTrans;
    public Transform MainButton;


    void Start()
    {
        _startPos = transform.localPosition;

        _joint = GetComponent<ConfigurableJoint>();

        rg = this.gameObject.GetComponent<Rigidbody>();
        Boxcol = this.gameObject.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (!_isPressed && transform.localPosition.y <= 0.042)
            LocalPosition = true;*/
        if (_isPressed && transform.localPosition.y >= 1.65)
            Released();

        if (transform.localPosition.y <= 0.27f)
            transform.localPosition = new Vector3(0, 0.27f, 0);

        if (transform.localPosition.y > _startPos.y)
            transform.localPosition = _startPos;


        transform.localEulerAngles = new Vector3(0, 0, 0);
        transform.localPosition = new Vector3(_startPos.x, transform.localPosition.y, _startPos.z);



        /*if (!_isPressed && GetValue() + threshold >= 1)
        Pressed();
        if (_isPressed && GetValue() - threshold <= 0)
        Released();*/

        if (rg.isKinematic == true)
        {
            rg.isKinematic = false;
        }

        if (Boxcol.enabled == false)
        {
            Boxcol.enabled = true;
        }

        
        MainButton.transform.position = ButtonTrans.position;
        MainButton.transform.eulerAngles = ButtonTrans.eulerAngles; 
    }


    void OnCollisionStay(Collision col)
    {
        Debug.Log("Col = " + col.gameObject.name + _isPressed + transform.localPosition.y );

        if (col.gameObject == HandCol && !_isPressed && transform.localPosition.y <= 0.042)
        {
                Debug.Log(col.gameObject);
                Pressed();      
        }

    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log("Stay = " + other.gameObject.name + _isPressed + transform.localPosition.y);

        if (other.gameObject == HandCol && !_isPressed && transform.localPosition.y <= 0.042)
        {
            Debug.Log(other.gameObject);
            Pressed();
        }
    }

    private float GetValue()
    {
        var value = Vector3.Distance(_startPos, transform.localPosition / _joint.linearLimit.limit);

        if (Mathf.Abs(value) < deadZone)
            value = 0;

        return Mathf.Clamp(value, -1f, 1f);
    }
    private void Pressed()
    {
        _isPressed = true;
        onPressed.Invoke();
        Debug.Log("Pressed");
        StartCoroutine(CameraFlashEffect());
    }

    private void Released()
    {
        _isPressed = false;
        onReleased.Invoke();
        Debug.Log("Released");
    }


    IEnumerator CameraFlashEffect()
    {
        Shutter.Play();
        cameraFlash.SetActive(true);
        yield return new WaitForSeconds(flashT);
        cameraFlash.SetActive(false);
        TakePolaroid();
    }


    public void TakePolaroid()
    {
        var NewPolaroid = (GameObject)Instantiate(Prefab, polParent.transform.position, Quaternion.identity);
    }
}
