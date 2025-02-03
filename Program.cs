using System.Net.Http.Headers;

namespace demo62飞行棋
{
    internal class Program
    {
        public static int[] Map = new int[100];//地图
        public static int[] PlayerPos = new int[2];// 玩家坐标
        public static string[] PlayerName = new string[2];// 玩家姓名
        public static bool[] Flags = new bool[2];// Flag[0] 默认值为False Flag[1]
        static void Main(string[] args)
        {
            GameHead();
            #region 输入玩家姓名
            //1.玩家a和玩家b姓名不相同的情况往下执行
            //2.玩家a和玩家b姓名不能相同，如果相同重新输入
            //3.玩家a和玩家b的姓名不能为空，为空重新输入
            Console.WriteLine("请输入玩家A的姓名");
            PlayerName[0] = Console.ReadLine();
            while (PlayerName[0]=="") 
            {
                Console.WriteLine("玩家A的姓名不能为空，请重新输入");
                PlayerName[0] = Console.ReadLine();
            }
            Console.WriteLine("请输入玩家B的姓名");
            PlayerName[1] = Console.ReadLine();
            while (PlayerName[1] == PlayerName[0] || PlayerName[1]=="")
            {
                if (PlayerName[1] == PlayerName[0])
                {
                    Console.WriteLine("玩家B的姓名不能和玩家A相同,请重新输入");
                    PlayerName[1] = Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("玩家B的姓名不能为空，请重新输入");
                    PlayerName[1] = Console.ReadLine();
                }

            }


            #endregion
            Console.Clear();//当用户输入完成按回车进行刷新清空上面的内容

            GameHead();
            Console.WriteLine($"玩家A的名字用{PlayerName[0]}");
            Console.WriteLine($"玩家B的名字用{PlayerName[1]}");


            InitMap();//初始化地图
            DrawMap();
            //玩游戏
            //玩家1和玩家2都没有到终点的时候
            while (PlayerPos[0]<99 && PlayerPos[1] < 99)
            {
                if (Flags[0] == false)
                {
                    PlayGame(0);
                }
                else
                {
                    Flags[0] = false;
                }
                if (PlayerPos[0]>=99)
                {
                    Console.WriteLine($"玩家{PlayerName[0]}赢了玩家{PlayerName[1]}");
                    break;
                }
                if (Flags[1] == false)
                {
                    PlayGame(1);
                }
                else
                {
                    Flags[1] = false;
                }
                if (PlayerPos[1] >= 99)
                {
                    Console.WriteLine($"玩家{PlayerName[1]}赢了玩家{PlayerName[0]}");
                    break;
                }


            }


            Console.ReadKey();


        }

        public static void PlayGame(int PlayName)//玩游戏
        {
            Random r = new Random();
            int number=r.Next(1,7);
            Console.WriteLine($"玩家{PlayerName[PlayName]}按任意键开始掷骰子");
            Console.ReadKey(true);//enter 写上true按任意键都是代表下一步
            Console.WriteLine($"玩家{PlayerName[PlayName]}掷出了{number}");
            PlayerPos[PlayName] += number;
            ChangePos();
            Console.ReadKey(true);
            Console.WriteLine($"玩家{PlayerName[PlayName]}按任意键开始掷骰子");
            Console.ReadKey(true);
            Console.WriteLine($"玩家{PlayerName[PlayName]}行动完了");
            Console.ReadKey(true);
            //1.玩家踩到了玩家 2，玩家踩到了关卡
            if (PlayerPos[PlayName] == PlayerPos[1- PlayName])//1-PlayName
            {
                Console.WriteLine($"玩家{PlayerName}[0]踩到了玩家{PlayerName[1- PlayName]},玩家{PlayerName[1 - PlayName]}退6格");
                PlayerPos[1- PlayName] -= 6;
                ChangePos();
                Console.ReadKey(true);
            }
            else
            {
                //2.玩家踩到了关卡
                //判断Map数组里面的值时多少 1，2，3，4
                switch (Map[PlayerPos[PlayName]])
                {
                    case 0:
                        Console.WriteLine($"玩家{PlayerName[PlayName]}踩到了方块，安全");
                        Console.ReadKey(true);
                        break;
                    case 1:
                        Console.WriteLine($"玩家{PlayerName[PlayName]}踩到了幸运轮盘，青选着1交换位置，2轰炸对方退六格");
                        //1.选择1 选择2 瞎输入
                        string input = Console.ReadLine();
                        while (true)
                        {
                            if (input == "1")
                            {
                                Console.WriteLine($"玩家{PlayerName[PlayName]}选择和玩家{PlayerName[1 - PlayName]}进行交换位置");
                                Console.ReadKey(true);
                                (PlayerName[PlayName], PlayerName[1 - PlayName]) = (PlayerName[1 - PlayName], PlayerName[PlayName]);
                                Console.WriteLine("交换完成，按任意键继续");
                                Console.ReadKey(true);
                                break ;
                            }
                            else if (input == "2")
                            {
                                Console.WriteLine($"玩家{PlayerName[PlayName]}选择轰炸玩家{PlayerName[1 - PlayName]},玩家{PlayerName[1 - PlayName]}退6格");
                                Console.ReadKey(true);
                                PlayerPos[1 - PlayName] -= 6;
                                ChangePos();
                                Console.WriteLine($"玩家{PlayerName[1 - PlayName]}退了6格");
                                Console.ReadKey(true);
                                break;
                            }
                            else
                            {
                                Console.WriteLine("您输入的格式有误，请从新输入只能1和2");
                                input = Console.ReadLine();
                            }
                        }
                        break;
                    case 2:
                        Console.WriteLine($"玩家{PlayerName[PlayName]}踩到了地雷，退六个格子");
                        Console.ReadKey(true);
                        PlayerPos[PlayName] -= 6;
                        ChangePos();
                        break;
                    case 3:
                        Console.WriteLine($"玩家{PlayerName[PlayName]}踩到了暂停，暂停一回合");
                        Console.ReadKey(true);
                        //暂停
                        Flags[PlayName] = true;
                        break;
                    case 4:
                        Console.WriteLine($"玩家{PlayerName[PlayName]}踩到了时空隧道，前进十格");
                        PlayerPos[PlayName] += 10;
                        ChangePos();
                        Console.WriteLine("前进完成");
                        Console.ReadKey(true);
                        break;
                }

            }
            ChangePos();
            Console.Clear();
            DrawMap();
        }
        public static void GameHead()
        {
            //Console.BackgroundColor = ConsoleColor.Red;//修改背景色
            Console.ForegroundColor = ConsoleColor.Green;//修改字体颜色
            Console.WriteLine("******************");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("******************");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("******************");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("*****Jun的飞行棋***");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("******************");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("******************");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("******************");
        }
        public static void InitMap()//初始化地图
        {
            //0空格
            int[] luckturn = { 6, 23, 40, 55, 69, 83 };//1
            for (int i = 0; i < luckturn.Length; i++)
            {
                int index = luckturn[i];
                Map[index] = 1;
            }
            int[] landmap = { 5, 13, 17, 33, 38, 50, 64, 80, 94 };//2
            for (int i = 0; i < landmap.Length; i++)
            {
                int index = landmap[i];
                Map[index] = 2;
            }

            int[] pause = { 9, 27, 60, 93 };//3
            for (int i = 0; i < pause.Length; i++)
            {
                int index = pause[i];
                Map[index] = 3;
            }
            int[] timetunne = { 20, 25, 45, 63, 72, 88, 90 };//4
            for (int i = 0; i < timetunne.Length; i++)
            {
                int index = timetunne[i];
                Map[index] = 4;
            }


        }

        public static void DrawMap() //画地图
        {
            Console.WriteLine("图列 幸运轮盘：©  地雷:★  暂停:▲  时空隧道:卍");
            #region 画第一横行
            //画第一行，1玩家在一起画<>括号
            //2.玩家不在一起画 A和B
            //3.判断数组里面存储的元素然后画地图
            for (int i = 0; i < 30; i++)
            {
                Console.Write(DrawMapIn(i));

            }
            Console.WriteLine();
            #endregion

            #region 画第一竖行
            for (int i = 30; i < 35; i++)
            {
                for (int j = 0; j < 31; j++)
                {
                    Console.Write(" ");
                }

                Console.WriteLine(DrawMapIn(i));
            }

            #endregion

            #region 画第二横行
            for (int i = 64;i >=35;i--)
            {
                Console.Write(DrawMapIn(i));
            }
            Console.WriteLine();//换行
            #endregion

            #region 画第二竖行
            for (int i = 65; i < 70; i++)
            {

                Console.WriteLine(DrawMapIn(i));
            }

            #endregion

            #region 画最后一横行
            for (int i = 70; i<99; i++)
            {
                Console.Write(DrawMapIn(i));
            }
            Console.WriteLine();
            #endregion

        }
        public static string DrawMapIn(int i)
        {
            string str = "";
            if (PlayerPos[0] == PlayerPos[1] && PlayerPos[0] == i)
            {
                str="P";
            }
            else if (PlayerPos[0] == i)
            {
                str="A";
            }
            else if (PlayerPos[1] == i)
            {
                str="B";
            }
            else
            {
                switch (Map[i])
                {
                    //▲©★卍⭕
                    case 0:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        str="□";
                        break;
                    case 1:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        str="©";
                        break;
                    case 2:
                        Console.ForegroundColor = ConsoleColor.Green;
                        str="★";
                        break;
                    case 3:
                        Console.ForegroundColor = ConsoleColor.Gray;
                        str="▲";
                        break;
                    case 4:
                        Console.ForegroundColor = ConsoleColor.Red;
                        str="卍";
                        break;

                }

            }
            return str;
        }

        public static void ChangePos()//超出坐标的时候
        {
            if (PlayerPos[0] < 0)
            {
                PlayerPos[0] = 0;
            }
            if (PlayerPos[1] < 0)
            {
                PlayerPos[1] = 0; 
            }
            if (PlayerPos[0] >= 99)
            {
                PlayerPos[0] = 99;
            }
            if (PlayerPos[1] >= 99)
            {
                PlayerPos[1] = 99;
            }


        }

    }
}
