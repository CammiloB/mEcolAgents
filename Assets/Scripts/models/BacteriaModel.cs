using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BacteriaModel : MonoBehaviour
{
    public bool enable = true;
    private double size;
    private int id;
    private int lineage;
    private double growthRate;
    private float splitRate;
    private float nextTimeDivision = 0;
    private int divisionSteps;

    private float x;
    private float y;

    private float divisionSize;
    public double initialSize;

    private double divisionStep;
    private double totalDivisionSteps = 10.0;

    private double meanSize = 0.2;
    private double time;

    private double tau;

    private double deltaTime = Time.fixedDeltaTime;

    public AmbienteScript ambiente;

    private GameObject bacteriaPrefab;
    private GameObject prefab;
    private Rigidbody rb;


    public BacteriaModel(double size, int id, int lineage, float x, float y, GameObject prefab)
    {
        this.size = size;
        this.lineage = lineage;
        this.initialSize = this.size;
        this.growthRate = Math.Log(2) / 10;
        this.divisionStep = 0;

        this.prefab = prefab;

        this.time = 0;
        this.x = x;
        this.y = y;

        this.setNextTimeDivision();
        this.tau = this.nextTimeDivision;

        bacteriaPrefab = Instantiate(prefab, new Vector3(x, y, 0f), Quaternion.identity);
        this.rb = this.bacteriaPrefab.GetComponent<Rigidbody>();
        this.rb.velocity = Vector3.zero;
        this.rb.angularVelocity = Vector3.zero;
        //bacteriaPrefab.transform.localScale = new Vector3(0.5f, (float) size, 0.5f);


        ambiente = FindObjectOfType<AmbienteScript>();

        //this.setDivisionSize();
        //this.setGrowthRate();



    }

    void Start()
    {

    }

    void Update()
    {

    }

    public GameObject getPrefab()
    {
        return this.bacteriaPrefab;
    }

    public double getSize()
    {
        return this.size;
    }

    public void setSize(double size)
    {
        this.size = size;
    }

    public int getId()
    {
        return this.id;
    }

    public void setId(int id)
    {
        this.id = id;
    }

    public int getLineage()
    {
        return this.lineage;
    }

    public void setLineage(int lineage)
    {
        this.lineage = lineage;
    }

    public double getGrowthRate()
    {
        return this.growthRate;
    }

    public float getSplitRate()
    {
        return this.splitRate;
    }

    public void setSplitRate(float splitRate)
    {
        this.splitRate = splitRate;
    }

    public float getNextTimeDivision()
    {
        return this.nextTimeDivision;
    }

    public void setNextTimeDivision()
    {

        System.Random random = new System.Random();
        double u1 = random.NextDouble();
        double operation = (1 / this.growthRate) * Math.Log((1 - (this.meanSize / (this.size * this.totalDivisionSteps)) * Math.Log(u1)));
        this.nextTimeDivision = (float)operation;

    }

    public void growth(double t)
    {

        this.tau -= t;
        this.size = this.size * Math.Exp((this.growthRate) * t);
        //Debug.Log("Size: " + this.size);
    }

    public void division()
    {
        this.divisionStep = 0;
        //double temp = Time.fixedDeltaTime - this.tau;
        double temp = deltaTime - this.tau;

        float yTemp = this.bacteriaPrefab.transform.position.y;
        //this.size = this.size * Math.Exp((this.growthRate) * this.tau);
        this.size = this.size / 2;
        setNextTimeDivision();


        float x = this.bacteriaPrefab.transform.position.x;
        float z = this.bacteriaPrefab.transform.position.z;


        float yMot = (float)(yTemp - (this.size/2)) + 0.1f;
        float ySon = (float)(yTemp + (this.size/2)) + 0.1f;

        BacteriaModel bacteria = ambiente.addBacteria(this.size, this.lineage, this.x, ySon);
        this.bacteriaPrefab.transform.position = new Vector3(x, yMot, z);

        bacteria.setNextTimeDivision();
        bacteria.tau = this.nextTimeDivision;
        bacteria.size = this.size * Math.Exp((this.growthRate) * temp);
        bacteria.tau -= temp;

        //this.tau = this.nextTimeDivision;
        //this.size = this.size * Math.Exp((this.growthRate) * temp);

        //this.tau -= temp;


    }

    public void simulate()
    {
        //double deltaTime = Time.deltaTime;
        if (this.bacteriaPrefab.transform.position.y > 5.5)
        {
            Destroy(this.bacteriaPrefab);
            this.enable = false;
        }

        if (this.tau < deltaTime)
        {
            //double tt = deltaTime;
            //while (this.tau < tt)
            //{
                this.divisionStep += 1;
                growth(this.tau);
                if (this.divisionStep >= this.totalDivisionSteps)
                {
                    division();
                }
                
                //tt += -this.tau;
                this.setNextTimeDivision();
                this.tau = this.nextTimeDivision;
            //}

        }else
        {
            growth(deltaTime);

        }
        Debug.Log("Lineage: "+this.lineage+", size: "+this.size);
        this.render();

    }

    public void render()
    {
        this.bacteriaPrefab.transform.localScale = new Vector3(0.5f, (float)this.size * 2.5f, 0.5f);

        /*if(this.bacteriaPrefab.transform.position.y > 5.5){
            Destroy(this.bacteriaPrefab);
            this.enable = false;
        }*/

    }

    public string getData()
    {
        return this.size + ";" + this.lineage;
    }

    public void renderPosition(float y)
    {
        float x = this.bacteriaPrefab.transform.position.x;
        float z = this.bacteriaPrefab.transform.position.z;

        this.bacteriaPrefab.transform.position = new Vector3(x, y, z);
    }

    public float getYPosition(){
        return this.bacteriaPrefab.transform.position.y;
    }

    public GameObject getGameObject(){
        return this.bacteriaPrefab;
    }

}
