using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOfLife
{
    class ProcessInput
    {
        //Set x,y value for 25x25 Grid
        protected const int x = 25;
        protected const int y = 25;

        protected int InterestedGeneration;//To get interested generation from user
        protected string UserInputPosition;//To get positions from user
        protected int[,] InitialGrid = new int[x, y];//To generate a grid based on given user inputs

        public string[] GetInput()
        {
            Console.WriteLine("Which generation's population positions are you interested in?");
            InterestedGeneration = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Input the population position of generation ZERO.\nPlease give the input seporated by colon(:) in (x,y) format (eg - 2,1:2,2:2,3");
            UserInputPosition = Console.ReadLine();
            string[] lst = UserInputPosition.Trim().Split(':');
            Console.WriteLine("Given inputs are :");
            foreach (string single in lst)
            {
                Console.WriteLine(single);
            }
            return lst;
        }

        public void Processtogrid(string[] lst)
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    for (int k = 0; k < lst.Count(); k++)
                    {
                        if (i.ToString() + "," + j.ToString() == lst[k].ToString())
                        {
                            InitialGrid[i, j] = 1;
                        }
                    }
                }
            }
        }

        public virtual void ShowGrid(int[,] InitialGrid)
        {
            Console.WriteLine("\nInput Grid");
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    Console.Write(InitialGrid[i, j].ToString() + " ");
                }
                Console.Write("\n");
            }
        }
    }
    class ProcessGeneration : ProcessInput
    {
        public int[,] Generationcheck()
        {
            int[,] finalgrid = new int[x, y];
            int gen = InterestedGeneration + 1;
            List<int[,]> CurrentGenerationGrid = new List<int[,]>();
            for (int jg = 0; jg < gen; jg++)
            {
                CurrentGenerationGrid.Add(new int[x, y]);
            }
            Array.Copy(InitialGrid, CurrentGenerationGrid[0], InitialGrid.Length);//Copying initial grid to process
            for (int ja = 1; ja < CurrentGenerationGrid.Count(); ja++)
            {
                Array.Copy(CurrentGenerationGrid[ja - 1], CurrentGenerationGrid[ja], CurrentGenerationGrid[ja - 1].Length);//copying previous generation grid to next generation
                for (int i = 1; i < x - 1; i++)
                {
                    for (int j = 1; j < y - 1; j++)
                    {
                        int aliveNeighbors = 0;
                        for (int p = -1; p <= 1; p++)
                        {
                            for (int n = -1; n <= 1; n++)
                            {
                                aliveNeighbors += CurrentGenerationGrid[ja - 1][i + p, j + n] == 1 ? 1 : 0;
                            }
                        }
                        int currentcell = CurrentGenerationGrid[ja - 1][i, j];

                        aliveNeighbors -= currentcell == 1 ? 1 : 0;

                        if (currentcell == 1 && aliveNeighbors < 2)//If live cell with lesser than 2 live neighbours will die due to under population
                        {
                            CurrentGenerationGrid[ja][i, j] = 0;
                        }
                        else if (currentcell == 1 && aliveNeighbors > 3)//If live cell with greater than 3 live neighbours will die due to over population
                        {
                            CurrentGenerationGrid[ja][i, j] = 0;
                        }
                        else if (currentcell == 0 && aliveNeighbors == 3)//If a dead cell with exactly 3 neighbours will come to life
                        {
                            CurrentGenerationGrid[ja][i, j] = 1;
                        }
                        else//other wise unchanged to next generation
                        {
                            CurrentGenerationGrid[ja][i, j] = currentcell;
                        }
                    }
                }
                if (ja == CurrentGenerationGrid.Count() - 1)//If all generation run is completed will move the values to final grid
                    Array.Copy(CurrentGenerationGrid[ja], finalgrid, CurrentGenerationGrid[ja].Length);
            }
            return finalgrid;
        }

        public override void ShowGrid(int[,] finalgrid)
        {
            Console.WriteLine("\nOutput Grid");
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    Console.Write(finalgrid[i, j].ToString() + " ");
                }
                Console.Write("\n");
            }
        }

        public void ProcessOutput(int[,] finalgrid)
        {
            this.ShowGrid(finalgrid);
            Console.WriteLine("\nList of positions of living population in generation " + InterestedGeneration + " is:");
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (finalgrid[i, j] == 1)
                        Console.WriteLine("(" + i.ToString() + "," + j.ToString() + ")");
                }
            }
        }
    }

    class Program 
    {
        ProcessGeneration obj = new ProcessGeneration();

        static void Main(string[] args)
        {
            try
            {
                Program ObjPrgm = new Program();
                string[] lst = ObjPrgm.GetGameInput();
                int[,] finalgrid = ObjPrgm.StartGame(lst);
                ObjPrgm.ShowGameOutput(finalgrid);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            finally
            {
                Console.WriteLine("\nPressing any key will exit the window...");
                Console.ReadLine();
            }
        }
        private string[] GetGameInput()
        {
            return obj.GetInput();//Getting inputs
        }

        private int[,] StartGame(string[] lst)
        {
            obj.Processtogrid(lst);//Processing input and generate grid
            return obj.Generationcheck();//Run generation game
        }

        private void ShowGameOutput(int[,] finalgrid)
        {
            obj.ShowGrid(finalgrid);//Display output
            obj.ProcessOutput(finalgrid);//Display output
        }
        
    }
}