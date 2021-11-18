using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;
using System.IO;



public class AmbienteScript : MonoBehaviour
{   

    public Text timeText;
    public Dropdown cameraOptions;
    List<string> dropOptions = new List<string>();
    public Camera camera;
    public int idCamera = 0;
    private int idCameraTemp;
    public GameObject prefab;
    public GameObject microtubuloPrefab;
    public BacteriaModel bacteriaInstance;
    public List<BacteriaModel> bacterias;
    public List<GameObject> microtubulos = new List<GameObject>();

    private bool running = false;

    

    

    private int totalTime = 5000;
    private int  time = 0;

    private string fileContent = "";

    private float sizeSep = 50;
    private float x = 0;

    private int numMicro = 0;

    
    void Start()
    {

       
    }

    void FixedUpdate(){

        
    }

    public void startSimulation(string numMicro){
        Debug.Log(numMicro);
        this.numMicro = int.Parse("0"+numMicro);
        cameraOptions.ClearOptions();
        DebugGUI.SetGraphProperties("graph", "Size Graph", 0, 1, 3, new Color(0, 1, 1), false);

         bacterias = new List<BacteriaModel>();

        this.initializeObjects();
        this.idCameraTemp = cameraOptions.value;  
        this.running = true;
          
    }

    void Update()
    {
        if(running){
            if(this.time < this.totalTime){
                this.idCamera = cameraOptions.value;
                this.timeText.text = this.time + " / " + this.totalTime;

                this.fileContent += this.time + ";";

                this.fileContent += bacterias[0].getData();

                foreach(BacteriaModel bacteria in this.bacterias.ToArray()){
                
                    if(bacteria.enable){
                        bacteria.simulate();
                    }

                    this.fileContent += bacteria.getData();
                }

                Debug.Log("------------------------------------------------------");
                Debug.Log("Size: "+this.bacterias[0].getSize());
                Debug.Log("GrowthRate: "+this.bacterias[0].getGrowthRate());

                for(int i = 0; i<numMicro; i++){
                    
                    if(this.bacterias[i].enable && this.bacterias[i].getGameObject().transform.position.y < 0){
                        float yTemp = 1f + (float)(bacterias[i].getSize()/2);
                        bacterias[i].renderPosition(yTemp);
                    }
                    
                    List<BacteriaModel> bacteriasTmp = new List<BacteriaModel>();
                    foreach(BacteriaModel bacteria in this.bacterias){
                        if(bacteria.enable && bacteria.getLineage() == i){
                            bacteriasTmp.Add(bacteria);
                        }
                    }

                    /*if(bacteriasTmp.Count > 5){
                        for(int j=bacteriasTmp.Count-1; j>5; j++){
                            this.bacterias.Remove(bacteriasTmp[j]);
                        }
                    }*/
                    
                }


                this.fileContent += "\n";
                File.WriteAllText(".A3DEcol_SizeData.csv", this.fileContent);

                this.time += 1;

                DebugGUI.Graph("graph", (float)(this.bacterias[this.idCamera].getSize()));

                this.changeCameraPosition();
            }else{
                this.running = false;
            }
        }

        
    }

    public BacteriaModel addBacteria(double size, int lineage, float x, float y){
        Debug.Log("add bacteria: "+ lineage.ToString());
        BacteriaModel bacteria  = new BacteriaModel(size, this.bacterias.Count, lineage, x, y, prefab);
        this.bacterias.Add(bacteria);   
        return bacteria;
   

    }
    void changeCameraPosition(){
        float x = this.microtubulos[this.idCamera].transform.position.x + 3;
        float y = this.camera.transform.position.y;
        float z = this.camera.transform.position.z;

        this.camera.transform.position = new Vector3(x, y, z);

        if(this.idCamera != this.idCameraTemp){
            DebugGUI.SetGraphProperties("graph", "Size Graph", 0, 1, 3, new Color( 
                UnityEngine.Random.Range(0f, 1f), 
                UnityEngine.Random.Range(0f, 1f), 
                UnityEngine.Random.Range(0f, 1f)), false);

            this.idCameraTemp = this.idCamera;
        }
        
    }

    private void initializeObjects(){
        for(int i=0; i<numMicro; i++){
            GameObject microTubulosTmp = Instantiate(microtubuloPrefab, new Vector3(x, 0f, 0f), Quaternion.identity);
            microtubulos.Add(microTubulosTmp);
            BacteriaModel motherCell  = new BacteriaModel(0.25, 0, i, x, 1f, prefab);
            this.bacterias.Add(motherCell);
            x += sizeSep;

            dropOptions.Add("Microchannel: "+i.ToString());
        }

        cameraOptions.AddOptions(dropOptions);
    }

}
