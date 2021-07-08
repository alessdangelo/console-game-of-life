//Author : alessdangelo
//Date : 07/07/2021
//Description : Quick console implementation of conway's game of life with borders
//ToDo: Add a menu under the game panel to tell which boutons can be used and some other informations
//ToDo: Add a method to generate random cell state
//ToDo: Add a button to play the game automaticaly and a button to stop (playing state is off)
using System;

namespace console_game_of_life
{
    class Program
    {
        /// <summary>
        /// Size of the grid
        /// </summary>
        private static int _gridSize = 32;

        /// <summary>
        /// Grid to store the cells
        /// </summary>
        private static int[,] _cellGrid;

        /// <summary>
        /// Grid to store the next cells (next turn)
        /// </summary>
        private static int[,] _nextCellGrid;

        /// <summary>
        /// Position of the current cell
        /// </summary>
        private static int currentCellPosX, currentCellPosY;

        /// <summary>
        /// Number of cell alives around the current one
        /// </summary>
        private static int aliveCells = 0;

        /// <summary>
        /// x position of the cursor
        /// </summary>
        private static int cursorPosX = 0;

        /// <summary>
        /// y position of the cursor
        /// </summary>
        private static int cursorPosY = 0;

        /// <summary>
        /// Play state of the game. False = not playing
        /// </summary>
        private static bool startTurn = false;

        static void Main(string[] args)
        {
            Console.SetWindowSize(_gridSize * 2, _gridSize + 5);
            GenerateGrid();
            WriteGrid();
            Console.SetCursorPosition(0, 0);

            while (true)
            {
                Console.SetCursorPosition(cursorPosX, cursorPosY);
                Move();
                if (startTurn)
                {
                    SingleTurn();
                    WriteGrid();
                    Console.SetCursorPosition(cursorPosX, cursorPosY);
                    startTurn = false;
                }
            }
        }


        /// <summary>
        /// Move in the grid, initiate next turn and or turn a cell Dead or alive
        /// </summary>
        private static void Move()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            switch (keyInfo.Key)
            {
                //Move to the left
                case ConsoleKey.LeftArrow:
                case ConsoleKey.A:
                    if (cursorPosX > 0)
                    {
                        Console.SetCursorPosition(cursorPosX -= 2, cursorPosY);
                    }
                    break;
                //Move to the right
                case ConsoleKey.RightArrow:
                case ConsoleKey.D:
                    if (cursorPosX < _gridSize * 2 - 2)
                    {
                        Console.SetCursorPosition(cursorPosX += 2, cursorPosY);
                    }
                    break;
                //Move downwards
                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    if (cursorPosY < _gridSize - 1)
                    {
                        Console.SetCursorPosition(cursorPosX, cursorPosY += 1);
                    }
                    break;
                //Move upwards
                case ConsoleKey.UpArrow:
                case ConsoleKey.W:
                    if (cursorPosY > 0)
                    {
                        Console.SetCursorPosition(cursorPosX, cursorPosY -= 1);
                    }
                    break;
                //Toggle the current cell state (0 = dead, 1 = alive)
                case ConsoleKey.Spacebar:
                    //Current cell is alive, turn it dead
                    if (_cellGrid[cursorPosX / 2, cursorPosY] == 1)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        _cellGrid[cursorPosX / 2, cursorPosY] = 0;
                    }
                    //Current cell is dead, turn it alive
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        _cellGrid[cursorPosX / 2, cursorPosY] = 1;
                    }
                    //Write the current cell and set the correct position to the cursor
                    Console.Write("  ");
                    Console.ResetColor();
                    if (cursorPosX > 0 && cursorPosX < _gridSize)
                    {
                        Console.SetCursorPosition(cursorPosX, cursorPosY);
                    }
                    break;
                case ConsoleKey.Enter:
                    startTurn = true;
                    break;
            }
        }

        /// <summary>
        /// Generate the grid of cells
        /// </summary>
        private static void GenerateGrid()
        {
            //Initiate the grid and fill it with 0 (Dead state of a cell)
            _cellGrid = new int[_gridSize, _gridSize];
            for (int y = 0; y < _gridSize; y++)
            {
                for (int x = 0; x < _gridSize; x++)
                {
                    _cellGrid[x, y] = 0;
                }
            }
        }

        /// <summary>
        /// Print the grid in the current state
        /// </summary>
        private static void WriteGrid()
        {
            Console.Clear();
            for (int y = 0; y < _gridSize; y++)
            {
                for (int x = 0; x < _gridSize; x++)
                {
                    if (_cellGrid[x, y] == 1)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                    }
                    Console.Write("  ");
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// A single game of life turn, check if 
        /// </summary>
        private static void SingleTurn()
        {
            _nextCellGrid = new int[_gridSize, _gridSize];
            //Loop through the 2d array
            for (int y = 0; y < _gridSize; y++)
            {
                for (int x = 0; x < _gridSize; x++)
                {
                    //Position of the current cell
                    currentCellPosX = x;
                    currentCellPosY = y;

                    //True if there's 2 or 3 cell alive surrounding the current one 
                    if (CanCellLive())
                    {
                        //2 cells alive, the current one stay alive next turn
                        if (aliveCells == 2)
                        {
                            _nextCellGrid[x, y] = _cellGrid[x, y];
                        }
                        //3 cells alive,  the  current one turns alive next turn if it was dead, stays alive if not
                        else if (aliveCells == 3)
                        {
                            _nextCellGrid[x, y] = 1;
                        }
                    }
                    //Set the state of the current cell to dead next turn
                    else
                    {
                        _nextCellGrid[x, y] = 0;
                    }
                }
            }
            IninitateNextTurn();
        }

        /// <summary>
        /// Method to check the neighbours of the current cell. It can live if there's 2 or 3 cell alive around
        /// </summary>
        /// <returns>True if the current cell can live, false if it cannot</returns>
        private static bool CanCellLive()
        {
            //Coordinates to go check if the neighbours cells are alive
            int[,] neighboursCoordinates = new int[,]
            {
               {-1, -1 },{-1, 0 },{-1, +1 },
               { 0, -1 },         { 0, +1 },
               {+1, -1 },{+1, 0 },{+1, +1 }
            };
            //Number of cells surronding the current one alive
            aliveCells = 0;

            // Go check if the neighbours are alive
            for (int y = 0; y < neighboursCoordinates.GetLength(0); y++)
            {
                //Position x and y of the cell we're currently checking in
                int possibleX = currentCellPosX + neighboursCoordinates[y, 0];
                int possibleY = currentCellPosY + neighboursCoordinates[y, 1];
                for (int x = 0; x < neighboursCoordinates.GetLength(1); x += 2)
                {
                    if (possibleX < _cellGrid.GetLength(1) && possibleX >= 0 && possibleY < _cellGrid.GetLength(0) && possibleY >= 0)
                    {
                        if (_cellGrid[possibleX, possibleY] == 1)
                        {
                            aliveCells += 1;
                        }
                    }
                }
            }

            //True if the number of cells alive is from 2 to 3
            if (aliveCells >= 2 && aliveCells <= 3)
            {
                return true;
            }
            else
            {
                //No cells alive in the neighbourhood, the state of the current cell is or will be dead
                return false;
            }
        }

        /// <summary>
        /// Prepare the next turn by assigning new cells from the next grid to the current one
        /// </summary>
        private static void IninitateNextTurn()
        {
            _cellGrid = _nextCellGrid;
        }
    }
}
