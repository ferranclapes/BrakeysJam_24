using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrilsManagerScript : MonoBehaviour
{

    [SerializeField] private List<GirlScript> teamA;
    [SerializeField] private List<GirlScript> teamB;

    private GirlScript girlToHighlight = null;


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
            girl.Initialize(1000, 100, 2f, 2, "TeamA");
        }
        
        foreach(GirlScript girl in teamB){
            girl.Initialize(1000, 100, 2f, 2f, "TeamB");
        }
    }

    public void HighlightCharacter(Vector3 mouse){
        float proximityThreshold = 1f;
        float minDistance = Mathf.Infinity;
        GirlScript girlToHighlightNow = null;
        foreach (GirlScript script in teamA)
        {
            GameObject character = script.gameObject;
            float distance = Vector2.Distance(mouse, character.transform.position);
            if ( distance <= proximityThreshold && distance < minDistance)
            {
                minDistance = distance;
                girlToHighlightNow = script;
            }
        }
        foreach (GirlScript script in teamB)
        {
            GameObject character = script.gameObject;
            float distance = Vector2.Distance(mouse, character.transform.position);
            if ( distance <= proximityThreshold && distance < minDistance)
            {
                minDistance = distance;
                girlToHighlightNow = script;
            }
        }

        if(girlToHighlightNow!= null){
            if(girlToHighlightNow != girlToHighlight  && girlToHighlight != null){
                girlToHighlight.StopHighlight();
            }
            girlToHighlight = girlToHighlightNow;
            girlToHighlight.Highlight();
        }
        else if(girlToHighlight!= null){
            girlToHighlight.StopHighlight();
            girlToHighlight = null;
        }
    }

    public bool CardUsed(string effect, float amount, float agresivity){
        if(girlToHighlight!= null){ 
            girlToHighlight.StopHighlight();
            girlToHighlight.TakeEffect(effect, amount);
            return true;
        }
        return false;
    }

    public bool CardUsed(string effect, float amount, float secondAmount, float agresivity){
        if(girlToHighlight!= null){ 
            girlToHighlight.StopHighlight();
            float value = Random.Range(amount, secondAmount);
            girlToHighlight.TakeEffect(effect, value);
            return true;
        }
        return false;
    } 

}
