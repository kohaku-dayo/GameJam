using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class towerHP : MonoBehaviour
{
    int maxHp = 100;
    int crenntHP;
    
    int Zakoenemy = 1;
    int Mediumenemy = 5;
    int Greatenemy = 10;

 


    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider.value = 100;
        crenntHP = maxHp;
       
        Debug.Log("Start crenntHP:" + crenntHP);
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("kk");
        if (collider.gameObject.tag == "tag") 
        {
            crenntHP -= Zakoenemy;
            slider.value = crenntHP / maxHp;

        }   
        if(collider.gameObject.tag == "mediamenmy")
        {
            crenntHP -= Mediumenemy;
            slider.value = crenntHP / maxHp;
        }
        if (collider.gameObject.tag == "daiene")
        {
            crenntHP -= Greatenemy;
            slider.value = crenntHP / maxHp;
        }


    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
