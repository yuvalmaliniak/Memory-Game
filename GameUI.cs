using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using Ex02.ConsoleUtils;

namespace Ex02
{
    internal class GameUI
    {
        private GameLogic<char> m_GameLogic;
        public GameUI()
        {
            Player[] players = PlayerSetup();
            Board<char> board = BoardSetup();
            m_GameLogic = new GameLogic<char>(board, players);
        }
        public void Play()
        {
            while (!m_GameLogic.isGameOver)
            {
                playTurn();
                m_GameLogic.isGameOver = m_GameLogic.Board.CheckIfAllRevealed();
            }
            EndMessage();
        }
        private void playTurn()
        {
            Player player = m_GameLogic.GetCurrentPlayer();
            string playerFirstTurn = "", playerSecondTurn = "";
            m_GameLogic.Board.Draw(); // Draw the board
            playerFirstTurn = getMove(1);
            (int firstRow, int firstColumn) = convertInputToCoordinates(playerFirstTurn);
            m_GameLogic.Board.RevealCell(firstRow, firstColumn);
            m_GameLogic.Board.Draw(); // Draw the board
            playerSecondTurn = getMove(2);
            while (playerSecondTurn.Equals(playerFirstTurn))
            {
                playerSecondTurn = getMove(2);
            }
            (int secondRow, int secondColumn) = convertInputToCoordinates(playerSecondTurn);
            m_GameLogic.Board.RevealCell(secondRow, secondColumn);
            m_GameLogic.Board.Draw(); // Draw the board
            //Console.WriteLine($"The user {player.PlayerName} chose the move {PlayerFirstTurn} and the move {PlayerSecondTurn}.");
            bool isEqual = m_GameLogic.CompleteTurn(firstRow, firstColumn, secondRow, secondColumn);
            m_GameLogic.Board.Draw(); // Draw the board after the hit
            if (isEqual)
            {
                Console.WriteLine("Boom! Nice hit.");
            }
            else
            {
                System.Threading.Thread.Sleep(2000);
                m_GameLogic.FlipNotEqual(firstRow, firstColumn, secondRow, secondColumn);
                m_GameLogic.Board.Draw();
                Console.WriteLine("Not a hit.");
                m_GameLogic.PlayingPlayer = 1 - m_GameLogic.PlayingPlayer; // Use game management to change to the other player
            }
        }
        private string getMove(int i_turnNumber)
        {
            Player player = m_GameLogic.GetCurrentPlayer();
            string playerTurn = "";
            if (player.IsHuman)
            {
                string numOfMove = i_turnNumber == 1 ? "1st" : "2nd";
                Console.WriteLine($"Hi {player.PlayerName}, Enter your {numOfMove} cell choice (for example: B4)");
                playerTurn = Console.ReadLine();
                bool validInput = false;
                while (!validInput)
                {
                    if (playerTurn.Equals("Q"))
                    {
                        quitGame();

                    }
                    else if(!validateSyntaxChoice(playerTurn))
                    {
                        Console.WriteLine("Your move is syntax invalid. Enter a correct move (for example: B4)");
                        playerTurn = Console.ReadLine();
                    }
                    else if (!validateLogicalChoice(playerTurn))
                    {
                        Console.WriteLine("Your move is logical invalid, make sure to not exceed bondaries. Enter a correct move within the row/column boundaries.");
                        playerTurn = Console.ReadLine();
                    }
                    else if (validateCellAlreadyRevealed(playerTurn))
                    {
                        Console.WriteLine("Your move is logical invalid, You chose a cell that was already revealed. Please choose other cell");
                        playerTurn = Console.ReadLine();
                    }
                    else
                    {
                        validInput = true;
                    }
                }  
            }
            else
            {
                playerTurn = m_GameLogic.GetAsStringRandomMove();
            }
            return playerTurn;
        }
        private bool validateCellAlreadyRevealed(string i_playerChoice)
        {
            (int row, int column) = convertInputToCoordinates(i_playerChoice);
            return m_GameLogic.IsCardRevealed(row, column);
        }
        static (int, int) convertInputToCoordinates(string i_input)
        {
            int column = i_input[0] - 'A';
            int row = int.Parse(i_input.Substring(1)) - 1;
            return (row, column);
        }
        private bool validateSyntaxChoice(string i_playerChoice)
        {
            bool isValid = true;
            // Ensure the input is exactly two characters long
            if (i_playerChoice.Length != 2)
            {
                isValid = false;
            }
            if (isValid)
            {
                char rowChar = i_playerChoice[1];
                char colChar = i_playerChoice[0];
                if (rowChar < '1' || !char.IsDigit(rowChar) || colChar < 'A' || colChar > 'Z')
                {
                    isValid = false;
                }
            }
            return isValid;
        }
        private bool validateLogicalChoice(string i_playerChoice)
        {
            int boardRows = m_GameLogic.Board.Rows;
            int boardCol = m_GameLogic.Board.Cols;
            char rowChar = i_playerChoice[1];
            char colChar = i_playerChoice[0];
            return rowChar >= '1' && rowChar <= (char)('0' + boardRows) && colChar >= 'A' && colChar <= (char)('A' + boardCol - 1);
        }
        public Player[] PlayerSetup()
        {
            string firstPlayerName = "";
            Console.WriteLine("Please enter the 1st username. Don't use whitespaces, maximum 20 characters");
            firstPlayerName = Console.ReadLine();
            while (!validatePlayerName(firstPlayerName))
            {
                Console.WriteLine("Please enter a valid username. Don't use whitespaces, maximum 20 characters");
                firstPlayerName = Console.ReadLine();
            }
            Player playerOne = new Player(firstPlayerName, true);
            Player playerTwo = null;
            Console.WriteLine("Please choose your opponent, Press 'C' to play against the computer or 'H' for 2 players mode");
            string userChoose = Console.ReadLine();
            while (!validateModeSelect(userChoose))
            {
                Console.WriteLine("Invalid chooise, Please try again. Press 'C' to play against the computer or 'H' for 2 player mode");
                userChoose = Console.ReadLine();
            }
            if (userChoose.Equals("C"))
            {
                playerTwo = new Player("Computer", false);
            }
            else
            {
                Console.WriteLine("Please enter the 2nd username. Don't use whitespaces, maximum 20 characters");
                string secondPlayerName = Console.ReadLine();
                while (!validatePlayerName(secondPlayerName))
                {
                    Console.WriteLine("Please enter a valid username. Don't use whitespaces, maximum 20 characters");
                    secondPlayerName = Console.ReadLine();
                }
                playerTwo = new Player(secondPlayerName, true);
            }
            Player[] playersArray = { playerOne, playerTwo };
            return playersArray;
        }
        private static bool validatePlayerName(string i_PlayerName)
        {
            if (i_PlayerName.Equals("Q"))
            {
                quitGame();

            }

            return !i_PlayerName.Any(char.IsWhiteSpace) && i_PlayerName.Length > 0 && i_PlayerName.Length <= 20;
        }
        private static bool validateModeSelect(string i_Mode)
        {
            if (i_Mode.Equals("Q"))
            {
                quitGame();

            }
            bool modeIsValid = false;
            if (i_Mode.Equals("C") || i_Mode.Equals("H"))
            {
                modeIsValid = true;
            }
            return modeIsValid;
        }
        public Board<char> BoardSetup()
        {
            string rowSize, colSize;
            int rowsCount = 0, colsCount = 0;
            bool validCombination = false;
            while (!validCombination)
            {
                Console.WriteLine("Please enter a valid number between 4-6 for the rows side.");
                rowSize = Console.ReadLine();
                while (!validateBoardSize(rowSize))
                {
                    Console.WriteLine("This is not a valid number. Please enter a valid number between 4-6 for the rows side");
                    rowSize = Console.ReadLine();
                }
                Console.WriteLine("Please enter a valid number between 4-6 for the cols side.");
                colSize = Console.ReadLine();
                while (!validateBoardSize(colSize))
                {
                    Console.WriteLine("This is not a valid number. Please enter a valid number between 4-6 for the cols side");
                    colSize = Console.ReadLine();
                }
                rowsCount = int.Parse(rowSize);
                colsCount = int.Parse(colSize);
                if (rowsCount * colsCount % 2 == 0)
                {
                    validCombination = true;
                }
                else
                {
                    Console.WriteLine("This is not a valid combination. Please do not enter 5 for rows and 5 for column as it's odd.");
                }
            }
            char[] cardValues = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            Board<char> board = new Board<char>(rowsCount, colsCount, cardValues);
            return board;
        }
        private static bool validateBoardSize(string i_Input)
        {
            if (i_Input.Equals("Q"))
            {
                quitGame();
            }
            bool isValid = true;
            if (int.TryParse(i_Input, out int boardSize))
            {
                // Check if size is between 4 and 6 inclusive
                isValid = boardSize >= 2 && boardSize <= 6;
            }
            else
            {
                // Input is not a valid integer
                isValid = false;
            }
            return isValid;
        }
        public void EndMessage()
        {
            foreach (Player player in m_GameLogic.GetPlayers)
            {
                Console.WriteLine($"Player {player.PlayerName} has a score of {player.UserPoints}.");
            }
            // Let the user decide if to continue or end the game.
            Console.WriteLine("For new Game please press 'N', press 'Q' to quit");
            char userChoose = Console.ReadKey().KeyChar;
            while (userChoose != 'N' && userChoose != 'Q')
            {
                Console.WriteLine("Invalid chooise, Please try again. Press 'N' to play again or 'Q' for quit");
                userChoose = Console.ReadKey().KeyChar;
            }
            if (userChoose == 'N')
            {
                Screen.Clear();
                Board<char> newBoard = BoardSetup();
                m_GameLogic.Board = newBoard;
                Screen.Clear();
                m_GameLogic.isGameOver = false;
                m_GameLogic.PlayingPlayer = 0;
                foreach(Player player in m_GameLogic.GetPlayers)
                {
                    player.UserPoints = 0;
                }
                Play();
            }
            else
            {
                quitGame();
            }
        }
        private static void quitGame()
        {
            Console.WriteLine("Thanks for playing, bye");
            Environment.Exit(0); 
        }
        private static void getMoveFromPlayer(Player player)
        {
            Console.WriteLine("Enter your move (for example: B4)");
            string PlayerChoice = Console.ReadLine();
        }
    }
}