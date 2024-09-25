using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG;

namespace TextRPG
{

    public class Item
    {
        protected bool equip;
        public bool Equip { get { return equip; } set { equip = value; } }

        protected ItemID id;
        protected string name;
        protected string valueLabel; //공격력,방어력
        protected float value; // 공격력, 방어력 수치 등등
        protected string info; //아이템 설명
        protected int count; //아이템 수량
        protected int countMax; //최대 아이템 수량
        protected int buyGold; //구매시 아이템 골드량

        public ItemID ID { get { return id; } }
        public string Name { get { return name; } }
        public string ValueLabel { get { return valueLabel; } }
        public float Value { get { return value; } }
        public string Info { get { return info; } }
        public int Count { get { return count; } set { count = value; } }
        public int CountMax { get { return countMax; } }
        public int BuyGold { get { return buyGold; } }

        public Item(ItemID id)
        {
            this.id = id;
            this.equip = false;
            this.name = "_";
            this.valueLabel = "_";
            this.value = 0;
            this.info = "_";
            this.count = 0;
            this.countMax = 0;
            this.buyGold = 0;
        }

        public Item(ItemID id, bool equip, string name , string valueLabel, float value, string info , int count, int countMax, int buyGold)
        {
            this.id = id;
            this.equip = equip;
            this.name = name;
            this.valueLabel = valueLabel;
            this.value = value;
            this.info = info;
            this.count = count;
            this.countMax = countMax;
            this.buyGold = buyGold;
        }
    }

    public class Weapon : Item
    {
        float atkValue; // 공격력 수치
        public float AtkValue { get { return atkValue; } }

        public Weapon(ItemID id) : base(id)
        {
            this.valueLabel = "공격력";
            atkValue = 0;
        }

        public Weapon(ItemID id,bool equip, string name, string valueLabel,float value, string info, int count, int countMax, int buyGold)
        : base(id, equip, name, valueLabel, value, info, count, countMax, buyGold)
        {
            this.valueLabel = "공격력";
            atkValue = value;
        }
    }

    public class Ammor : Item
    {
        float defValue; // 방어력 수치
        public float DefValue { get { return defValue; } }

        public Ammor(ItemID id) : base(id)
        {
            this.valueLabel = "방어력";
            defValue = 0;
        }

        public Ammor(ItemID id,bool equip, string name, string valueLabel,float value, string info, int count, int countMax, int buyGold)
        : base(id, equip, name, valueLabel, value, info, count, countMax, buyGold)
        {
            this.valueLabel = "방어력";
            defValue = value;

        }
    }
}
