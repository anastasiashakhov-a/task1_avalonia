namespace BasicMvvmSample.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public SimpleViewModel SimpleViewModel { get; } = new SimpleViewModel();

    }
}
