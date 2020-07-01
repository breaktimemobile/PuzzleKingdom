using Assets.Scripts.Configs;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.Scripts.Utils
{
	internal class Language
	{
		private SystemLanguage m_id = SystemLanguage.English;

		private Font m_fonts;

		private static Language m_instance;

		private Action m_transformHandle;

        //private event Action m_transformHandle;


        public SystemLanguage Id
		{
			get
			{
				return this.m_id;
			}
			set
			{
				this.m_id = value;
			}
		}

        public Language()
		{
			this.Init();
		}

		public static Language GetInstance()
		{
			if (Language.m_instance == null)
			{
				Language.m_instance = new Language();
			}
			return Language.m_instance;
		}

		public static string GetText(string id)
		{
			if (!Configs.Configs.TLanguages.ContainsKey(id))
			{
				return "ERROR NO " + id;
			}
			TLanguage tLanguage = Configs.Configs.TLanguages[id];
			SystemLanguage id2 = Language.GetInstance().Id;
			if (id2 <= SystemLanguage.Japanese)
			{
				if (id2 != SystemLanguage.Chinese)
				{
					if (id2 == SystemLanguage.Indonesian)
					{
						return tLanguage.L_ID;
					}
					if (id2 != SystemLanguage.Japanese)
					{
						goto IL_8C;
					}
					return tLanguage.L_JP;
				}
			}
			else if (id2 <= SystemLanguage.Russian)
			{
				if (id2 == SystemLanguage.Portuguese)
				{
					return tLanguage.L_PT;
				}
				if (id2 != SystemLanguage.Russian)
				{
					goto IL_8C;
				}
				return tLanguage.L_RU;
			}
			else if (id2 != SystemLanguage.ChineseSimplified)
			{
				if (id2 != SystemLanguage.ChineseTraditional)
				{
					goto IL_8C;
				}
				return tLanguage.L_ZH_CN;
			}
			return tLanguage.L_ZH_CN;
			IL_8C:
			return tLanguage.L_EN;
		}

		public Font GetFont()
		{
            if (m_fonts == null)
            {
                this.m_fonts = this.LoadFont();

            }

            //Debug.Log(m_instance.m_fonts.name);
            return m_instance.m_fonts;
		}

		public void Set(SystemLanguage id)
		{
			this.Id = id;
			this.m_fonts = this.LoadFont();
            DataManager.Instance.state_Player.LocalData_LanguageId = (int)this.Id;
            DataManager.Instance.Save_Player_Data();
			Action expr_29 = this.m_transformHandle;
			if (expr_29 == null)
			{
				return;
			}
			expr_29();
		}

		public void AddEvent(Action e)
		{
			this.m_transformHandle += e;
		}

		public void RemoveEvent(Action e)
		{
			this.m_transformHandle -= e;
		}


        private void Init()
		{
            if (DataManager.Instance.state_Player.LocalData_LanguageId == -1)
            {
                this.Id = Application.systemLanguage;

            }
            else
            {
                this.Id = (SystemLanguage)DataManager.Instance.state_Player.LocalData_LanguageId;

            }

            this.m_fonts = this.LoadFont();
		}

		private Font LoadFont()
		{

            SystemLanguage id = Application.systemLanguage;

            if (DataManager.Instance.state_Player.LocalData_LanguageId != -1)
            {
                id = (SystemLanguage)DataManager.Instance.state_Player.LocalData_LanguageId;

            }

			string path;
			if (id == SystemLanguage.Korean)
			{
				path = "font/BMJUA_ttf";
			}
			else
			{
				path = "font/ARLRDBD";
			}

            return Resources.Load(path) as Font;
		}

        public static string Change_language(SystemLanguage sl)
        {
            switch (sl)
            {
                case UnityEngine.SystemLanguage.Afrikaans:
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
                    return "English";
                case UnityEngine.SystemLanguage.Thai:

                    return "Thai";
                case UnityEngine.SystemLanguage.Italian:

                    return "Italian";
                case UnityEngine.SystemLanguage.Turkish:

                    return "Turkish";

                case UnityEngine.SystemLanguage.French:

                    return "French";
                case UnityEngine.SystemLanguage.German:
    
                    return "German";
                case UnityEngine.SystemLanguage.Indonesian:
     
                    return "Indonesian";
                case UnityEngine.SystemLanguage.Japanese:
  
                    return "Japanese";
                case UnityEngine.SystemLanguage.Korean:

                    return "Korean";
                case UnityEngine.SystemLanguage.Portuguese:

                    return "Portuguese";
                case UnityEngine.SystemLanguage.Russian:

                    return "Russian";
                case UnityEngine.SystemLanguage.Spanish:

                    return "Spanish";
                case UnityEngine.SystemLanguage.Vietnamese:

                    return "Vietnamese";
                case UnityEngine.SystemLanguage.ChineseSimplified:
                case UnityEngine.SystemLanguage.Chinese:

                    return "Chinese_Sim";
                case UnityEngine.SystemLanguage.ChineseTraditional:

                    return "Chinese_Tra";
                case UnityEngine.SystemLanguage.Arabic:
                    return "Arabic";
                case UnityEngine.SystemLanguage.Unknown:

                    return "Hindi";
                default:
                    return "English";
            }

        }

        public static string GetText(string m_id, SystemLanguage @int)
        {

            Dictionary<string, object> data = DataManager.Instance.language_data.Find(x => x["key"].Equals(m_id));

            if(data == null)
            {
                //Debug.Log("<color=yellow>None</color>");

                return null;

            }
            else
            {
                //Debug.Log("<color=red>" + data[Change_language(@int)].ToString() + "</color>");

                string[] str = data[Change_language(@int)].ToString().Split('^');

                string str_name = str[0];

                if (str.Length > 0)
                {
                    for (int i = 1; i < str.Length; i++)
                    {
                        str_name += "\n" + str[i];
                    }

                }
              
                    
                return str_name;

            }

        }
    }
}
