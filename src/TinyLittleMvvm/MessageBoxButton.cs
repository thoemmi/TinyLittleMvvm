namespace TinyLittleMvvm {
    /// <summary>
    /// Specifies the buttons that are displayed on a message box. Used as an
    /// argument of the <see cref="IWindowManager.ShowMessageBox"/> method.
    /// </summary>
    public enum MessageBoxButton {
        /// <summary>
        /// The message box displays an <b>OK</b> button.
        /// </summary>
        OK = System.Windows.MessageBoxButton.OK,

        /// <summary>
        /// The message box displays <b>OK</b> and <b>Cancel</b> buttons.
        /// </summary>
        OKCancel = System.Windows.MessageBoxButton.OKCancel,

        /// <summary>
        /// The message box displays <b>Yes</b>, <b>No</b>, and <b>Cancel</b> buttons.
        /// </summary>
        YesNoCancel = System.Windows.MessageBoxButton.YesNoCancel,

        /// <summary>
        /// The message box displays <b>Yes</b> and <b>No</b> buttons.
        /// </summary>
        YesNo = System.Windows.MessageBoxButton.YesNo,
    }
}