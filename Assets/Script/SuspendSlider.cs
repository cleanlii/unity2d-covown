using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuspendSlider : MonoBehaviour
{

    Slider sld;

    // Start is called before the first frame update
    void Start()
    {
        sld = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        sld.value = PlayerController.suspendableTime / PlayerController.maxSuspendTime;
    }
}
