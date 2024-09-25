using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using TextRPG;

namespace TextRPG
{
       
    internal class TextRPG
    {
        public static void Main(string[] args)
        {
            GameManager gameManager = new GameManager();
            gameManager.Initialize();

            //게임 무한루프 생성
            while (true)
            {
                gameManager.Update();
            }

        }
    }
}