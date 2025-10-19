using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGame.Mechanics
{
    public static class SudokuGenerator
    {
        // Returns false if given 3x3 block contains num
        // Ensure the number is not used in the box
        static bool unUsedInBox(int[,] grid, int rowStart, int colStart, int num) {
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    if (grid[rowStart + i, colStart + j] == num) {
                        return false;
                    }
                }
            }
            return true;
        }

        // Fill a 3x3 matrix
        // Assign valid random numbers to the 3x3 subgrid
        static void fillBox(int[,] grid, int row, int col) {
            Random rand = new Random();
            int num;
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    do {
                        // Generate a random number between 1 and 9
                        num = rand.Next(1, 10);
                    } while (!unUsedInBox(grid, row, col, num));
                    grid[row + i, col + j] = num;
                }
            }
        }

        // Check if it's safe to put num in row i
        // Ensure num is not already used in the row
        static bool unUsedInRow(int[,] grid, int i, int num) {
            for (int j = 0; j < 9; j++) {
                if (grid[i, j] == num) {
                    return false;
                }
            }
            return true;
        }

        // Check if it's safe to put num in column j
        // Ensure num is not already used in the column
        static bool unUsedInCol(int[,] grid, int j, int num) {
            for (int i = 0; i < 9; i++) {
                if (grid[i, j] == num) {
                    return false;
                }
            }
            return true;
        }

        // Check if it's safe to put num in the cell (i, j)
        // Ensure num is not used in row, column, or box
        static bool checkIfSafe(int[,] grid, int i, int j, int num) {
            return (unUsedInRow(grid, i, num) && unUsedInCol(grid, j, num) &&
                    unUsedInBox(grid, i - i % 3, j - j % 3, num));
        }

        // Fill the diagonal 3x3 matrices
        // The diagonal blocks are filled to simplify the process
        static void fillDiagonal(int[,] grid) {
            for (int i = 0; i < 9; i += 3) {
                // Fill each 3x3 subgrid diagonally
                fillBox(grid, i, i);
            }
        }

        // Fill remaining blocks in the grid
        // Recursively fill the remaining cells with valid numbers
        static bool fillRemaining(int[,] grid, int i, int j) {
            if (i == 9) {
                return true;
            }
            if (j == 9) {
                return fillRemaining(grid, i + 1, 0);
            }
            if (grid[i, j] != 0) {
                return fillRemaining(grid, i, j + 1);
            }
            for (int num = 1; num <= 9; num++) {
                if (checkIfSafe(grid, i, j, num)) {
                    grid[i, j] = num;
                    if (fillRemaining(grid, i, j + 1)) {
                        return true;
                    }
                    grid[i, j] = 0;
                }
            }
            return false;
        }

        // Remove K digits randomly from the grid
        // This will create a Sudoku puzzle by removing digits
        static void removeKDigits(int[,] grid, int k) {
            Random rand = new Random();
            while (k > 0) {
                int cellId = rand.Next(81);
                int i = cellId / 9;
                int j = cellId % 9;
                if (grid[i, j] != 0) {
                    grid[i, j] = 0;
                    k--;
                }
            }
        }

        // Generate a Sudoku grid with K empty cells
        public static int[,] sudokuGenerator(int k) {
            int[,] grid = new int[9, 9];
            fillDiagonal(grid);
            fillRemaining(grid, 0, 0);
            removeKDigits(grid, k);
            return grid;
        }
    }
}
