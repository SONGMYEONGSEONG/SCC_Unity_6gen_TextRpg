using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG;

namespace TextRPG
{
    public enum SceneType
    {
        ConsoleOff = -1,
        Title = 0,
        Status = 1,
        Inventory = 2,
        Equipment = 3,
        Shop = 4,
        Dungeon = 5,
        Rest = 6,

        SceneEnd
    }

    public enum ItemID
    {
        None = 0,
        MetalAmmor = 1, // 무쇠 갑옷
        SpartaSpear = 2, // 스파르타의 창
        OldSword = 3, // 낡은 검
        BiggnerAmmor = 4, // 수련자 갑옷
        BronzeAxe = 5, // 청동도끼
        SpartaAmmor =6,// 스파르타의 갑옷
        JJANGSword = 7,// 짱쎈소드
        JJANGAmmor = 8,// 짱쎈갑옷

        EndID
    }

    public struct StatusData
    {
        public int level;
        public int clearCount;
        public string name;
        public string job; //enum 화 해야됨
        public float atk;
        public int def;
        public int health;
        public int healthMax;
        public int gold;

        public StatusData(int level, string name, string job, float atk, int def, int health, int healthMax, int gold)
        {
            this.level = level;
            clearCount = this.level;
            this.name = name;
            this.job = job;
            this.atk = atk;
            this.def = def;
            this.health = health;
            this.healthMax = healthMax;
            this.gold = gold;
        }

        public bool LevelCheck()
        {
            if (clearCount >= level)
            {
                level += 1; //레벨업
                atk += 0.5f;
                def += 1;
                clearCount = 0;
                return true;
            }
            return false;
        }
    }

    
    public struct Dungeon
    {
        public string name;
        public int accessDefCondition; //던전 입장 권장 조건 (방어력)
        
        public int clearGold;//클리어 보상
        //1000 / 1700 / 2500 난이도 순

        public Dungeon(string name,int accessDefCondition, int clearGold)
        {
            this.name = name;
            this.accessDefCondition = accessDefCondition;
            this.clearGold = clearGold;
        }
    }

    public static class StrBuild
    {
        public static StringBuilder strBuilder = new StringBuilder();

        public static void Print(string str)
        {
            strBuilder.Clear();
            strBuilder.Append(str);

            Console.WriteLine(strBuilder.ToString());
        }

        public static void StatusPrint(StatusData status, float sumAtk, float sumDef)
        {
            strBuilder.Clear();
            strBuilder.AppendFormat("\nLv. {0}\n", status.level);
            strBuilder.AppendFormat("{0} ( {1} )\n", status.name, status.job);

            if(sumAtk > 0)
                strBuilder.AppendFormat("공격력 : {0} ( + {1} )\n", status.atk, sumAtk);
            else
                strBuilder.AppendFormat("공격력 : {0}\n", status.atk);

            if (sumDef > 0)
                strBuilder.AppendFormat("방어력 : {0} ( + {1} )\n", status.def,sumDef);
            else
                strBuilder.AppendFormat("방어력 : {0}\n", status.def);

            strBuilder.AppendFormat("체  력 : {0} / {1}\n", status.health, status.healthMax);
            strBuilder.AppendFormat("Gold : {0} G\n\n", status.gold);

            Console.Write(strBuilder.ToString());

        }
    }
}
