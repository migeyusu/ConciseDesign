namespace ConciseDesign.WPF.CustomControls
{
    /// <summary>
    /// a procedure can be confirmed before submit.
    /// can be used in dialog or form 
    /// </summary>
    public interface IDialogContent
    {
        /// <summary>
        /// confirm the user input 
        /// </summary>
        bool TrySubmit();
    }
}