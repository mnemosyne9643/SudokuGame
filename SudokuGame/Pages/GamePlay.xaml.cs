using Microsoft.Maui.Controls.Shapes;

namespace SudokuGame.Pages;

public partial class GamePlay : ContentPage
{
    private string[,] SudokuGame = {
        {"", "", "", "", "", "", "", "", ""},
        {"", "", "", "", "", "", "", "", ""},
        {"", "", "", "", "", "", "", "", ""},
        {"", "", "", "", "", "", "", "", ""},
        {"", "", "", "", "", "", "", "", ""},
        {"", "", "", "", "", "", "", "", ""},
        {"", "", "", "", "", "", "", "", ""},
        {"", "", "", "", "", "", "", "", ""},
        {"", "", "", "", "", "", "", "", ""},
        };

    private Dictionary<string, HashSet<string>> rows = new();
    private Dictionary<string, HashSet<string>> columns = new();
    private Dictionary<string, HashSet<string>> box = new();

    private Button? currGrid = null;

	public GamePlay()
	{
		InitializeComponent();
    }

    private void GridClick(object sender, EventArgs e)
    {
        if (currGrid is not null)
        {
            currGrid.BackgroundColor = Colors.White;

            if (currGrid.TextColor == Colors.Red || currGrid.TextColor == Colors.IndianRed)
            {
                currGrid.TextColor = Colors.IndianRed;
            }
            else
            {
                currGrid.TextColor = Colors.Black; 
            }
        }

        currGrid = (Button)sender;
        currGrid.BackgroundColor = Colors.MediumBlue;
        currGrid.TextColor = Colors.WhiteSmoke;
    }
    private void Numpad(object sender, EventArgs e)
    {
        if (currGrid is null) return;

        string number = ((Button)sender).Text;

        int row = Grid.GetRow(currGrid.Parent);
        int col = Grid.GetColumn(currGrid.Parent);
        // Get the current grid using borders
        string row_key = row.ToString();
        string col_key = col.ToString();
        string box_key = $"{row / 3},{col / 3}";

        if (!rows.ContainsKey(row_key)) rows[row_key] = new();
        if (!columns.ContainsKey(col_key)) columns[col_key] = new();
        if (!box.ContainsKey(box_key)) box[box_key] = new();
        
        currGrid.Text = number;
        currGrid.TextColor = Colors.Black;

        if (rows[row_key].Contains(number))
        {
            currGrid.TextColor = Colors.Red;
        }
        else
        {
            rows[row_key].Add(number);
        }

        if (columns[col_key].Contains(number))
        {
            currGrid.TextColor = Colors.Red;
        }
        else
        {
            columns[col_key].Add(number);
        }

        if (box[box_key].Contains(number))
        {
            currGrid.TextColor = Colors.Red;
        }
        else
        {
            box[box_key].Add(number);
        }
    }
}
