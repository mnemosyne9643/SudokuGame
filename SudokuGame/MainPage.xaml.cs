namespace SudokuGame
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
           
        }
        private async void StartGameButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Pages.StartGame());
        }
        private async void TipsButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Pages.Tips());
        }

    }

}
