namespace Framework.Core.Wizard
{
    public interface IWizard
    {
        int CurrentStep { get; set; }

        void NavigateToNextPage();
        
        void SubmitWizard();
    }
}
