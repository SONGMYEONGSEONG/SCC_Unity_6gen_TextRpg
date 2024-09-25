using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG;

namespace TextRPG
{
    public class GameManager
    {
        SceneType curScene; //현재 게임이 어느 Scene을 가르키는지 확인하는 변수
        Scene[] scenes;
        Player player;

        public GameManager()
        {
            TextFileStream.Init();

            curScene = SceneType.Title;
            scenes = new Scene[(int)SceneType.SceneEnd];

            scenes[(int)SceneType.Title] = new TitleScene();
            scenes[(int)SceneType.Status] = new StatusScene();
            scenes[(int)SceneType.Inventory] = new InventoryScene();
            scenes[(int)SceneType.Equipment] = new EquipmentScene();
            scenes[(int)SceneType.Shop] = new ShopScene();
            scenes[(int)SceneType.Dungeon] = new DungeonScene();
            scenes[(int)SceneType.Rest] = new RestScene();

            //Load
            player = new Player();
            TextFileStream.Load(ref player);

        }

        public void Initialize()
        {

        }

        public void Update()
        {
            scenes[(int)curScene].Initialize(player);

            scenes[(int)curScene].Update();

            scenes[(int)curScene].Exit(ref curScene,ref player);
        }

    }
}
