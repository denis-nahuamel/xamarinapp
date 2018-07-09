using xamainazureapp.ViewModels;

namespace xamainazureapp.Infraestructure
{
    public class InstanceLocator
    {
        public MainViewModel Main {get; set;}
        public InstanceLocator()
        {
            Main =new MainViewModel();
        }
    }
}
