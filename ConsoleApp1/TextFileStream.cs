using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TextRPG
{
    public static class TextFileStream
    {
        static string[] filePaths = new string[4];

        public static void Init()
        {
            ////save.txt를 저장할 폴더 생성
            //Directory.CreateDirectory("../../../SaveFolder");

            //save.txt 경로 생성
            filePaths[0] = @"../../../SaveFolder/playerSave.txt";
            filePaths[1] = @"../../../SaveFolder/inventorySave.txt";
            filePaths[2] = @"../../../SaveFolder/equipSave.txt";
            filePaths[3] = @"../../../SaveFolder/allItemList.txt";

            using (StreamReader reader = new StreamReader(filePaths[3]))
            {
                List<string> loadItemData = new List<string>();

                while (reader.EndOfStream == false)
                {
                    string readData = reader.ReadLine();

                    string[] readDatas = readData.Split(',');

                    //총 9가지
                    ItemID id = (ItemID)int.Parse(readDatas[0]);
                    bool equip = readDatas[1] == "true" ? true : false ;
                    string name = readDatas[2];
                    string valueLabel = readDatas[3];
                    float value = float.Parse(readDatas[4]);
                    string info = readDatas[5];
                    int count = int.Parse(readDatas[6]);
                    int countMax = int.Parse(readDatas[7]);
                    int buyGold = int.Parse(readDatas[8]);

                    switch (valueLabel)
                    {
                        case "공격력":
                            Item weapon = new Weapon(id,equip,name,valueLabel,value,info,count,countMax,buyGold);
                            ItemManager.allItems.Add(id, weapon);
                            break;

                        case "방어력":
                            Item Ammor = new Ammor(id, equip, name, valueLabel, value, info, count, countMax, buyGold);
                            ItemManager.allItems.Add(id, Ammor);
                            break;
                        default:
                            Item item = new Item(id, equip, name, valueLabel, value, info, count, countMax, buyGold);
                            ItemManager.allItems.Add(id, item);
                            break;
                    }

                }

            }
        }

        public static void Reset()
        {
            StringBuilder builder = StrBuild.strBuilder;

            using (StreamWriter writer = File.CreateText(filePaths[0]))
            {
                builder.Clear();
                builder.Append("1\n");
                builder.Append("_NoNamE\n");
                builder.Append("전사\n");
                builder.Append("10\n");
                builder.Append("5\n");
                builder.Append("100\n");
                builder.Append("100\n");
                builder.Append("1500");
                writer.WriteLine(builder.ToString());
            }
            using (StreamWriter writer = File.CreateText(filePaths[1]))
            {
                Item item = ItemManager.allItems[ItemID.MetalAmmor];

                builder.Clear();
                builder.Append((int)item.ID);
                builder.Append(",");
                builder.Append(item.Equip);
                builder.Append(",");
                builder.Append(item.Name);
                builder.Append(",");
                builder.Append(item.ValueLabel);
                builder.Append(",");
                builder.Append(item.Value);
                builder.Append(",");
                builder.Append(item.Info);
                builder.Append(",");
                builder.Append(item.Count);
                builder.Append(",");
                builder.Append(item.CountMax);
                builder.Append(",");
                builder.Append(item.BuyGold);
                writer.WriteLine(builder.ToString());
            }
            using (StreamWriter writer = File.CreateText(filePaths[2]))
            {
                Item item = ItemManager.allItems[ItemID.None];

                //공격 장비 reset
                builder.Clear();
                builder.Append((int)item.ID);
                builder.Append(",");
                builder.Append(item.Equip);
                builder.Append(",");
                builder.Append(item.Name);
                builder.Append(",");
                builder.Append("공격력");
                builder.Append(",");
                builder.Append(item.Value);
                builder.Append(",");
                builder.Append(item.Info);
                builder.Append(",");
                builder.Append(item.Count);
                builder.Append(",");
                builder.Append(item.CountMax);
                builder.Append(",");
                builder.Append(item.BuyGold);
                writer.WriteLine(builder.ToString());

                //방어구 장비 reset
                builder.Clear();
                builder.Append((int)item.ID);
                builder.Append(",");
                builder.Append(item.Equip);
                builder.Append(",");
                builder.Append(item.Name);
                builder.Append(",");
                builder.Append("방어력");
                builder.Append(",");
                builder.Append(item.Value);
                builder.Append(",");
                builder.Append(item.Info);
                builder.Append(",");
                builder.Append(item.Count);
                builder.Append(",");
                builder.Append(item.CountMax);
                builder.Append(",");
                builder.Append(item.BuyGold);
                writer.WriteLine(builder.ToString());
            }

        }

        public static void Save(Player player)
        {
            //텍스트 파일 생성 및 접근
            //외부에서 파일을 열 경우 .close로 파일을 닫아줘야하지만
            //using을 사용하면 해당 괄호 동작후 파일이 자동으로 닫히게 된다.
            using (StreamWriter writer = File.CreateText(filePaths[0]))
            {
                writer.WriteLine(player.Statue.level);
                writer.WriteLine(player.Statue.name);
                writer.WriteLine(player.Statue.job);
                writer.WriteLine(player.Statue.atk);
                writer.WriteLine(player.Statue.def);
                writer.WriteLine(player.Statue.health);
                writer.WriteLine(player.Statue.healthMax);
                writer.WriteLine(player.Statue.gold);
            }

            using (StreamWriter writer = File.CreateText(filePaths[1]))
            {
                StringBuilder builder = StrBuild.strBuilder;

                foreach (KeyValuePair<ItemID,Item> itemData in player.Items)
                {
                    builder.Clear();
                    builder.Append($"{(int)itemData.Key},");
                    builder.Append($"{itemData.Value.Equip},");
                    builder.Append($"{itemData.Value.Name},");
                    builder.Append($"{itemData.Value.ValueLabel},");
                    builder.Append($"{itemData.Value.Value},");
                    builder.Append($"{itemData.Value.Info},");
                    builder.Append($"{itemData.Value.Count},");
                    builder.Append($"{itemData.Value.CountMax},");
                    builder.Append($"{itemData.Value.BuyGold},");
                    writer.WriteLine(builder.ToString());
                } 
            }

            using (StreamWriter writer = File.CreateText(filePaths[2]))
            {
                StringBuilder builder = StrBuild.strBuilder;

                Weapon equipWeapon = player.EquipWeapon;
                builder.Clear();
                builder.Append($"{(int)equipWeapon.ID},");
                builder.Append($"{equipWeapon.Equip},");
                builder.Append($"{equipWeapon.Name},");
                builder.Append($"{equipWeapon.ValueLabel},");
                builder.Append($"{equipWeapon.Value},");
                builder.Append($"{equipWeapon.Info},");
                builder.Append($"{equipWeapon.Count},");
                builder.Append($"{equipWeapon.CountMax},");
                builder.Append($"{equipWeapon.BuyGold},");
                writer.WriteLine(builder.ToString());

                Ammor equipAmmor = player.EquipAmmor;
                builder.Clear();
                builder.Append($"{(int)equipAmmor.ID},");
                builder.Append($"{equipAmmor.Equip},");
                builder.Append($"{equipAmmor.Name},");
                builder.Append($"{equipAmmor.ValueLabel},");
                builder.Append($"{equipAmmor.Value},");
                builder.Append($"{equipAmmor.Info},");
                builder.Append($"{equipAmmor.Count},");
                builder.Append($"{equipAmmor.CountMax},");
                builder.Append($"{equipAmmor.BuyGold},");
                writer.WriteLine(builder.ToString());
            }
        }

        public static void Load(ref Player player)
        {
            StatusData loadStatus = new StatusData();
            List<string> loadData = new List<string>();

            //텍스트 파일 생성 및 접근
            //외부에서 파일을 열 경우 .close로 파일을 닫아줘야하지만
            //using을 사용하면 해당 괄호 동작후 파일이 자동으로 닫히게 된다.
            using (StreamReader reader = new StreamReader(filePaths[0]))
            {
                while(reader.EndOfStream == false)
                {
                    string readData = reader.ReadLine();
                    loadData.Add(readData);
                }

                loadStatus.level = int.Parse(loadData[0]);
                loadStatus.name = loadData[1];
                loadStatus.job = loadData[2];
                loadStatus.atk = float.Parse(loadData[3]);
                loadStatus.def = int.Parse(loadData[4]);
                loadStatus.health = int.Parse(loadData[5]);
                loadStatus.healthMax = int.Parse(loadData[6]);
                loadStatus.gold = int.Parse(loadData[7]);

                player.Statue = loadStatus;
            }

            using (StreamReader reader = new StreamReader(filePaths[1]))
            {
                List<string> loadItemData = new List<string>();

                while (reader.EndOfStream == false)
                {
                    string readData = reader.ReadLine();

                    string[] readDatas = readData.Split(',');

                    //총 9가지
                    ItemID id = (ItemID)int.Parse(readDatas[0]);
                    bool equip = readDatas[1] == "True" ? true : false;
                    string name = readDatas[2];
                    string valueLabel = readDatas[3];
                    float value = float.Parse(readDatas[4]);
                    string info = readDatas[5];
                    int count = int.Parse(readDatas[6]);
                    int countMax = int.Parse(readDatas[7]);
                    int buyGold = int.Parse(readDatas[8]);

                    switch (valueLabel)
                    {
                        case "공격력":
                            Item weapon = new Weapon(id, equip, name, valueLabel, value, info, count, countMax, buyGold);
                            player.Items.Add(weapon.ID, weapon);
                            break;

                        case "방어력":
                            Item Ammor = new Ammor(id, equip, name, valueLabel, value, info, count, countMax, buyGold);
                            player.Items.Add(Ammor.ID, Ammor);
                            break;
                        default:
                            Item Item = new Item(id, equip, name, valueLabel, value, info, count, countMax, buyGold);
                            player.Items.Add(Item.ID, Item);
                            break;

                    }

                   
                }
            }
            using (StreamReader reader = new StreamReader(filePaths[2]))
            {
                while (reader.EndOfStream == false)
                {
                    string readData = reader.ReadLine();

                    string[] readDatas = readData.Split(',');

                    //Item의 필드는 총 9가지
                    ItemID id = (ItemID)int.Parse(readDatas[0]);
                    bool equip = readDatas[1] == "True" ? true : false;
                    string name = readDatas[2];
                    string valueLabel = readDatas[3];
                    float value = float.Parse(readDatas[4]);
                    string info = readDatas[5];
                    int count = int.Parse(readDatas[6]);
                    int countMax = int.Parse(readDatas[7]);
                    int buyGold = int.Parse(readDatas[8]);

                    switch (valueLabel)
                    {
                        case "공격력": 
                            player.EquipWeapon = new Weapon(id, equip, name, valueLabel, value, info, count, countMax, buyGold);
                            break;

                        case "방어력":
                            player.EquipAmmor= new Ammor(id, equip, name, valueLabel, value, info, count, countMax, buyGold);
                            break;
                        default:

                            break;
                    }


                }
            }
        }

    }
}
