using System.Threading.Tasks;
using SudokuGame.Mechanics;

namespace SudokuGame.Pages;

public partial class GamePlay : ContentPage
{
    // Game Stats
    private int total_wins = 0;
    private int current_streak = 0;

    // Game modes
    private bool noteMode = false;

    // Moves
    private int maxMoves;
    private int moves;
    private int mistakes;

    // Colors
    private Color safeColorText = Color.FromArgb("1d4fb1");
    private Color selectGrid = Color.FromArgb("2458be");

    private Color numberExisted = Color.FromArgb("8bb0ff");
    private Color notSafe = Color.FromArgb("cc3256");

    private Color selectedRowColumn = Color.FromArgb("f2f1f6");


    // Boards properties
    private Button[,] gridButtons = new Button[9, 9];

    private int[,] answerMat = new int[9, 9];
    private int[,] mat;

    // Currents
    private Button? currGrid = null;
    private (int, int) lastGridIndex;

    public GamePlay()
    {
        InitializeComponent();
    }

    public void GenerateBoard(int k)
    {
        // Map the buttons in 2D array to modify them on the go.
        mat = SudokuGenerator.sudokuGenerator(k);
        Buffer.BlockCopy(mat, 0, answerMat, 0, answerMat.Length * sizeof(int));

        // reset moves
        maxMoves = k;
        moves = 0;
        mistakes = 0;

        // Set the labels properly
        lbl_Mistakes.Text = "Mistakes 0/3";
        lbl_CurrentStreak.Text = current_streak.ToString();
        lbl_TotalWins.Text = total_wins.ToString();

        MapButtons();
    }


    private void MapButtons()
    {
        // Map buttons to a 2d array.
        foreach (var child in SudokuGrid)
        {
            if (child is Border border && border.Content is Button button)
            {
                int row = Grid.GetRow(border);
                int col = Grid.GetColumn(border);

                gridButtons[row, col] = button;

                gridButtons[row, col].Text = mat[row, col] != 0 ? mat[row, col].ToString() : "";

                gridButtons[row, col].TextColor = Colors.Black;
                gridButtons[row, col].Background = Colors.White;
                gridButtons[row, col].BackgroundColor = Colors.White;
            }
        }
    }

    private bool IsValidBoard()
    {
        Dictionary<int, HashSet<int>> row = new();
        Dictionary<int, HashSet<int>> col = new();
        Dictionary<string, HashSet<int>> boxes = new();

        // row
        for (int i = 0; i < 9; i++)
        {
            // columns
            for (int j = 0; j < 9; j++)
            {
                int value = mat[i, j];
                if (value == 0) continue;

                string boxKey = $"{i / 3},{j / 3}";

                if (!row.TryGetValue(i, out var rowSet))
                {
                    row[i] = rowSet = new HashSet<int>();
                }

                if (!col.TryGetValue(j, out var colSet))
                {
                    col[j] = colSet = new HashSet<int>();
                }

                if (!boxes.TryGetValue(boxKey, out var boxSet))
                {
                    boxes[boxKey] = boxSet = new HashSet<int>();
                }

                if (rowSet.Contains(value) || colSet.Contains(value) || boxSet.Contains(value))
                    return false;

                rowSet.Add(value);
                colSet.Add(value);
                boxSet.Add(value);
            }
        }

        return true;
    }

    // Helper method
    private async Task ResetColorAfterDelay(Button button, Color color)
    {
        await Task.Delay(3000); // wait 3 seconds
        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            button.TextColor = Colors.Black;
        });
    }


    private bool IsSafe(int row, int col, int num)
    {
        bool safe = true;

        // Check duplicates on row
        for (int i = 0; i < 9; i++)
            if (i != col && answerMat[row, i] == num)
            {
                safe = false;
                gridButtons[row, i].TextColor = notSafe;

                if (mat[row, i] == num)
                {
                    _ = ResetColorAfterDelay(gridButtons[row, i], Colors.Black);
                }
            }

        // Check duplicates on column
        for (int i = 0; i < 9; i++)
            if (i != row && answerMat[i, col] == num)
            {
                safe = false;
                gridButtons[i, col].TextColor = notSafe;

                if (mat[i, col] == num)
                {
                    _ = ResetColorAfterDelay(gridButtons[i, col], Colors.Black);
                }
            }

        int startRow = row - (row % 3);
        int startColumn = col - (col % 3);
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if ((i + startRow) != row && (j + startColumn) != col && answerMat[i + startRow, j + startColumn] == num)
                {
                    safe = false;
                    gridButtons[i + startRow, j + startColumn].TextColor = notSafe;

                    if (answerMat[i + startRow, j + startColumn] == num)
                    {
                        _ = ResetColorAfterDelay(gridButtons[i + startRow, j + startColumn], Colors.Black);
                    }
                }
            }
        }

        return safe;
    }

    private void Numpad(object sender, EventArgs e)
    {
        if (currGrid is null) return;

        int number = int.Parse(((Button)sender).Text);
        int row = Grid.GetRow(currGrid.Parent);
        int col = Grid.GetColumn(currGrid.Parent);

        // If fixed number, skip
        if (mat[row, col] != 0) return;

        // set the text, then decide after if its safe or not
        currGrid.Text = number.ToString();
        currGrid.TextColor = Colors.White;

        if (noteMode)
        {
            currGrid.Background = Colors.Gray;
            currGrid.BackgroundColor = Colors.Gray;
            return;
        }

        if (!IsSafe(row, col, number))
        {
            currGrid.BackgroundColor = notSafe;
            currGrid.Background = notSafe;

            mistakes++;
            lbl_Mistakes.Text = "Mistakes: " + mistakes + "/3";
            if (mistakes == 3)
            {
                DisplayAlert("Game over", "you lose the game, try to do better next time.", "Okay");
                current_streak = 0;
                Navigation.PopAsync();
            }
        }
        else
        {
            currGrid.BackgroundColor = selectGrid;
            currGrid.Background = selectGrid;

            moves++;
        }

        answerMat[row, col] = number;

        if (moves >= maxMoves)
        {
            if (IsValidBoard())
            {
                DisplayAlert("Result", "You won the game, congratulations!", "Okay");

                total_wins++;
                current_streak++;
            }
            else
            {
                DisplayAlert("Game over", "you lose the game, try to do better next time.", "Okay");
                current_streak = 0;
            }

            Navigation.PopAsync();
        }
    }

    private void GridClick(object sender, EventArgs e)
    {
        // reset last selected grid
        if (currGrid is not null)
        {
            if (currGrid.BackgroundColor == Colors.Gray)
            {
                currGrid.TextColor = Colors.Gray;
            }
            else if (currGrid.BackgroundColor == notSafe)
            {
                currGrid.TextColor = notSafe;
            }
            else if (currGrid.BackgroundColor == selectGrid)
            {
                currGrid.TextColor = safeColorText;
            }
            else
            {
                currGrid.TextColor = Colors.Black;
            }

            currGrid.Background = Colors.White;
            currGrid.BackgroundColor = Colors.White;
        }

        currGrid = (Button)sender;
        int row = Grid.GetRow(currGrid.Parent);
        int col = Grid.GetColumn(currGrid.Parent);

        //Reset
        ResetColorRowCol(lastGridIndex.Item1, lastGridIndex.Item2);
        lastGridIndex = (row, col);

        HighlightRowCol(row, col);

        if (mat[row, col] == 0)
        {
            if (currGrid.TextColor == notSafe)
            {
                currGrid.BackgroundColor = notSafe;
                currGrid.Background = notSafe;
            }
            else
            {
                currGrid.BackgroundColor = safeColorText;
                currGrid.Background = safeColorText;
            }
        }
        else
        {
            currGrid.BackgroundColor = selectGrid;
            currGrid.Background = selectGrid;
        }

        currGrid.TextColor = Colors.WhiteSmoke;

    }
    private void HighlightRowCol(int row, int col)
    {
        // Check duplicates on row
        for (int i = 0; i < 9; i++)
        {
            gridButtons[row, i].Background = selectedRowColumn;
        }

        // Check duplicates on column
        for (int i = 0; i < 9; i++)
        {
            gridButtons[i, col].Background = selectedRowColumn;
        }

        int startRow = row - (row % 3);
        int startColumn = col - (col % 3);
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                gridButtons[i + startRow, j + startColumn].Background = selectedRowColumn;
            }
        }
    }

    private void ResetColorRowCol(int row, int col)
    {
        // Check duplicates on row
        for (int i = 0; i < 9; i++)
        {
            gridButtons[row, i].Background = Colors.White;
        }

        // Check duplicates on column
        for (int i = 0; i < 9; i++)
        {
            gridButtons[i, col].Background = Colors.White;
        }

        int startRow = row - (row % 3);
        int startColumn = col - (col % 3);
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                gridButtons[i + startRow, j + startColumn].Background = Colors.White;
            }
        }
    }

    private void Erase_Clicked(object sender, EventArgs e)
    {
        if (currGrid is null) return;

        int row = Grid.GetRow(currGrid.Parent);
        int col = Grid.GetColumn(currGrid.Parent);

        if (mat[row, col] == 0)
        {
            currGrid.Text = "";
            answerMat[row, col] = 0;
            moves--;
        }
    }

    private void Notes_Clicked(object sender, EventArgs e)
    {
        noteMode ^= true;
        lbl_NoteMode.Text = noteMode ? "Note mode: TRUE" : "Note mode: FALSE";
    }
}
