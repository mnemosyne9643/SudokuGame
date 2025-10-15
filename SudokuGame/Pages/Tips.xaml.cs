using System.Collections.ObjectModel;

namespace SudokuGame.Pages;

public partial class Tips : ContentPage
{
    public ObservableCollection<TipPage> TipPages { get; set; }
    private int currentIndex = 0;

    public Tips()
	{
		InitializeComponent();
        TipPages = new ObservableCollection<TipPage>
            {
                new TipPage { Image = "one.jpg", Description = "The goal is to fill in all the empty squares on the board with numbers" },
                new TipPage { Image = "two.jpg", Description = "The board has 9 individual regions, 9 rows and 9 columns" },
                new TipPage { Image = "three.jpg", Description = "Each number from 1 to 9 appears only once in any of these regions and columns and rows" },
                new TipPage { Image = "four.jpg", Description = "To solve the puzzle, select an empty square, and choose a number to place in it" },
                new TipPage { Image = "five.jpg", Description = "You can use notes to note which number you believe to be possible for a square" },
                new TipPage { Image = "six.jpg", Description = "Use HINTS when you get stuck" }
            };
        TipsCarousel.ItemsSource = TipPages;
        TipsCarousel.Position = 0;

        // Keep index updated if user swipes
        TipsCarousel.PositionChanged += (s, e) =>
        {
            currentIndex = e.CurrentPosition;
        };
    }
    private void PrevButton_Clicked(object sender, EventArgs e)
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            TipsCarousel.Position = currentIndex;
        }
    }

    private void NextButton_Clicked(object sender, EventArgs e)
    {
        if (currentIndex < TipPages.Count - 1)
        {
            currentIndex++;
            TipsCarousel.Position = currentIndex;
        }
    }

    private async void OkButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    public class TipPage
    {
        public string Image { get; set; }
        public string Description { get; set; }
    }
    

}
