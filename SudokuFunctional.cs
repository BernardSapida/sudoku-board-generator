using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sudoku
{
	public class SudokuFunctional
	{
		public static void Main(string[] args)
		{
      // Soduku Fields
      int[,] sudokuBoard = new int[9,9];
      int[] firstRow = {1, 2, 3, 4, 5, 6, 7, 8, 9};
      int[] zeroValuesCoordinate = new int[2]; 
      int attempts = 0;

      /// <summary>
      ///   It takes the first row of the sudoku board and shuffles it using the Fisher-Yates shuffle algorithm
      ///   The first thing I do is create a new randomizer object. This is used to shuffle the first row.
      ///   Next, I create an array of integers that represents the first row of the sudoku board.
      ///   Then, I use the randomizer object to shuffle the first row.
      ///   Then, I copy the shuffled first row back into the sudoku board
      ///   Finally, I increment the number of attempts by 9 since there are 9 numbers in a row
      /// </summary>
      void shuffleFirstRow() 
      {
        var randomizer = new Random();
        var shuffledArray = firstRow.OrderBy(e => randomizer.NextDouble()).ToArray();

        for(int i = 0; i < shuffledArray.Length; i++) sudokuBoard[0, i] = shuffledArray[i];
        attempts += 9;
      }

      /// <summary>
      ///   If the current board state has no empty spots, return true. Otherwise, get the coordinates of the
      ///   first empty spot, and try to fill it with a number from 1 to 9. If the number is valid,
      ///   recursively call the function again with the new board state. If the function returns true, return
      ///   true. Otherwise, reset the current board state to 0 and try the next number. If all numbers have
      ///   been tried and none of them worked, return false
      /// </summary>
      /// <param name="currentBoardState">The current state of the board.</param>
      /// <returns>
      ///   a boolean value.
      /// </returns>
      bool generateSudokuBoard(int[,] currentBoardState) 
      {
        int xAxis, yAxis;

        zeroValuesCoordinate = getEmptySpotCoordinate(currentBoardState);

        if(Enumerable.SequenceEqual(zeroValuesCoordinate, new int[] {404, 404})) return true;

        xAxis = zeroValuesCoordinate[0];
        yAxis = zeroValuesCoordinate[1];
        
        for(int number = 1; number < 10; number++)
        {
          if(validateCoordinate(currentBoardState, number, new int[] {xAxis, yAxis}))
          {
            currentBoardState[xAxis, yAxis] = number;
            if(generateSudokuBoard(currentBoardState)) return true;
            currentBoardState[xAxis, yAxis] = 0;
          }
        }

        return false;
      }

      /// <summary>
      ///   It checks if the number is valid in the row, column, and 3x3 grid.
      /// </summary>
      /// <param name="currentBoardState">The current state of the board.</param>
      /// <param name="number">The number that is being tested</param>
      /// <param name="coordinates">The coordinates of the cell you want to validate.</param>
      /// <returns>
      ///   The number of attempts it took to solve the puzzle.
      /// </returns>
      bool validateCoordinate(int[,] currentBoardState, int number, int[] coordinates) 
      {
        int boardXX = coordinates[1] / 3;
        int boardXY = coordinates[0] / 3;

        attempts++;

        for(int xCoordinate = 0; xCoordinate < 9; xCoordinate++) 
        {
          if (currentBoardState[coordinates[0], xCoordinate] == number && coordinates[1] != xCoordinate) return false;
        }

        for(int yCoordinate = 0; yCoordinate < 9; yCoordinate++) 
        {
          if (currentBoardState[yCoordinate, coordinates[1]] == number && coordinates[0] != yCoordinate) return false;
        }

        for(int xy = boardXY*3; xy < boardXY*3 + 3; xy++) 
        {
          for(int xx = boardXX * 3; xx < boardXX*3 + 3; xx++) 
          {
            if(currentBoardState[xy, xx] == number && coordinates != new int[] {xy, xx}) return false;
          }
        }

        return true;
      }

      /// <summary>
      ///   It prints the sudoku board to the console.
      /// </summary>
      /// <returns>
      ///   The method is returning a boolean value.
      /// </returns>
      bool printBoard(int[,] board) 
      {
        String sudokuBoard = "";
    
        for(int yCoordinate = 0; yCoordinate < 9; yCoordinate++) 
        {
          if(yCoordinate == 3 || yCoordinate == 6) Console.WriteLine("--------------------------------");
          
          for(int xCoordinate = 0; xCoordinate < 9; xCoordinate++) 
          {
            sudokuBoard += (" " + board[yCoordinate, xCoordinate] + " ");
            
            if(xCoordinate == 2 || xCoordinate == 5) sudokuBoard += " | ";
            if(xCoordinate == 8) 
            {
              Console.WriteLine(sudokuBoard);
              sudokuBoard = "";
            }
          }
        }

        return false;
      }

      /// <summary>
      ///   It returns the coordinates of the first empty spot it finds in the board.
      /// </summary>
      /// <param name="currentBoardState">The current state of the board.</param>
      /// <returns>
      ///   an array of two integers. The first integer is the y coordinate of the empty spot and the second
      ///   integer is the x coordinate of the empty spot.
      /// </returns>
      int[] getEmptySpotCoordinate(int[,] currentBoardState) 
      {
        for(int yCoordinate = 0; yCoordinate < 9; yCoordinate++)
        {
          for(int xCoordinate = 0; xCoordinate < 9; xCoordinate++)
          {
            if(currentBoardState[yCoordinate, xCoordinate] == 0) return new int[] {yCoordinate, xCoordinate};
          }
        }
        return new int[] {404, 404};
      }

      /// <summary>
      ///   I check if the sum of each row, column, and box is equal to 45.
      /// </summary>
      /// <param name="board">a 2D array of integers</param>
      /// <returns>
      ///   A boolean value.
      /// </returns>
      bool isValidSudokuBoard(int[,] board) 
      {
        int[] xCoordinate = new int[9];
        int[] yCoordinate = new int[9];
        int[,] eachBox = new int[3, 3];

        for (var i = 0; i < 9; i++)
        {
          for (var j = 0; j < 9; j++)
          {
            xCoordinate[i] += board[i, j];
            yCoordinate[j] += board[i, j];
            eachBox[i/3, j/3] += board[i, j];
          }
        }

        for (int i=0; i < 3; i++) 
        {
          for (int j=0; j < 3; j++) 
          {
            if(eachBox[i, j] != 45) return false;
          }
        }

        for (int i=0; i < 9; i++) if(xCoordinate[i] != 45) return false;
        for (int i=0; i < 9; i++) if(yCoordinate[i] != 45) return false;

        return true;
      }

      SudokuFunctional sudoku = new SudokuFunctional();
      shuffleFirstRow();
      generateSudokuBoard(sudokuBoard);
      Console.WriteLine("\nProgram took " + attempts + " attempts to generate sudoku board:\n");
      printBoard(sudokuBoard);
      Console.WriteLine("\nValid Board: " + isValidSudokuBoard(sudokuBoard));
    }
  }
}