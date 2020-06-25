using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LanguageTxt
{
    public List<Text> texts = new List<Text>();

}
public class LanguageManager : MonoBehaviour
{

    public Font Korean_font;
    public Font Defalt_font;

    private Font baseFont;

    public static LanguageManager Instance;

    public List<Dictionary<string, object>> language_data;      //튜토리얼 블럭 정보

    string str_language = "";

    
    [SerializeField]
    public List<LanguageTxt> texts = new List<LanguageTxt>();

    private void Awake()
    {
        Instance = this;

        language_data = CSVReader.Read("language");


    }

    public void Get_Language()
    {
        if (DataManager.Instance.state_Player.LocalData_LanguageId == -1)
            Change_language(Application.systemLanguage);
        else
            Change_language((SystemLanguage)DataManager.Instance.state_Player.LocalData_LanguageId);

    }

    public void Set_Language_Txt()
    {
        EditorStyleSet();
        int day = 1;
        Debug.Log(texts.Count +"   " + language_data.Count);
        for (int i = 0; i < texts.Count; i++)
            foreach (var item in texts[i].texts)
                if ((string)language_data[i][str_language] != "")
                {
                    if (item.name.Contains("Txt_daily_1"))
                    {
                        item.text = day + (string)language_data[i][str_language];
                        day += 1;
                    }
                    else
                    {
                     
                        item.text = (string)language_data[i][str_language];

                    }

                }

    }

    public void Change_language(SystemLanguage sl)
    {

        switch (sl)
        {
            case UnityEngine.SystemLanguage.Afrikaans:
            case UnityEngine.SystemLanguage.Arabic:
            case UnityEngine.SystemLanguage.Basque:
            case UnityEngine.SystemLanguage.Belarusian:
            case UnityEngine.SystemLanguage.Bulgarian:
            case UnityEngine.SystemLanguage.Catalan:
            case UnityEngine.SystemLanguage.Czech:
            case UnityEngine.SystemLanguage.Danish:
            case UnityEngine.SystemLanguage.Dutch:
            case UnityEngine.SystemLanguage.Estonian:
            case UnityEngine.SystemLanguage.Faroese:
            case UnityEngine.SystemLanguage.Finnish:
            case UnityEngine.SystemLanguage.English:
            case UnityEngine.SystemLanguage.Greek:
            case UnityEngine.SystemLanguage.Hebrew:
            case UnityEngine.SystemLanguage.Hungarian:
            case UnityEngine.SystemLanguage.Icelandic:
            case UnityEngine.SystemLanguage.Latvian:
            case UnityEngine.SystemLanguage.Lithuanian:
            case UnityEngine.SystemLanguage.Norwegian:
            case UnityEngine.SystemLanguage.Polish:
            case UnityEngine.SystemLanguage.Romanian:
            case UnityEngine.SystemLanguage.SerboCroatian:
            case UnityEngine.SystemLanguage.Slovak:
            case UnityEngine.SystemLanguage.Slovenian:
            case UnityEngine.SystemLanguage.Swedish:
            case UnityEngine.SystemLanguage.Ukrainian:
            case UnityEngine.SystemLanguage.Unknown:
                str_language = "English";
                baseFont = Korean_font;
                break;
            case UnityEngine.SystemLanguage.Thai:
                str_language = "Thai";
                baseFont = Defalt_font;

                break;
            case UnityEngine.SystemLanguage.Italian:
                str_language = "Italian";
                baseFont = Defalt_font;

                break;
            case UnityEngine.SystemLanguage.Turkish:
                str_language = "Turkish";
                baseFont = Defalt_font;

                break;

            case UnityEngine.SystemLanguage.French:
                str_language = "French";
                baseFont = Defalt_font;

                break;
            case UnityEngine.SystemLanguage.German:
                str_language = "German";
                baseFont = Defalt_font;

                break;
            case UnityEngine.SystemLanguage.Indonesian:
                str_language = "Indonesian";
                baseFont = Defalt_font;

                break;
            case UnityEngine.SystemLanguage.Japanese:
                str_language = "Japanese";
                baseFont = Korean_font;
                break;
            case UnityEngine.SystemLanguage.Korean:
                str_language = "Korean";
                baseFont = Korean_font;
                break;
            case UnityEngine.SystemLanguage.Portuguese:
                str_language = "Portuguese";
                baseFont = Defalt_font;

                break;
            case UnityEngine.SystemLanguage.Russian:
                str_language = "Russian";
                baseFont = Defalt_font;

                break;
            case UnityEngine.SystemLanguage.Spanish:
                str_language = "Spanish";
                baseFont = Defalt_font;

                break;
            case UnityEngine.SystemLanguage.Vietnamese:
                str_language = "Vietnamese";
                baseFont = Defalt_font;

                break;
            case UnityEngine.SystemLanguage.ChineseSimplified:
            case UnityEngine.SystemLanguage.Chinese:
                str_language = "Chinese_Sim";
                baseFont = Defalt_font;

                break;
            case UnityEngine.SystemLanguage.ChineseTraditional:
                str_language = "Chinese_Tra";
                baseFont = Defalt_font;

                break;
            default:
                str_language = "Hindi";
                baseFont = Defalt_font;

                break;
        }

        DataManager.Instance.state_Player.LocalData_LanguageId = (int)sl;
        DataManager.Instance.Save_Player_Data();

        Set_Language_Txt();


    }

    void EditorStyleSet()
    {
        var textComponents = GameObject.Find("Canvas").GetComponentsInChildren<Text>(true);
        foreach (var component in textComponents)
        {
            if(component.font.name != "score" && component.font.name != "combo")
                component.font = baseFont;
        }
            
    }

}
