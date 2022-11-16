using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch4Macro : MonoBehaviour
{
    public GameObject virus1;
    public GameObject virus2;
    public GameObject virus3;
    public GameObject Medicine;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ClearVirus()
    {
        Destroy(virus1);
        Destroy(virus2);
        Destroy(virus3);
    }
}
