namespace ConciseDesign.WPF.CustomControls
{
    /// <summary>
    /// 用于可提交的对话矿
    /// </summary>
    public interface IDialogContent
    {
        /// <summary>
        /// 尝试提交，实现应进行检验
        /// </summary>
        bool TrySubmit();
    }
}