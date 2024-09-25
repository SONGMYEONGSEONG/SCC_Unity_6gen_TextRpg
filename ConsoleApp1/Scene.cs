using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG;
using System.Linq;

namespace TextRPG
{
    public abstract class Scene
    {
        protected Player curplayer;
        public Player Curplayer { set { curplayer = value; } }

        protected SceneType changeSceneType;
        public SceneType ChangSeceneType { get { return changeSceneType; } set { changeSceneType = value; } }

        public void SetPlayer(Player player)
        {
            Curplayer = player;
        }
        public bool InputCheck(ref int selectNum, int selectMax)
        {
            string select = Console.ReadLine();

            if (!(int.TryParse(select, out int number)) || !(selectMax > number))
            {
                StrBuild.Print("잘못된 입력입니다.");
                StrBuild.Print("아무키나 눌러주십시오.");
                return false;
            }

            selectNum = number;
            return true;
        }
        public abstract void Initialize(Player player);
        public abstract void Print();
        public abstract void Update();

        public virtual void Exit(ref SceneType curScene,  ref Player player)
        {
            curScene = ChangSeceneType;
            player = curplayer;
        }
    }

    public class TitleScene : Scene
    {
        enum TitleSceneSelect
        {
            NoSelect = -1,
            Status = 1,
            Inventory = 2,
            Shop = 3,
            Dungeon = 4,
            Rest = 5,
            Save = 6,
            Reset = 7,

            SelectEnd
        }

        public override void Initialize(Player player)
        {
            base.SetPlayer(player);

            ChangSeceneType = SceneType.Title;

            if(curplayer.Statue.name == "_NoNamE")
            {
                StrBuild.Print("스파르타 마을에 오신 여러분 환영합니다.\r\n이름을 입력해주십시오.\r\n");

                StatusData statusData = curplayer.Statue;
                statusData.name = Console.ReadLine();
                curplayer.Statue = statusData;
                Console.Clear();
            }

            
            Print();
        }

        //이름 입력 메서드
        private string NameRegister()
        {
            string name = Console.ReadLine();
            return name;
        }

        public override void Print()
        {
            Console.Clear();
            StrBuild.Print("스파르타 마을에 오신 여러분 환영합니다.\r\n이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\r\n");

            StrBuild.Print("1.상태 보기");
            StrBuild.Print("2.인벤토리");
            StrBuild.Print("3.상점");
            StrBuild.Print("4.던전입장");
            StrBuild.Print("5.휴식하기");
            StrBuild.Print("6.저장하기");
            StrBuild.Print("7.데이터초기화\n");
            StrBuild.Print("원하시는 행동을 입력해주세요.\r\n>>");
        }

        public override void Update()
        {
            TitleSceneSelect selectNum = TitleSceneSelect.NoSelect;

            while (ChangSeceneType == SceneType.Title)
            {
                int number = -1;

                if (!(InputCheck(ref number, (int)TitleSceneSelect.SelectEnd)))
                {
                    Console.ReadLine();
                    return;
                }
                selectNum = (TitleSceneSelect)number;

                switch (selectNum)
                {
                    case TitleSceneSelect.Status:
                        ChangSeceneType = SceneType.Status;
                        break;

                    case TitleSceneSelect.Inventory:
                        ChangSeceneType = SceneType.Inventory;
                        break;

                    case TitleSceneSelect.Shop:
                        ChangSeceneType = SceneType.Shop;
                        break;

                    case TitleSceneSelect.Dungeon:
                        ChangSeceneType = SceneType.Dungeon;
                        break;
                        
                    case TitleSceneSelect.Rest:
                        ChangSeceneType = SceneType.Rest;
                        break;

                    case TitleSceneSelect.Save:
                        TextFileStream.Save(curplayer);
                        StrBuild.Print("저장 되었습니다.");
                        Console.ReadLine();
                        Console.Clear();
                        break;

                    case TitleSceneSelect.Reset:
                        TextFileStream.Reset();
                        curplayer.PlayerReset();
                        TextFileStream.Load(ref curplayer);
                        StrBuild.Print("초기화 되었습니다.");
                        Console.ReadLine();
                        Console.Clear();
                        return;
                }

                Print();
            }
        }

    }

    public class StatusScene : Scene
    {
        enum StatusSceneSelect
        {
            NoSelect = -1,
            Exit = 0,

            SelectEnd
        }

        public override void Initialize(Player player)
        {
            base.SetPlayer(player);

            changeSceneType = SceneType.Status;

            Print();
        }


        public override void Print()
        {
            Console.Clear();
            StrBuild.Print("상태 보기\r\n캐릭터의 정보가 표시됩니다.");

            //공격력 장착 아이템 분배
            float sumAtk = 0;
            if(curplayer.EquipWeapon.ID != ItemID.None)
            {
               sumAtk = curplayer.EquipWeapon.AtkValue;
            }
            //방어력 장착 아이템 분배
            float sumDef = 0;
            if (curplayer.EquipAmmor.ID != ItemID.None)
            {
                sumDef = curplayer.EquipAmmor.DefValue;
            }

            StrBuild.StatusPrint(curplayer.Statue,sumAtk,sumDef);

            StrBuild.Print("0. 나가기\r\n\r\n원하시는 행동을 입력해주세요.\r\n>>");
        }

        public override void Update()
        {
            StatusSceneSelect selectNum = StatusSceneSelect.NoSelect;

            while (ChangSeceneType == SceneType.Status)
            {
                int number = -1;

                if (!(InputCheck(ref number, (int)StatusSceneSelect.SelectEnd)))
                {
                    Console.ReadLine();
                    return;
                }
                selectNum = (StatusSceneSelect)number;

                switch (selectNum)
                {
                    case StatusSceneSelect.Exit:
                        ChangSeceneType = SceneType.Title;
                        break;
                }

            }
        }

    }

    public class InventoryScene : Scene
    {
        enum InventorySelect
        {
            NoSelect = -1,
            EquipSelect = 1,
            Exit = 0,

            SelectEnd =2
        }

        public override void Initialize(Player player)
        {
            base.SetPlayer(player);

            ChangSeceneType = SceneType.Inventory;
            Print();
        }

        public override void Print()
        {
            Console.Clear();
            StrBuild.Print("인벤토리\r\n보유 중인 아이템을 관리할 수 있습니다.\n");

            StrBuild.Print("[아이템목록]\n");

            int selectCount = 1;
            //Player가 가지고 있는 Item 리스트 출력하기
            foreach (KeyValuePair<ItemID, Item> item in curplayer.Items)
            {
                string name = item.Value.Name;
                string valuelabel = item.Value.ValueLabel;
                float value = item.Value.Value;
                string info = item.Value.Info;
                string equipStr = "";
                if (item.Value.Equip) { equipStr = "[E]"; }

                string result = $"-{equipStr}{name}    | {valuelabel} +{value} | {info}";
                StrBuild.Print(result);

                selectCount++;
            }

            StrBuild.Print("\n1. 장착 관리");
            StrBuild.Print("0. 나가기\r\n\r\n원하시는 행동을 입력해주세요.\r\n>>");
        }

        public override void Update()
        {
            InventorySelect selectNum = InventorySelect.NoSelect;

            while (ChangSeceneType == SceneType.Inventory)
            {
                int number = -1;

                if (!(InputCheck(ref number, (int)InventorySelect.SelectEnd)))
                {
                    Console.ReadLine();
                    return;
                }
                selectNum = (InventorySelect)number;

                switch (selectNum)
                {
                    case InventorySelect.EquipSelect:
                        ChangSeceneType = SceneType.Equipment;
                        break;
                    case InventorySelect.Exit:
                        ChangSeceneType = SceneType.Title;
                        break;
                }

            }
        }

    }

    public class EquipmentScene : Scene
    {
        List<ItemID> equipItemList;

        enum EquipmentSelect
        {
            NoSelect = -1,
            Exit = 0,
            SelectEnd = 99
        }

        public override void Initialize(Player player)
        {
            base.SetPlayer(player);

            equipItemList = new List<ItemID>();
            equipItemList.Capacity = curplayer.Items.Count;
            for(int i = 0; i <  equipItemList.Capacity; i++)
            {
                ItemID nullID = ItemID.None;
                equipItemList.Add(nullID);
            }

            ChangSeceneType = SceneType.Equipment;
            Print();
        }

        public override void Print()
        {
            Console.Clear();
            StrBuild.Print("인벤토리 - 장착관리\r\n보유 중인 아이템을 관리할 수 있습니다.\n");

            StrBuild.Print("[아이템목록]\n");

            int selectCount = 1;
            //Player가 가지고 있는 Item 리스트 출력하기
            foreach (KeyValuePair<ItemID, Item> item in curplayer.Items)
            {
                string name = item.Value.Name;
                string valuelabel = item.Value.ValueLabel;
                float value = item.Value.Value;
                string info = item.Value.Info;
                string equipStr = "";
                if (item.Value.Equip) { equipStr = "[E]"; }

                string result = $"{selectCount}.{equipStr}{name}    | {valuelabel} +{value} | {info}";
                StrBuild.Print(result);

                equipItemList[selectCount-1] = (item.Key);

               selectCount++;
            }

            StrBuild.Print("");
            StrBuild.Print("0. 나가기\r\n\r\n원하시는 행동을 입력해주세요.\r\n>>");
        }

        public override void Update()
        {
            while (ChangSeceneType == SceneType.Equipment)
            {
                int number = -1;
                
                if (!(InputCheck(ref number, equipItemList.Count + 1)))
                {
                    Console.ReadLine();
                    return;
                }
               
                switch ((EquipmentSelect)number)
                {
                    case EquipmentSelect.Exit: //두번쨰 enum 형변화후 선택지 고르기 
                        ChangSeceneType = SceneType.Title;
                        break;

                    default: //첫번쨰 장비 장착 코드
                        number -= 1;

                        if( curplayer.Items[equipItemList[number]] is Weapon)
                        {
                            if (curplayer.EquipWeapon.ID == ItemID.None)
                            {
                                curplayer.Items[equipItemList[number]].Equip = true;
                                curplayer.EquipWeapon = curplayer.Items[equipItemList[number]] as Weapon;
                            }
                            else
                            {
                                curplayer.Items[curplayer.EquipWeapon.ID].Equip = false;

                                curplayer.Items[equipItemList[number]].Equip = true;
                                curplayer.EquipWeapon = curplayer.Items[equipItemList[number]] as Weapon;
                            }

                            
                        }
                        else if(curplayer.Items[equipItemList[number]] is Ammor)
                        {
                            if (curplayer.EquipAmmor.ID == ItemID.None)
                            {
                                curplayer.Items[equipItemList[number]].Equip = true;
                                curplayer.EquipAmmor = curplayer.Items[equipItemList[number]] as Ammor;
                            }
                            else
                            {
                                curplayer.Items[curplayer.EquipAmmor.ID].Equip = false;

                                curplayer.Items[equipItemList[number]].Equip = true;
                                curplayer.EquipAmmor = curplayer.Items[equipItemList[number]] as Ammor;
                            }

                        }

                        //bool oldEquip = curplayer.Items[equipItemList[number]].Equip;
                        //curplayer.Items[equipItemList[number]].Equip = !oldEquip;

                        //if (curplayer.Items[equipItemList[number]].Equip != oldEquip)
                        //{
                        //    Print();
                        //}


                        break;
                }

                Print();
            }
        }
    }

    public class ShopScene : Scene
    {
        Dictionary<ItemID,Item> sellItems;
        List<KeyValuePair<ItemID, bool>> isSellItems;

        enum ShopSelect
        {
            NoSelect = -1,
            Exit = 0,
            Buy = 1,
            Sell = 2,
            SelectEnd
        }

        private Dictionary<ItemID, Item> SellItemInit()
        {
            Dictionary<ItemID, Item> sellList = ItemManager.allItems;
            return sellList;
            
        }

        public override void Initialize(Player player)
        {
            base.SetPlayer(player);

            if (sellItems == null)
            {
                sellItems = SellItemInit();
            }

            if(isSellItems == null)
            {
                isSellItems = new List<KeyValuePair<ItemID, bool>>();
                isSellItems.Capacity = sellItems.Count;

                foreach (ItemID itemID in sellItems.Keys)
                {
                    KeyValuePair<ItemID, bool> NoneSellID = new KeyValuePair<ItemID, bool>(itemID, false);
                    isSellItems.Add(NoneSellID);
                }
            }

            ChangSeceneType = SceneType.Shop;
        }

        public override void Print()
        {
            Console.Clear();
            StrBuild.Print("상점\r\n필요한 아이템을 얻을 수 있는 상점입니다.\n");

            StrBuild.Print("[보유골드]");
            string isGold = $"{curplayer.Statue.gold} G\n";
            StrBuild.Print(isGold);

            StrBuild.Print("[아이템목록]\n");

            int selectCount = 1;
            //상점이 가지고 있는 판매목록 아이템 리스트 출력
            foreach (KeyValuePair<ItemID, Item> item in sellItems)
            {
                //해당 아이템 오브젝트가 빈 아이템코드인경우 탈출
                if (item.Key == ItemID.None) break;

                string name = item.Value.Name;
                string valuelabel = item.Value.ValueLabel;
                float value = item.Value.Value;
                string info = item.Value.Info;
                int buyGold = item.Value.BuyGold;
                string result;

                if (isSellItems[selectCount - 1].Value)
                {
                    result = $"-{name}    | {valuelabel} +{value} | {info}      | 구매완료";
                }
                else
                {
                    result = $"-{name}    | {valuelabel} +{value} | {info}      |{buyGold} G";
                }
                StrBuild.Print(result);

                selectCount++;
            }

            StrBuild.Print("");
            StrBuild.Print("1. 아이템 구매");
            StrBuild.Print("2. 아이템 판매");
            StrBuild.Print("0. 나가기\r\n\r\n원하시는 행동을 입력해주세요.\r\n>>");
        }

        public override void Update()
        {
            while (ChangSeceneType == SceneType.Shop)
            {
                Print();

                int number = -1;

                if (!(InputCheck(ref number, (int)ShopSelect.SelectEnd)))
                {
                    Console.ReadLine();
                    return;
                }

                switch ((ShopSelect)number)
                {
                    case ShopSelect.Exit:
                        ChangSeceneType = SceneType.Title;
                        break;

                    case ShopSelect.Buy:
                        BuyItemPrint();
                        BuyItem();
                        break;

                    case ShopSelect.Sell:
                        List<ItemID> playerItemID = PlayerSellInit();
                        PlayerSellItemPrint();
                        PlayerSellItem(playerItemID);
                        break;
                }

            }
        }

        private void BuyItemPrint()
        {
            Console.Clear();
            StrBuild.Print("상점 - 아이템 구매\r\n필요한 아이템을 얻을 수 있는 상점입니다.\n");

            StrBuild.Print("[보유골드]");
            string isGold = $"{curplayer.Statue.gold} G\n";
            StrBuild.Print(isGold);

            StrBuild.Print("[아이템목록]\n");

            int selectCount = 1;
            //상점이 가지고 있는 판매목록 아이템 리스트 출력
            foreach (KeyValuePair<ItemID, Item> item in sellItems)
            {
                //해당 아이템 오브젝트가 빈 아이템코드인경우 탈출
                if(item.Key == ItemID.None) break;

                string name = item.Value.Name;
                string valuelabel = item.Value.ValueLabel;
                float value = item.Value.Value;
                string info = item.Value.Info;
                int buyGold = item.Value.BuyGold;
                string result;

                if (isSellItems[selectCount - 1].Value)
                {
                    result = $"{selectCount}.{name}    | {valuelabel} +{value} | {info}      | 구매완료";
                }
                else
                {
                    result = $"{selectCount}.{name}    | {valuelabel} +{value} | {info}      |{buyGold} G";
                }
                StrBuild.Print(result);

                selectCount++;
            }

            StrBuild.Print("");
            StrBuild.Print("0. 나가기\r\n\r\n원하시는 행동을 입력해주세요.\r\n>>");
        }

        private void BuyItem()
        {
            int number = -1;

            if (!(InputCheck(ref number, sellItems.Count+1)))
            {
                Console.ReadLine();
                return;
            }

            if(number == (int)ShopSelect.Exit) return;

            ItemID buyItemID = isSellItems[number - 1].Key;

            //아이템 구매 구현
            if (curplayer.Statue.gold >= sellItems[buyItemID].BuyGold)
            {
                //Test :: 튜플로 만들어 할지 아닐지 , 지금 읽기속성이라 아예 새로 바꿔줘야됨
                isSellItems[number - 1] = new KeyValuePair<ItemID, bool>(buyItemID, true);

                StatusData Status = curplayer.Statue;
                Status.gold -= sellItems[buyItemID].BuyGold;
                curplayer.Statue = Status;

                if (curplayer.Items.ContainsKey(buyItemID))
                {
                    curplayer.Items[buyItemID].Count++;
                }
                else
                {
                    curplayer.Items.Add(buyItemID, sellItems[buyItemID]);
                }

                string buyResult = $"{curplayer.Items[buyItemID].Name} 을 구매하였습니다.";
                StrBuild.Print(buyResult);
                Console.ReadLine();
            }
            else
            {
                StrBuild.Print("Gold가 부족합니다.");
                StrBuild.Print("아무키나 눌러주십시오.");
                Console.ReadLine();
            }
        }

        private List<ItemID> PlayerSellInit()
        {
            List<ItemID> playerItemID = new List<ItemID>();
            playerItemID.Capacity = curplayer.Items.Count;

            foreach (ItemID itemID in curplayer.Items.Keys)
            {
                playerItemID.Add(itemID);
            }

            return playerItemID;
        }

        private void PlayerSellItemPrint()
        {
            Console.Clear();
            StrBuild.Print("상점 - 아이템 판매\r\n보유 중인 아이템을 판매 할 수 있습니다.\n");

            StrBuild.Print("[보유골드]");
            string isGold = $"{curplayer.Statue.gold} G\n";
            StrBuild.Print(isGold);

            StrBuild.Print("[아이템목록]\n");

            int selectCount = 1;
            //Player가 가지고 있는 Item 리스트 출력하기
            foreach (KeyValuePair<ItemID, Item> item in curplayer.Items)
            {
                string name = item.Value.Name;
                string valuelabel = item.Value.ValueLabel;
                float value = item.Value.Value;
                string info = item.Value.Info;
                string equipStr = "";
                if (item.Value.Equip) { equipStr = "[E]"; }

                int sellGold = (int)(item.Value.BuyGold * 0.85f);
                int count = item.Value.Count;

                string result = $"{selectCount}.{equipStr}{name}    | {valuelabel} +{value} | {info}    | { sellGold } G    |{ count } ";
                StrBuild.Print(result);

                selectCount++;
            }

            StrBuild.Print("");
            StrBuild.Print("0. 나가기\r\n\r\n원하시는 행동을 입력해주세요.\r\n>>");
        }

        private void PlayerSellItem(List<ItemID> playerItemID)
        {
            int number = -1;

            if (!(InputCheck(ref number, curplayer.Items.Count+1)))
            {
                Console.ReadLine();
                return;
            }

            if (number == (int)ShopSelect.Exit) return;

            //아이템 판매 구현
            ItemID sellItemID = playerItemID[number -1];
           
            StatusData Status = curplayer.Statue;
            Status.gold += (int)(curplayer.Items[sellItemID].BuyGold * 0.85f);
            curplayer.Statue = Status;

            string buyResult = $"{curplayer.Items[sellItemID].Name} 을 판매하였습니다.";
            StrBuild.Print(buyResult);

            if (sellItemID == curplayer.EquipWeapon.ID)
            {
                curplayer.EquipWeapon = new Weapon(ItemID.None);
            }
            else if(sellItemID == curplayer.EquipAmmor.ID)
            {
                curplayer.EquipAmmor = new Ammor(ItemID.None);
            }

            curplayer.Items.Remove(sellItemID);
           
            Console.ReadLine();
        }
    }

    public class DungeonScene : Scene
    {
        enum DungeonSceneSelect
        {
            NoSelect = -1,
            Exit = 0,

            SelectEnd
        }

        int dungenosMax; 
        Dungeon[] dungenos;

        public override void Initialize(Player player)
        {
            base.SetPlayer(player);

            changeSceneType = SceneType.Dungeon;

            dungenosMax = 3;
            dungenos = new Dungeon[dungenosMax];

            dungenos[0] = new Dungeon("쉬운 던전", 5, 1000);
            dungenos[1] = new Dungeon("일반 던전", 11, 1700);
            dungenos[2] = new Dungeon("어려운 던전", 17, 2500);

            Print();
        }

        public override void Print()
        {
            Console.Clear();
            StrBuild.Print("던전입장");
            StrBuild.Print("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");

            for(int i = 0; i < dungenos.Length; i++)
            {
                string result = $"{i +1 }. {dungenos[i].name}    | 방어력 {dungenos[i].accessDefCondition}이상 권장 ";
                StrBuild.Print(result);
            }

            StrBuild.Print("0. 나가기\r\n\r\n원하시는 행동을 입력해주세요.\r\n>>");
        }

        public override void Update()
        {
            DungeonSceneSelect selectNum = DungeonSceneSelect.NoSelect;

            while (ChangSeceneType == SceneType.Dungeon)
            {
                int number = -1;

                if (!(InputCheck(ref number, dungenos.Length+1)))
                {
                    Console.ReadLine();
                    return;
                }

                switch ((DungeonSceneSelect)number)
                {
                    case DungeonSceneSelect.Exit:
                        ChangSeceneType = SceneType.Title;
                        break;

                    default:

                        switch (curplayer.Statue.health > 0)
                        {
                            case true: //플레이어 체력이 있을때 던전입장 가능

                                float playerDef = curplayer.Statue.def;
                                if (curplayer.EquipAmmor != null)
                                {
                                    playerDef += curplayer.EquipAmmor.DefValue;
                                }

                                if (playerDef >= dungenos[number - 1].accessDefCondition) // 권장 방어력보다 높음
                                {
                                    DungeonClear(dungenos[number - 1], playerDef);
                                    ChangSeceneType = SceneType.Title;
                                }

                                else // 권장 방어력보다 낮음
                                {
                                    Random rand = new Random();
                                    if (rand.Next(0, 100) < 40) // 40%확률로 던전 실패
                                    {
                                        DungeonFail(dungenos[number - 1]);
                                    }
                                    else
                                    {
                                        DungeonClear(dungenos[number - 1], playerDef);
                                    }

                                    ChangSeceneType = SceneType.Title;
                                }

                                break;

                            case false: //플레이어 체력이 0이기에 던전입장 불가능
                                StrBuild.Print("현재 체력이 0 입니다. 입장이 불가능 합니다.");
                                StrBuild.Print("아무키를 누르시면 Title 화면으로 이동합니다.");
                                Console.ReadLine();
                                ChangSeceneType = SceneType.Title;
                                break;
                           
                        }

                        break;
                }

            }
        }

        private void DungeonClear(Dungeon dungeno, float playerDef)
        {
            //던전 클리어 , 권장 방어력 +-에 따라 종료시 체력 소모 반영
            //기본 체력 감소량 20 - 35랜덤
            //(내 방어력 - 권장 방어력) 만큼 랜덤 값 설정
            //ex)내 방어력 : 11 , 권장 방어력(accessDefCondition) : 7
            //(20+(11-7)) 에서 (35+(11-7)) 랜덤 체력 소모 값
            Random rand = new Random();
            int hpDecrease = rand.Next(20, 36);
            hpDecrease += (int)(playerDef - dungeno.accessDefCondition);

            StatusData scenePlayerStatus = curplayer.Statue;
            if(scenePlayerStatus.health - hpDecrease < 0)
            {
                scenePlayerStatus.health = 0;
            }
            else
            {
                scenePlayerStatus.health -= hpDecrease;
            }
           
            /*
            - 공격력  ~ 공격력 * 2 의 % 만큼 추가 보상 획득 가능
            - ex) 공격력 10, 쉬움 던전
            기본 보상 1000 G
            공격력으로 인한 추가 보상 10 ~ 20%
            - ex) 공격력 15, 어려운 던전
            기본 보상 2500 G
            공격력으로 인한 추가 보상 15 ~ 30%
             */

            float playerAtk = curplayer.Statue.atk;
            if (curplayer.EquipWeapon != null)
            {
                playerAtk += curplayer.EquipWeapon.AtkValue;
            }

            int rewardPercent = rand.Next((int)(playerAtk), (int)(playerAtk * 2));
            int Reward = (int)(dungeno.clearGold + (dungeno.clearGold * (rewardPercent * 0.01f)));

            scenePlayerStatus.gold += Reward;

           scenePlayerStatus.clearCount++;
           bool isLevelUp = scenePlayerStatus.LevelCheck();

            int number = -1;
            while (number != (int)DungeonSceneSelect.Exit)
            {
                Console.Clear();

                DungeonClearPrint(dungeno, scenePlayerStatus, isLevelUp);

                if (!(InputCheck(ref number, (int)DungeonSceneSelect.SelectEnd)))
                {
                    Console.ReadLine();
                }
            }

            curplayer.Statue = scenePlayerStatus;
        }

        private void DungeonClearPrint(Dungeon dungeno, StatusData scenePlayerStatus,bool isLevelUp)
        {
            StrBuild.Print("던전 클리어");
            StrBuild.Print("축하합니다!!");

            string result = $"{dungeno.name}을 클리어 하였습니다.\n";
            StrBuild.Print(result);

            if(isLevelUp)
            {
                StrBuild.Print("Level UP!!!!");
                result = $"레벨 {curplayer.Statue.level} -> {scenePlayerStatus.level}\n";
                StrBuild.Print(result);
            }

            result = $"체력 {curplayer.Statue.health} -> {scenePlayerStatus.health}";
            StrBuild.Print(result);

            result = $"Gold {curplayer.Statue.gold} -> {scenePlayerStatus.gold}\n";
            StrBuild.Print(result);

            StrBuild.Print("0. 나가기\r\n\r\n원하시는 행동을 입력해주세요.\r\n>>");

        }

        private void DungeonFail(Dungeon dungeno)
        {
            //보상없고 체력 감소 절반
            StatusData scenePlayerStatus = curplayer.Statue;
            scenePlayerStatus.health = (int)(scenePlayerStatus.health * 0.5f);

            int number = -1;
            while (number != (int)DungeonSceneSelect.Exit)
            {
                Console.Clear();

                DungeonFailPrint(dungeno, scenePlayerStatus);

                if (!(InputCheck(ref number, (int)DungeonSceneSelect.SelectEnd)))
                {
                    Console.ReadLine();
                }
            }

            curplayer.Statue = scenePlayerStatus;
        }

        private void DungeonFailPrint(Dungeon dungeno, StatusData scenePlayerStatus)
        {
            StrBuild.Print("던전 실패");
            
            string result = $"{dungeno.name}을 실패 하였습니다.\n";
            StrBuild.Print(result);

            result = $"체력 {curplayer.Statue.health} -> {scenePlayerStatus.health}";
            StrBuild.Print(result);

            StrBuild.Print("0. 나가기\r\n\r\n원하시는 행동을 입력해주세요.\r\n>>");

        }
    }

    public class RestScene : Scene
    {
        int costRest;
        int increaseHealth;

        enum RestSceneSelect
        {
            NoSelect = -1,
            Exit = 0,
            Rest = 1,
            

            SelectEnd 
        }

        public override void Initialize(Player player)
        {
            base.SetPlayer(player);

            changeSceneType = SceneType.Rest;
            costRest = 500;
            increaseHealth = 100;

            Print();
        }


        public override void Print()
        {
            Console.Clear();
            StrBuild.Print("휴식하기");

            string str = $"{costRest} G를 내면 체력을 회복할 수 있습니다.  (보유 골드 : { curplayer.Statue.gold } G )\n";
            StrBuild.Print(str);

            StrBuild.Print("1. 휴식하기");
            StrBuild.Print("0. 나가기\r\n\r\n원하시는 행동을 입력해주세요.\r\n>>");
        }

        public override void Update()
        {
            RestSceneSelect selectNum = RestSceneSelect.NoSelect;

            while (ChangSeceneType == SceneType.Rest)
            {
                int number = -1;

                if (!(InputCheck(ref number, (int)RestSceneSelect.SelectEnd)))
                {
                    Console.ReadLine();
                    return;
                }
                selectNum = (RestSceneSelect)number;

                switch (selectNum)
                {
                    case RestSceneSelect.Rest:
                        if (curplayer.Statue.gold >= costRest)
                        {
                            StatusData Status = curplayer.Statue;
                            Status.gold -= costRest;
                            if(Status.healthMax <= Status.health + increaseHealth)
                            {
                                Status.health = Status.healthMax;
                            }
                            else
                            {
                                Status.health += increaseHealth;
                            }
                            curplayer.Statue = Status;

                            StrBuild.Print("휴식을 완료했습니다.");
                            StrBuild.Print("아무키나 누르면 메인화면으로 이동합니다.");
                            ChangSeceneType = SceneType.Title;
                        }
                        else
                        {
                            StrBuild.Print("Gold가 부족합니다.");
                            StrBuild.Print("아무키나 눌러주십시오.");

                        }

                        Console.ReadLine();
                        break;

                    case RestSceneSelect.Exit:
                        ChangSeceneType = SceneType.Title;
                        break;
                }

            }
        }

    }

}
