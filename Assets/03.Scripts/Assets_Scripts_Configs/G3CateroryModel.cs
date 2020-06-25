using System;

namespace Assets.Scripts.Configs
{
    /*
     * this class show model of category of Connect Line Puzzle Game  
     */  
	[Serializable]
	internal class G3CateroryModel
	{
        /// <summary>
        /// id of category
        /// </summary>
		public int ID;
        /// <summary>
        /// difficult of levels
        /// </summary>
		public int Lv;
        /// <summary>
        /// star show difficult of levels
        /// </summary>
		public float Star;
        /// <summary>
        /// total of levels in this category
        /// </summary>
		public int Count;
        /// <summary>
        /// language
        /// </summary>
		public string Lang = "";
	}
}
