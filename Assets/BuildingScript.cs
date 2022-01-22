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
    //public static int age;
    public bool display = false;
    bool generated = false;


    string text;

    static string test = "{\"origin\":[\"hello world!\"]}";
    static string grammarString = @"
    {
    ""name"": [""Dave"", ""Dom"", ""Dan"", ""Apaay"", ""Sedna"", ""Takaani"", ""Rachel"", ""Akna"", ""Ahnah"", ""Alasie"", ""Muktuk"", ""Anjij"", ""Sulak"", ""Sakari"", ""Tutegh"", ""Panu"", ""Pilip"", ""Siku"", ""Rama"", ""Hanta"", ""Anik"", ""Yotimo"", ""Cupun"", ""Imij""],
    ""job"": [""miner"", ""smith"", ""carpenter"", ""council Member"", ""cook"", ""child"", ""hunter"", ""gatherer"", ""snow shoveler""],
    ""quality"": [""incompetent"", ""competent"", ""capable"", ""tired"", ""masterful"", ""begrudging"", ""resourceful"", ""aspiring"", ""apprentice""],
    ""situation"": [""feeling #emotion#."", ""desperately needs to #activity#."", ""just finished #activity.ing# and is now #emotion#.""],
    ""cold"": [""chilly"", ""frozen"", ""nippy"", ""frosty"", ""cold""],
    ""emotion"": [""#cold#"", ""tired"", ""sleepy"", ""energetic"", ""hungry""],
    ""problem"": [""a bad back"", ""frostbite"", ""pneumonia"", ""feeling a bit #emotion#""],
    ""activity"": [""cook"", ""exercise"", ""wash"", ""garden"", ""rest"", ""consume""],
    ""age"": [""6"", ""7"", ""8"", ""9"", ""10"", ""11"", ""12"", ""15"", ""17"", ""18"", ""19"", ""20"", ""24"", ""32"", ""45"", ""54"", ""67"", ""78"", ""82"", ""99""],
    ""namePrefix"": [""elder"", ""brother"", ""sister"", ""youngling"", ""dunce""],
    ""origin"": [""Name: #namePrefix.capitalize# #name#
Age: #age#
Job: #quality.capitalize# #job#
Currently: #situation.capitalize#""]
    }";
    TraceryGrammar grammar = new TraceryGrammar(grammarString);

    private void Awake()
    {
        cb = GameObject.FindGameObjectWithTag("GameController").GetComponent<CityBuilder>();
        cam = cb.cam;
        //age = Random.Range(6, 99);
        
        
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
