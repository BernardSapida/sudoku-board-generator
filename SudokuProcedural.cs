using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sudoku
{
	public class SudokuProcedural
	{
		public static void Main(string[] args)
		{
      // Soduku Fields
      int[,] sudokuBoard = new int[9,9];
      int[] firstRow = {7,8,3,6,2,9,1,5,4};
      int[,] recordOfValidNumbers = new int[9,9];
      int attempts = 0;

      /* Shuffling the first row of the sudoku board. */
      Random randomizer = new Random();
      int[] shuffledArray = firstRow.OrderBy(e => randomizer.NextDouble()).ToArray();
      for(int i = 0; i < firstRow.Length; i++) sudokuBoard[0, i] = shuffledArray[i];
      attempts += 9;
      
      /* Generating the board. */
      while(true)
      {
        int row, column;
        int[] zeroValuesCoordinate = new int[] {-1, -1};
        
        // FIND EMPTY SPOT START
        for(int yCoordinate = 0; yCoordinate < 9; yCoordinate++)
        {
          if(zeroValuesCoordinate[0] != -1 && zeroValuesCoordinate[1] != -1) break;

          for(int xCoordinate = 0; xCoordinate < 9; xCoordinate++)
          {
            if(sudokuBoard[yCoordinate, xCoordinate] == 0) {
              zeroValuesCoordinate = new int[] {yCoordinate, xCoordinate};
              break;
            }
          } 
        }
        
        if(zeroValuesCoordinate[0] == -1 && zeroValuesCoordinate[1] == -1) zeroValuesCoordinate = new int[] {404, 404};
        // FIND EMPTY SPOT END

        if(zeroValuesCoordinate[0] == 404 && zeroValuesCoordinate[1] == 404) goto printAttempts;

        row = zeroValuesCoordinate[0];
        column = zeroValuesCoordinate[1];

        for(int number = recordOfValidNumbers[row, column]+1; number < 10; number++)
        {
          // VALIDATE COORDINATE START
          int boardXY = row / 3;
          int boardXX = column / 3;
          String isValidCoordinate = "true";

          attempts++;

          for(int xCoords = 0; xCoords < 9; xCoords++) 
          {
            if(sudokuBoard[row, xCoords] == number && column != xCoords) {
              isValidCoordinate = "false";
              goto insertion;
            }
          }

          for(int yCoords = 0; yCoords < 9; yCoords++) 
          {
            if(sudokuBoard[yCoords, column] == number && row != yCoords) {
              isValidCoordinate = "false";
              goto insertion;
            }
          }

          for(int xy = boardXY*3; xy < boardXY*3 + 3; xy++) 
          {
            for(int xx = boardXX * 3; xx < boardXX*3 + 3; xx++) 
            {
              if(sudokuBoard[xy, xx] == number && new int[] {row, column} != new int[] {xy, xx}) {
                isValidCoordinate = "false";
                goto insertion;
              }
            }
          }
          // VALIDATE COORDINATE END

          insertion:
            if(isValidCoordinate != "false")
            {
              sudokuBoard[row, column] = number;
              recordOfValidNumbers[row, column] = number;
              break;
            }
        }

        if(sudokuBoard[row, column] == 0 && column > 0) {
          sudokuBoard[row, column-1] = 0;
          recordOfValidNumbers[row, column] = 0;
        }
      }

      printAttempts:
        Console.WriteLine("\nProgram took " + attempts + " attempts to generate sudoku board:");

      /* Printing the board. */
      String sudokuBoardString = "\n";
      for(int yCoordinate = 0; yCoordinate < 9; yCoordinate++) 
      {
        if(yCoordinate == 3 || yCoordinate == 6) Console.WriteLine("--------------------------------");
        
        for(int xCoordinate = 0; xCoordinate < 9; xCoordinate++) 
        {
          sudokuBoardString += (" " + sudokuBoard[yCoordinate, xCoordinate] + " ");
          
          if(xCoordinate == 2 || xCoordinate == 5) sudokuBoardString += " | ";
          if(xCoordinate == 8) 
          {
            Console.WriteLine(sudokuBoardString);
            sudokuBoardString = "";
          }
        }
      }
    }
  }
}