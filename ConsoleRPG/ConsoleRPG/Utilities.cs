using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace ConsoleRPG
{
    internal static class Utilities
    {
        static readonly Random random = new Random();

        /// <summary>
        /// Draws a char-symbol at given position
        /// </summary>
        /// <param name="anXPosition"></param>
        /// <param name="aYPosition"></param>
        /// <param name="aSymbol"></param>
        public static void Draw(int anXPosition, int aYPosition, char aSymbol)
        {
            CursorPosition(anXPosition, aYPosition);

            Console.Write(aSymbol);
            CursorPosition();
        }

        /// <summary>
        /// Return a random int between 1 and given maximum
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int GetRandom(int max)
        {
            int result;
            result = random.Next(1, max + 1);
            return result;
        }

        /// <summary>
        /// Takes in a standard dice notation as a string and returns result of rolls as an int.
        /// Example of standard dice notation: 2d6+10--
        /// 2rolls, 6 faces and then ads 10 to final result
        /// </summary>
        /// <param name="aDiceType"></param>
        /// <returns></returns>
        public static int RollDices(string aDiceType)
        {
            string pattern = @"(\d{0,3})d([468]|10|20)(\s|([-+])(\d{1,2}))?";
            int numberOfRolls = 1;
            int facesOnDice = 0;
            int followNumber = 0;
            int result = 0;

            MatchCollection matches = Regex.Matches(aDiceType, pattern);
            foreach (Match match in matches)
            {
                GroupCollection data = match.Groups;

                int.TryParse(data[1].Value, out numberOfRolls);
                int.TryParse(data[2].Value, out facesOnDice);
                int.TryParse(data[5].Value, out followNumber);
                if (data[4].Value == "-")
                {
                    followNumber = followNumber - (2 * followNumber);
                }
            }
            if (numberOfRolls == 0)
            {
                numberOfRolls = 1;
            }
            for (int i = 1; i <= numberOfRolls; i++)
            {
                result += GetRandom(facesOnDice);
            }

            result += followNumber;


            return result;
        }

        public static int Abs(int aValue)
        {
            if (aValue >= 0)
            {
                return aValue;
            }
            else
            {
                return -aValue;
            }

        }

        public static int Clamp(int aValue, int aMinimumValue = int.MinValue, int aMaximumValue = int.MaxValue)
        {
            if (aValue < aMinimumValue)
            {
                return aMinimumValue;
            }
            else if (aValue > aMaximumValue)
            {
                return aMaximumValue;
            }
            return aValue;

        }

        /// <summary>
        /// Sets cursor at given position. Default is 0,0
        /// </summary>
        /// <param name="anXPosition"></param>
        /// <param name="aYPosition"></param>
        public static void CursorPosition(int anXPosition = 0, int aYPosition = 0)
        {
            Console.SetCursorPosition(anXPosition, aYPosition);
        }


        /// <summary>
        /// Sets cursor at position given by a Vector2 for X and Y
        /// </summary>
        /// <param name="aScreenPosition"></param>
        public static void Cursor(Vector2 aScreenPosition)
        {
            Console.SetCursorPosition(aScreenPosition.X, aScreenPosition.Y);
        }

        public static char[,] ReadFromFile(string aMapPath, out string aTitle)
        {
            aTitle = "";
            char[,] map;
            int width = 0;
            int heigth = 0;
            try
            {
                using (StreamReader sr = new StreamReader(aMapPath))
                {
                    aTitle = sr.ReadLine();
                    string size = sr.ReadLine();
                    string pattern = @"(\d{1,3})[xX](\d{1,3})";
                    string xWidth = "";
                    string yHeigth = "";

                    MatchCollection matches = Regex.Matches(size, pattern);

                    foreach (Match match in matches)
                    {
                        GroupCollection data = match.Groups;
                        xWidth = data[1].Value;
                        yHeigth = data[2].Value;
                    }

                    width = int.Parse(xWidth);
                    heigth = int.Parse(yHeigth);
                    map = new char[width, heigth];

                    List<string> lines = new List<string>();
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                    for (int y = 0; y < heigth; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            if (x >= lines[y].Length)
                            {
                                map[x, y] = ' ';
                                continue;
                            }
                            else
                             {
                                map[x, y] = lines[y][x];
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not read file");
                Console.WriteLine(e.Message);
                map = new char[width, heigth];
            }

            return map;
        }

        static public Dialogue GetDialogue(string aDialoguePath)
        {
            string title = "";
            string idTemp = "0";
            int id;
            List<string> lines = new List<string>();

            try
            {
                using (StreamReader sr = new StreamReader(aDialoguePath))
                {
                    title = sr.ReadLine();
                    idTemp = sr.ReadLine();

                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not read file");
                Console.WriteLine(e.Message);
            }
            id = int.TryParse(idTemp,out id) ? id : 0;
            return new Dialogue(title, id, lines);
        }

        public static void PressEnterToContinue()
        {
            Console.Write("Press \'Enter\' to continue");
            Console.ReadLine();
        }

    }

    enum DoorDirections
    {
        West,
        North,
        East,
        South
    }

    enum Actors
    {
        Player,
        Bat,
        Spider,
        Dragon,
        EvilLord
    }

    enum Actions
    {
        Attack,
        Magic,
        Defend
    }
    
    enum SpellType
    {
        LightningBolt,
        Fireball,
        Iceblast
    }

    enum FrameType
    {
        Frame,
        ActionFrame,
        SpellFrame,
        EnemyNameFrame
    }

    enum SoundType
    {
        EnemyHurt,
        PlayerHurt,
        BattleStart,
        Heal,
        LighningHurt,
        BattleVictory,
        MansionAmbience,
        VillageAmbience,
        PortalCast,
        OpenChest,
        GetKey
    }

    enum Quest
    {

    }
}
