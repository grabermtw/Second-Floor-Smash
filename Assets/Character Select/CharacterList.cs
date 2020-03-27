using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterList : MonoBehaviour
{
    // Put these public arrays all here so that we can easily assign in the editor
    public GameObject[] AJShannon;
    public GameObject[] AmandaOShaughnessy;
    public GameObject[] AndersJulin;
    public GameObject[] BeboHarraz;
    public GameObject[] CalvinCrunkleton;
    public GameObject[] ChristinaHuang;
    public GameObject[] DefaultCharacter;
    public GameObject[] EmilyWhittaker;
    public GameObject[] EmmaMirizio;
    public GameObject[] FelixAdams;
    public GameObject[] FloNing;
    public GameObject[] FredDelawie;
    public GameObject[] Graber;
    public GameObject[] JamesHall;
    public GameObject[] JasmineLim;
    public GameObject[] JesseParreira;
    public GameObject[] Jimbo;
    public GameObject[] JohnBall;
    public GameObject[] JordanWoo;
    public GameObject[] JoyLondon;
    public GameObject[] JudyTram;
    public GameObject[] JuneXu;
    public GameObject[] MelissaBaum;
    public GameObject[] NateRogers;
    public GameObject[] OmerBowman;
    public GameObject[] PatrickVorsteg;
    public GameObject[] QuinnMorris;
    public GameObject[] Robos;
    public GameObject[] Testudo;
    public GameObject[] TimHenderson;
    public GameObject[] TomZong;
    public GameObject[] Vorsteg;
    
    private Dictionary<string, GameObject[]> characters;

    void Awake()
    {
        // We'd like to keep this consistent throughout the project
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Put all the above arrays into a the characters dictionary
        characters = new Dictionary<string, GameObject[]>();

        characters.Add("AJ Shannon", AJShannon);
        characters.Add("Amanda OShaughnessy", AmandaOShaughnessy);
        characters.Add("Anders Julin", AndersJulin);
        characters.Add("Bebo Harraz", BeboHarraz);
        characters.Add("Calvin Crunkleton", CalvinCrunkleton);
        characters.Add("Christina Huang", ChristinaHuang);
        characters.Add("DefaultCharacter", DefaultCharacter);
        characters.Add("Emily Whittaker", EmilyWhittaker);
        characters.Add("Emma Mirizio", EmmaMirizio);
        characters.Add("Felix Adams", FelixAdams);
        characters.Add("Flo Ning", FloNing);
        characters.Add("Fred Delawie", FredDelawie);
        characters.Add("Graber", Graber);
        characters.Add("James Hall", JamesHall);
        characters.Add("Jasmine Lim", JasmineLim);
        characters.Add("Jesse Parreira", JesseParreira);
        characters.Add("Jimbo", Jimbo);
        characters.Add("John Ball", JohnBall);
        characters.Add("Jordan Woo", JordanWoo);
        characters.Add("Joy London", JoyLondon);
        characters.Add("Judy Tram", JudyTram);
        characters.Add("June Xu", JuneXu);
        characters.Add("Melissa Baum", MelissaBaum);
        characters.Add("Nate Rogers", NateRogers);
        characters.Add("Omer Bowman", OmerBowman);
        characters.Add("Patrick Vorsteg", PatrickVorsteg);
        characters.Add("Quinn Morris", QuinnMorris);
        characters.Add("VorstegBot", Robos);
        characters.Add("Testudo", Testudo);
        characters.Add("Tim Henderson", TimHenderson);
        characters.Add("Tom Zong", TomZong);
        characters.Add("Vorsteg", Vorsteg);
    }

    public GameObject[] GetCharacterArray(string characterName)
    {
        return characters[characterName];
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
