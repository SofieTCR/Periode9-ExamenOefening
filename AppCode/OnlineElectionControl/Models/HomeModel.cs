namespace OnlineElectionControl.Models
{
    public class HomeModel
    {
        /// <summary>
        /// A method to return a string array of the provided string, split on the letter t
        /// </summary>
        /// <param name="splitString">The string to be split</param>
        /// <returns></returns>
        public string[] SplitOnT(string splitString)
        {
            return splitString.Split('t');
        }
    }
}
