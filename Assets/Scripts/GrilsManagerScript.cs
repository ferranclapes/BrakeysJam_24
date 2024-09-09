using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrilsManagerScript : MonoBehaviour
{

    [SerializeField] private List<GirlScript> teamA;
    [SerializeField] private List<GirlScript> teamB;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            InitializeGirls();
        }
    }

    private void InitializeGirls(){
        foreach(GirlScript girl in teamA){
            girl.Initialize(10, 1, 0.3f, 5, "TeamA");
        }
        
        foreach(GirlScript girl in teamB){
            girl.Initialize(8, 2, 0.4f, 3.5f, "TeamB");
        }
    }
}
