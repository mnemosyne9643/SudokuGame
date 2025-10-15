namespace SudokuGame
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            Window window = new Window(new AppShell());

            window.Width = 550;
            window.Height = 800;

            return window;
        }
    }
}
