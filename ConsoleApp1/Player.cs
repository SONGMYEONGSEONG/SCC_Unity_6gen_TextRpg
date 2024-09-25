using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG;

namespace TextRPG
{
    public class Player
    {
        StatusData status;
        public StatusData Statue { get { return status; } set { status = value; } }

        //보유중인 아이템 확인
        Dictionary<ItemID, Item> items;
        public Dictionary<ItemID, Item> Items { get { return items; } }

        Weapon equipWeapon;
        Ammor equipAmmor;

        public Weapon EquipWeapon { get { return equipWeapon; } set { equipWeapon = value ; } }
        public Ammor EquipAmmor { get { return equipAmmor; } set { equipAmmor = value ; } }

        public Player()
        {
            items = new Dictionary<ItemID, Item>();
            equipWeapon = new Weapon(ItemID.None);
            equipAmmor = new Ammor(ItemID.None);
        }

        public void PlayerReset()
        {
            items.Clear();
            equipWeapon = new Weapon(ItemID.None);
            equipAmmor = new Ammor(ItemID.None);
        }

    }

}
