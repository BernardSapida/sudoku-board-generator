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
      int[] zeroValuesCoordinate = new int[2]; 
      int attempts = 0;

      void shuffleFirstRow() 
      {
        var randomizer = new Random();
        int[] firstRow = {1, 2, 3, 4, 5, 6, 7, 8, 9};
        var shuffledArray = firstRow.OrderBy(e => randomizer.NextDouble()).ToArray();
        for(int i = 0; i < shuffledArray.Length; i++) sudokuBoard[0, i] = shuffledArray[i];
        attempts += 9;
      }

      bool generateSudokuBoard(int[,] currentBoardState) 
      {
        int xAxis, yAxis;

        zeroValuesCoordinate = getEmptySpotCoordinate(currentBoardState);

        if(Enumerable.SequenceEqual(zeroValuesCoordinate, new int[] {404, 404})) return true;

        xAxis = zeroValuesCoordinate[0];
        yAxis = zeroValuesCoordinate[1];
        
        for(int number = 1; number < 10; number++) {
          if(validateCoordinate(currentBoardState, number, new int[] {xAxis, yAxis})) {
            currentBoardState[xAxis, yAxis] = number;

            if(generateSudokuBoard(currentBoardState)) return true;

            currentBoardState[xAxis, yAxis] = 0;
          }
        }

        return false;
      }

      bool validateCoordinate(int[,] currentBoardState, int number, int[] coordinates) 
      {
        int boardXX = coordinates[1] / 3;
        int boardXY = coordinates[0] / 3;

        attempts++;

        for(int xCoordinate = 0; xCoordinate < 9; xCoordinate++) {
          if (currentBoardState[coordinates[0], xCoordinate] == number && coordinates[1] != xCoordinate) {
            return false;
          }
        }

        for(int yCoordinate = 0; yCoordinate < 9; yCoordinate++) {
          if (currentBoardState[yCoordinate, coordinates[1]] == number && coordinates[0] != yCoordinate) {
            return false;
          }
        }

        for(int xy = boardXY*3; xy < boardXY*3 + 3; xy++) {
          for(int xx = boardXX * 3; xx < boardXX*3 + 3; xx++) {
            if(currentBoardState[xy, xx] == number && coordinates != new int[] {xy, xx}) {
              return false;
            }
          }
        }

        return true;
      }

      bool printBoard(int[,] board) 
      {
        String sudokuBoard = "";
    
        for(int yCoordinate = 0; yCoordinate < 9; yCoordinate++) {
          if(yCoordinate == 3 || yCoordinate == 6) Console.WriteLine("--------------------------------");
          
          for(int xCoordinate = 0; xCoordinate < 9; xCoordinate++) {
            sudokuBoard += (" " + board[yCoordinate, xCoordinate] + " ");
            
            if(xCoordinate == 2 || xCoordinate == 5) sudokuBoard += " | ";
            if(xCoordinate == 8) {
              Console.WriteLine(sudokuBoard);
              sudokuBoard = "";
            }
          }
        }

        return false;
      }

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

      bool isValidSudokuBoard(int[,] board) 
      {
        int[] xCoordinate = new int[9];
        int[] yCoordinate = new int[9];
        int[,] eachBox = new int[3, 3];

        for (var i = 0; i < 9; i++){
          for (var j = 0; j < 9; j++){
            xCoordinate[i] += board[i, j];
            yCoordinate[j] += board[i, j];
            eachBox[i/3, j/3] += board[i, j];
          }
        }

        for (int i=0; i < 3; i++) {
          for (int j=0; j < 3; j++) {
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