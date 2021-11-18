using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;
using System.IO;
using TMPro;

using System.Globalization;

public class MenuController : MonoBehaviour
{
    // It's the variable with the input in the Menu to define the number of microtubulos
    public Slider microInput;

    public AmbienteScript ambiente;

    public Camera principalCamera;

    public Text numMicrotubulos;

    public Slider slider;


    void Start(){
        Debug.Log("Start");
        principalCamera.enabled = false;
    }

    void Update() {
        
    }

    public void comenzarSimulacion(){
        //int numMicro = int.Parse(temp, CultureInfo.InvariantCulture.NumberFormat);

        GameObject menu = GameObject.FindGameObjectWithTag("Menu");
        Destroy(menu);
        Debug.Log(int.Parse(microInput.value.ToString()));
        principalCamera.enabled = true;
        ambiente.startSimulation(microInput.value.ToString());

        
    }

    public void updateSlider(){
        int value = int.Parse(slider.value.ToString());
        this.numMicrotubulos.text = "Microchannels: "+value;
    }
}
