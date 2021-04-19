namespace TinyLittleMvvm {
    /// <summary>
    /// Specifies which message box button that a user clicks. <see cref="MessageBoxResult"/> is returned by
    /// the <see cref="IWindowManager.ShowMessageBox"/> method.
    /// </summary>
    public enum MessageBoxResult {
        /// <summary>
        /// The message box returns no result.
        /// </summary>
        None = System.Windows.MessageBoxResult.None,

        /// <summary>
        /// The result value of the message box is <b>OK</b>.
        /// </summary>
        OK = System.Windows.MessageBoxResult.OK,

        /// <summary>
        /// The result value of the message box is <b>Cancel</b>.
        /// </summary>
        Cancel = System.Windows.MessageBoxResult.Cancel,

        /// <summary>
        /// The result value of the message box is <b>Yes</b>.
        /// </summary>
        Yes = System.Windows.MessageBoxResult.Yes,

        /// <summary>
        /// The result value of the message box is <b>No</b>.
        /// </summary>
        No = System.Windows.MessageBoxResult.No,
    }
}