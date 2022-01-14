using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityTracery;

public class BuildingScript : MonoBehaviour
{
    CityBuilder cb;
    Camera cam;
    public int colour;
    public bool display = false;
    bool generated = false;


    string text;

    static string test = "{\"origin\":[\"hello world!\"]}";
    static string grammarString = @"
    {
    ""origin"":[""#namePrefix.capitalize# #name# #situation#""],
    ""name"": [""Dave"", ""Dom"", ""Dan""],
    ""situation"":[""was feeling #emotion#."", ""desperately needs to #activity#."", ""has just #activity.ed# and is now #emotion#.""],
    ""cold"":[""chilly"", ""frozen"", ""nippy"", ""frosty"", ""cold""],
    ""emotion"":[""#cold#"", ""tired"", ""sleepy"", ""energetic"", ""hungry""],
    ""activity"":[""cook"", ""exercise"", ""wash"", ""garden"", ""rest"", ""consume""],
    ""namePrefix"":[""elder"", ""brother"", ""sister"", ""youngling"", ""dunce""],
    ""origin"":[""#namePrefix.capitalize# #name# #situation#
    <p></p>
    they live with #name#, who is feeling #emotion#.""]
    }";
    TraceryGrammar grammar = new TraceryGrammar(grammarString);

    private void Awake()
    {
        cb = GameObject.FindGameObjectWithTag("GameController").GetComponent<CityBuilder>();
        cam = cb.cam;
    }

    void OnMouseOver()
    {
        cb.timer = 10;
        if (generated)
        {
            // If we've already generated residents, display them:
            Debug.Log(text);
            cb.text.text = text;
            cb.uiPanel.SetActive(true);
            cb.uiPanel.transform.position = cam.WorldToScreenPoint(this.transform.position);
        }
        else
        {
            // Else, generate a sentence from the grammar and save it:
            generated = true;
            text = grammar.Generate();
        }
    }




}
