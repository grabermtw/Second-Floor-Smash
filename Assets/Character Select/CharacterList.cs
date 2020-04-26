using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterList : MonoBehaviour
{
    // Put these public arrays all here so that we can easily assign in the editor
    public GameObject[] AJShannon;
    public float AJShannonHeightOffset;
    public GameObject[] AmandaOShaughnessy;
    public float AmandaOShaughnessyHeightOffset;
    public GameObject[] AndersJulin;
    public float AndersJulinHeightOffset;
    public GameObject[] BeboHarraz;
    public float BeboHarrazHeightOffset;
    public GameObject[] BrianDeLorenzo;
    public float BrianDeLorenzoHeightOffset;
    public GameObject[] CalvinCrunkleton;
    public float CalvinCrunkletonHeightOffset;
    public GameObject[] ChristinaHuang;
    public float ChristinaHuangHeightOffset;
    public GameObject[] DefaultCharacter;
    public float DefaultCharacterHeightOffset;
    public GameObject[] ElizabethKilpatrick;
    public float ElizabethKilpatrickHeightOffset;
    public GameObject[] EmilyWhittaker;
    public float EmilyWhittakerHeightOffset;
    public GameObject[] EmmaMirizio;
    public float EmmaMirizioHeightOffset;
    public GameObject[] FelixAdams;
    public float FelixAdamsHeightOffset;
    public GameObject[] FloNing;
    public float FloNingHeightOffset;
    public GameObject[] FredDelawie;
    public float FredDelawieHeightOffset;
    public GameObject[] Graber;
    public float GraberHeightOffset;
    public GameObject[] JamesHall;
    public float JamesHallHeightOffset;
    public GameObject[] JasmineLim;
    public float JasmineLimHeightOffset;
    public GameObject[] JesseParreira;
    public float JesseParreiraHeightOffset;
    public GameObject[] Jimbo;
    public float JimboHeightOffset;
    public GameObject[] JohnBall;
    public float JohnBallHeightOffset;
    public GameObject[] JordanWoo;
    public float JordanWooHeightOffset;
    public GameObject[] JoyLondon;
    public float JoyLondonHeightOffset;
    public GameObject[] JudyTram;
    public float JudyTramHeightOffset;
    public GameObject[] JuneXu;
    public float JuneXuHeightOffset;
    public GameObject[] MeghanGraber;
    public float MeghanGraberHeightOffset;
    public GameObject[] MelissaBaum;
    public float MelissaBaumHeightOffset;
    public GameObject[] NateRogers;
    public float NateRogersHeightOffset;
    public GameObject[] OmerBowman;
    public float OmerBowmanHeightOffset;
    public GameObject[] PatrickVorsteg;
    public float PatrickVorstegHeightOffset;
    public GameObject[] QuinnMorris;
    public float QuinnMorrisHeightOffset;
    public GameObject[] Robos;
    public float RobosHeightOffset;
    public GameObject[] SamPolhemus;
    public float SamPolhemusHeightOffset;
    public GameObject[] Testudo;
    public float TestudoHeightOffset;
    public GameObject[] TimHenderson;
    public float TimHendersonHeightOffset;
    public GameObject[] TomZong;
    public float TomZongHeightOffset;
    public GameObject[] Vorsteg;
    public float VorstegHeightOffset;

    public bool DontDestroyOnLoad;

    private Dictionary<string, GameObject[]> characters;
    private Dictionary<string, float> heightOffsets;

    void Awake()
    {
        if (DontDestroyOnLoad)
        {
            // We'd like to keep this consistent throughout the project
            DontDestroyOnLoad(this.gameObject);
        }

        // Put all the above arrays into a the characters dictionary
        characters = new Dictionary<string, GameObject[]>();
        heightOffsets = new Dictionary<string, float>();

        characters.Add("AJ Shannon", AJShannon);
        heightOffsets.Add("AJ Shannon", AJShannonHeightOffset);
        characters.Add("Amanda OShaughnessy", AmandaOShaughnessy);
        heightOffsets.Add("Amanda OShaughnessy", AmandaOShaughnessyHeightOffset);
        characters.Add("Anders Julin", AndersJulin);
        heightOffsets.Add("Anders Julin", AndersJulinHeightOffset);
        characters.Add("Bebo Harraz", BeboHarraz);
        heightOffsets.Add("Bebo Harraz", BeboHarrazHeightOffset);
        characters.Add("Brian DeLorenzo", BrianDeLorenzo);
        heightOffsets.Add("Brian DeLorenzo", BrianDeLorenzoHeightOffset);
        characters.Add("Calvin Crunkleton", CalvinCrunkleton);
        heightOffsets.Add("Calvin Crunkleton", CalvinCrunkletonHeightOffset);
        characters.Add("Christina Huang", ChristinaHuang);
        heightOffsets.Add("Christina Huang", ChristinaHuangHeightOffset);
        characters.Add("DefaultCharacter", DefaultCharacter);
        heightOffsets.Add("DefaultCharacter", DefaultCharacterHeightOffset);
        characters.Add("Elizabeth K", ElizabethKilpatrick);
        heightOffsets.Add("Elizabeth K", ElizabethKilpatrickHeightOffset);
        characters.Add("Emily Whittaker", EmilyWhittaker);
        heightOffsets.Add("Emily Whittaker", EmilyWhittakerHeightOffset);
        characters.Add("Emma Mirizio", EmmaMirizio);
        heightOffsets.Add("Emma Mirizio", EmmaMirizioHeightOffset);
        characters.Add("Felix Adams", FelixAdams);
        heightOffsets.Add("Felix Adams", FelixAdamsHeightOffset);
        characters.Add("Flo Ning", FloNing);
        heightOffsets.Add("Flo Ning", FloNingHeightOffset);
        characters.Add("Fred Delawie", FredDelawie);
        heightOffsets.Add("Fred Delawie", FredDelawieHeightOffset);
        characters.Add("Graber", Graber);
        heightOffsets.Add("Graber", GraberHeightOffset);
        characters.Add("James Hall", JamesHall);
        heightOffsets.Add("James Hall", JamesHallHeightOffset);
        characters.Add("Jasmine Lim", JasmineLim);
        heightOffsets.Add("Jasmine Lim", JasmineLimHeightOffset);
        characters.Add("Jesse Parreira", JesseParreira);
        heightOffsets.Add("Jesse Parreira", JesseParreiraHeightOffset);
        characters.Add("Jimbo", Jimbo);
        heightOffsets.Add("Jimbo", JimboHeightOffset);
        characters.Add("John Ball", JohnBall);
        heightOffsets.Add("John Ball", JohnBallHeightOffset);
        characters.Add("Jordan Woo", JordanWoo);
        heightOffsets.Add("Jordan Woo", JordanWooHeightOffset);
        characters.Add("Joy London", JoyLondon);
        heightOffsets.Add("Joy London", JoyLondonHeightOffset);
        characters.Add("Judy Tram", JudyTram);
        heightOffsets.Add("Judy Tram", JudyTramHeightOffset);
        characters.Add("June Xu", JuneXu);
        heightOffsets.Add("June Xu", JuneXuHeightOffset);
        characters.Add("Meghan Graber", MeghanGraber);
        heightOffsets.Add("Meghan Graber", MeghanGraberHeightOffset);
        characters.Add("Melissa Baum", MelissaBaum);
        heightOffsets.Add("Melissa Baum", MelissaBaumHeightOffset);
        characters.Add("Nate Rogers", NateRogers);
        heightOffsets.Add("Nate Rogers", NateRogersHeightOffset);
        characters.Add("Omer Bowman", OmerBowman);
        heightOffsets.Add("Omer Bowman", OmerBowmanHeightOffset);
        characters.Add("Patrick Vorsteg", PatrickVorsteg);
        heightOffsets.Add("Patrick Vorsteg", PatrickVorstegHeightOffset);
        characters.Add("Quinn Morris", QuinnMorris);
        heightOffsets.Add("Quinn Morris", QuinnMorrisHeightOffset);
        characters.Add("VorstegBot", Robos);
        heightOffsets.Add("VorstegBot", RobosHeightOffset);
        characters.Add("Sam Polhemus", SamPolhemus);
        heightOffsets.Add("Sam Polhemus", SamPolhemusHeightOffset);
        characters.Add("Testudo", Testudo);
        heightOffsets.Add("Testudo", TestudoHeightOffset);
        characters.Add("Tim Henderson", TimHenderson);
        heightOffsets.Add("Tim Henderson", TimHendersonHeightOffset);
        characters.Add("Tom Zong", TomZong);
        heightOffsets.Add("Tom Zong", TomZongHeightOffset);
        characters.Add("Vorsteg", Vorsteg);
        heightOffsets.Add("Vorsteg", VorstegHeightOffset);
    }

    // Returns the array of skins for a specific character
    public GameObject[] GetCharacterArray(string characterName)
    {
        return characters[characterName];
    }

    // Returns the height offset for the specific character (this is for the character selection screen)
    public float GetHeightOffset(string characterName)
    {
        return heightOffsets[characterName];
    }

    // Returns a list of all the arrays of skins for each character
    public List<GameObject[]> GetAllCharactersList()
    {
        List<GameObject[]> charList = new List<GameObject[]>();
        foreach (KeyValuePair<string, GameObject[]> p in characters)
        {
            if(p.Key != "Elizabeth K" && p.Key != "Meghan Graber" && p.Key != "Brian DeLorenzo") // temporarily
            {
                charList.Add(p.Value);
            }
        }
        return charList;
    }
}
